//using System;
//using System.Collections.Generic;
//using System.Collections.ObjectModel;
//using System.Linq;
//using System.ServiceModel;
//using System.ServiceModel.Channels;
//using System.ServiceModel.Description;
//using System.ServiceModel.Dispatcher;
//using System.Text;
//using Kraken.Core.Web.Behaviours.PerfMon;

//namespace Kraken.Core
//{
//    [AttributeUsage(AttributeTargets.Class)]
//    public class KrakenWcfPerfMonBehaviorAttribute : Attribute, IServiceBehavior
//    {
//        #region Properties
//        public int WarnLongerThanMilliseconds { get; set; }
//        #endregion

//        public KrakenWcfPerfMonBehaviorAttribute()
//        {
//            WarnLongerThanMilliseconds = int.MaxValue;
//        }

//        #region Methods
//        public void AddBindingParameters(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase, Collection<ServiceEndpoint> endpoints, BindingParameterCollection bindingParameters)
//        {
//        }

//        public void ApplyDispatchBehavior(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
//        {
//            foreach (ChannelDispatcher cDispatcher in serviceHostBase.ChannelDispatchers)
//            {
//                foreach (EndpointDispatcher eDispatcher in cDispatcher.Endpoints)
//                {
//                    eDispatcher.DispatchRuntime.MessageInspectors.Add(new KrakenWcfPerfMonMessageInspector { WarnLongerThanMilliseconds = WarnLongerThanMilliseconds });
//                }
//            }
//        }

//        public void Validate(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
//        {
//        }
//        #endregion
//    }
//}
