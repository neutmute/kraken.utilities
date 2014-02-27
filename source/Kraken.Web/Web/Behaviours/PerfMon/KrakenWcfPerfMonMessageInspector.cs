// reqyures nstatsD
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.ServiceModel;
//using System.ServiceModel.Channels;
//using System.ServiceModel.Dispatcher;
//using System.Text;
//using NLog;
//using NStatsD;

//namespace Kraken.Core.Web.Behaviours.PerfMon
//{
//    public class KrakenWcfPerfMonMessageInspector : IDispatchMessageInspector
//    {
//        #region Fields
//        public int WarnLongerThanMilliseconds {get;set;}
//        private static readonly Logger Log = LogManager.GetCurrentClassLogger();
//        #endregion

//        #region Ctor
//        public KrakenWcfPerfMonMessageInspector()
//        {
//            WarnLongerThanMilliseconds = int.MaxValue;
//        }
//        #endregion
        
//        #region IDispatchMessageInspector Members

//        public object AfterReceiveRequest(ref Message request, IClientChannel channel, InstanceContext instanceContext)
//        {
//            var metadata = WcfChannelLogic.GetMessageMetadata(request);
//            var statBucket = NStatsDClient.With("WCF", metadata.Interface, metadata.Operation).BeginTimer();
//            return statBucket;
//        }

//        public void BeforeSendReply(ref Message reply, object correlationState)
//        {
//            var statBucket = (StatBucket) correlationState;
//            var duration = statBucket.EndTimer();

//            var milliSeconds = Convert.ToInt32(duration.TotalMilliseconds);
//            if (milliSeconds > WarnLongerThanMilliseconds)
//            {
//                Log.Warn("{0} took {1}ms to reply", statBucket.Name, milliSeconds);
//            }
//        }
//        #endregion
//    }
//}
