using System;
using System.Collections.ObjectModel;
using System.IO;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using Kraken.Core;
using Common.Logging;
using Kraken.Web;

namespace Kraken.Web
{
    /// <summary>
    /// Applying this attribute to any WCF service class will automatically publish any unhandled exceptions using ExceptionManager.Publish().
    /// The exception will still be passed to the client as a FaultException. If you wish to alter the exception before passing to the client,
    /// create your own attribute that inherits from this class and override the ProvideFault method.
    /// </summary>
    /// <example>
    /// [ServiceBehavior]
    /// [WhaleErrorServiceBehaviorAttribute]
    /// public class MyService : IMyContract
    /// { ... }
    /// </example>
    [AttributeUsage(AttributeTargets.Class)]
    public class KrakenWcfErrorHandler : Attribute, IServiceBehavior, IErrorHandler
    {
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Where to dump captures of bad WCF requests
        /// </summary>
        public static string LogFolder { get; set; }

        #region IServiceBehavior Members
        public virtual void AddBindingParameters(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase, Collection<ServiceEndpoint> endpoints, BindingParameterCollection bindingParameters)
        {
            // No implementation required
        }

        /// <summary>
        /// Adds this class as an error handler for the service to which the attribute has been applied.
        /// </summary>
        public virtual void ApplyDispatchBehavior(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {
            foreach (ChannelDispatcher dispatcher in serviceHostBase.ChannelDispatchers)
            {
                dispatcher.ErrorHandlers.Add(this);
            }
        }

        public virtual void Validate(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {
            // No implementation required
        }

        #endregion

        #region IErrorHandler Members

        /// <summary>
        /// Handles any unhandled exception after it has been sent to the client as a FaultException and publishes it using ExceptionManager.Publish.
        /// </summary>
        public virtual bool HandleError(Exception error)
        {
            // Return false to allow other handlers to be called.
            return false;
        }

        public virtual void ProvideFault(Exception error, MessageVersion version, ref Message fault)
        {
            Log.Fatal(error);

            if (OperationContext.Current == null)
            {
                Log.Warn(m => m("OperationContext.Current was null. Cannot record inputs that caused fault", typeof(KrakenWcfParameterInspector)));
                return;
            }

            object[] inputs = null;
            string operationName = null;

            if (OperationContext.Current.IncomingMessageProperties.ContainsKey(KrakenWcfParameterInspector.OperationInputKey))
            {
                inputs = OperationContext.Current.IncomingMessageProperties[KrakenWcfParameterInspector.OperationInputKey] as object[];
            }

            if (OperationContext.Current.IncomingMessageProperties.ContainsKey(KrakenWcfParameterInspector.OperationNameKey))
            {
                operationName = OperationContext.Current.IncomingMessageProperties[KrakenWcfParameterInspector.OperationNameKey] as string;
            }

            if (inputs == null || operationName == null)
            {
                Log.Warn(m => m(
                    "{0} was not applied. Cannot record inputs that caused fault. This can happen if the method is parameterless like on Ping()"
                    , typeof(KrakenWcfParameterInspector)));
            }
            else
            {
                try
                {
                    if (string.IsNullOrEmpty(LogFolder))
                    {
                        Log.Error("KrakenWcfErrorHandler.LogFolder is not set. Set it to capture WCF inputs");
                    }
                    else
                    {
                        byte[] binBytes = Kelvin<object[]>.ToBinary(inputs);
                        string filename = string.Format("{0}_failure_{1:yyyyMMdd_HHmmss}.bin", operationName, SystemDate.Now);

                        string targetFilename = Path.Combine(LogFolder, filename);
                        Log.Info(m => m("{0} failure: Writing input parameters to {1}", operationName, targetFilename));
                        File.WriteAllBytes(targetFilename, binBytes);    
                    }

                }
                catch (Exception e)
                {
                    // If not marked serialiseable then ToBinary fails
                    Log.Error(m=>m("Unable to save WCF error message"), e);
                }
            }
        }

        #endregion
    }
}
