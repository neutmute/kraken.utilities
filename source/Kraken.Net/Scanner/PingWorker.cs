using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Net.NetworkInformation;
using System.Threading;
using Kraken.Net.Scanner;
using NLog;

namespace Kraken.Net
{
    public class PingWorker : IDisposable
    {
        #region Fields
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();
        private NetworkTestWorker _networkTestWorker;
        private readonly ConcurrentQueue<PingRequest> _workQueue;
        private CancellationToken _cancellationToken;

        private readonly string _scanVerb;

        #endregion

        #region Events

        public event EventHandler<ScanProgressDetailEventArgs> ProgressDetailUpdate;

        #endregion
        
        #region Properties

        public int Index { get; set; }

        #endregion

        #region Ctor

        public PingWorker(ConcurrentQueue<PingRequest> workQueue, CancellationToken cancellationToken, string scanVerb, Func<NetworkTestWorker> networkTestFactory)
        {
            _cancellationToken = cancellationToken;
            _scanVerb = scanVerb;
            _workQueue = workQueue;

            _networkTestWorker = networkTestFactory();
            _networkTestWorker.Completed += PingCompleted;
        }

        #endregion

        #region Methods

        public void ProcessQueue()
        {
            PingLog(LogLevel.Trace, "Starting");
            StartNewPing();
        }

        private void AlertProgressDetail(ScanProgressDetail scanDetail)
        {
            if (ProgressDetailUpdate != null)
            {
                ProgressDetailUpdate(this, new ScanProgressDetailEventArgs { Item = scanDetail });
            }
        }

        private void StartNewPing()
        {
            PingRequest pingRequest;
            while (_workQueue.TryDequeue(out pingRequest))
            {
                if (_cancellationToken.IsCancellationRequested)
                {
                    PingLog(LogLevel.Trace, "Exiting due to cancel request");
                    return;
                }

                PingLog(LogLevel.Trace, "{0}", pingRequest.NetworkAddress);
                
                var operation = string.Format("{0} scanning...", _scanVerb);
                AlertProgressDetail(new ScanProgressDetail(pingRequest.NetworkAddress, operation));

                _networkTestWorker.Execute(pingRequest);
            }
            PingLog(LogLevel.Trace, "Exiting due to empty queue");
        }

        void PingCompleted(object sender, NetworkTestResultEventArgs e)
        {
            var pingRequest = e.Request;
            pingRequest.Success = e.Success;
            pingRequest.State = PingRequestState.Complete;

            if (pingRequest.Success)
            {
                PingLog("Reply from {0}", pingRequest.NetworkAddress);
                var scanDetail = new ScanProgressDetail(pingRequest.NetworkAddress, _scanVerb, e.Message);
                scanDetail.Tag = pingRequest;
                AlertProgressDetail(scanDetail);
            }
            else
            {
                pingRequest.Error = e.Message;
                PingLog(LogLevel.Trace, "Fail from {0} with {1}", pingRequest.NetworkAddress, pingRequest.Error);
                AlertProgressDetail(new ScanProgressDetail(pingRequest.NetworkAddress, _scanVerb, pingRequest.Error, true));
            }

            StartNewPing();
        }

        private void PingLog(string messageFormat, params object[] args)
        {
            PingLog(LogLevel.Info, messageFormat, args);
        }

        private void PingLog(LogLevel logLevel, string messageFormat, params object[] args)
        {
            string message = string.Format("{0}{2}{1}: ", GetType().Name, Index, _scanVerb) + string.Format(messageFormat, args);
            Log.Log(logLevel, message);
        }

        public void Dispose()
        {
            if (_networkTestWorker != null)
            {
                _networkTestWorker.Dispose();
                _networkTestWorker = null;
            }
        }

        #endregion

    }
}