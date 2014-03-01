using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using System.Web;

namespace Kraken.Web
{
    public class KrakenWcfParameterInspector : Attribute, IServiceBehavior, IParameterInspector
    {
        #region Fields
        public const string OperationInputKey = "OperationInput";
        public const string OperationNameKey = "OperationName";
        #endregion

        #region Behaviour

        public void ApplyDispatchBehavior(ServiceDescription desc, ServiceHostBase host)
        {
            foreach (ChannelDispatcher cDispatcher in host.ChannelDispatchers)
            {
                foreach (EndpointDispatcher eDispatcher in cDispatcher.Endpoints)
                {
                    foreach (DispatchOperation op in eDispatcher.DispatchRuntime.Operations)
                    {
                        op.ParameterInspectors.Add(this);
                    }
                }
            }
        }

        public void AddBindingParameters(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase, Collection<ServiceEndpoint> endpoints, BindingParameterCollection bindingParameters)
        {
        }

        public void Validate(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {
        }

        #endregion

        #region Param Inspector
        public void AfterCall(string operationName, object[] outputs, object returnValue, object correlationState)
        {
        }

        /// <summary>
        /// Save for later so the error handler can pull them out for tracing
        /// </summary>
        public object BeforeCall(string operationName, object[] inputs)
        {
            OperationContext.Current.IncomingMessageProperties[OperationInputKey] = inputs;
            OperationContext.Current.IncomingMessageProperties[OperationNameKey] = operationName;
            return null;
        }

        #endregion
    }
}