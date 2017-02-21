using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Threading;
using System.Threading.Tasks;
using Kraken.Net.Scanner;
using Common.Logging;

namespace Kraken.Net
{
    public class NetworkScanner
    {
        #region Fields

        private CancellationTokenSource _cancelSource;
        private CancellationToken _cancelToken;
        private ConcurrentQueue<PingRequest> _workQueue;
        private List<PingRequest> _pingRequests;
        private List<PingWorker> _pingWorkers;
        private static readonly ILog Log = LogManager.GetLogger("NetworkScanner");
        #endregion

        #region Events

        public event EventHandler<ProgressEventArgs> ProgressUpdate;
        public event EventHandler<ScanProgressDetailEventArgs> ProgressDetailUpdate;
        #endregion
        
        #region Properties
        public Func<NetworkTestWorker> NetworkTestFactory { get; set; }

        public string ScanVerb { get; set; }
        public int ThreadCount { get; set; }
        #endregion
        
        #region Ctor

        public NetworkScanner()
        {
            _cancelSource = new CancellationTokenSource();
            _cancelToken = _cancelSource.Token;
            ThreadCount = 10;
            _pingRequests = new List<PingRequest>();
            _workQueue = new ConcurrentQueue<PingRequest>();
        }

        #endregion
        
        #region Methods


        public void Execute(List<NetworkAddress> addresses)
        {
            Log.Info(m => m("{2} scanning {0} IP's with {1} threads", addresses.Count, ThreadCount, ScanVerb));
            BuildWorkQueue(addresses);
            ExecuteWorkers();
            AlertProgress();
        }

        public void Cancel()
        {
            _cancelSource.Cancel();
        }

        private void BuildWorkQueue(List<NetworkAddress> addresses)
        {
            for (int i = 0; i < addresses.Count; i++)
            {
                var pingRequest = new PingRequest(addresses[i]);
                _workQueue.Enqueue(pingRequest);
                _pingRequests.Add(pingRequest);
            }
        }

        private void ExecuteWorkers()
        {
            _pingWorkers = new List<PingWorker>();
            Task[] tasks = new Task[ThreadCount];

            TimeSpan reportProgressTimespan = TimeSpan.FromSeconds(1);
            Timer reportTimer = new Timer(s => AlertProgress(), null, reportProgressTimespan, reportProgressTimespan);
            
            for (int i = 0; i < ThreadCount; i++)
            {
                PingWorker pingWorker = new PingWorker(_workQueue, _cancelToken, ScanVerb, NetworkTestFactory);
                pingWorker.Index = i;
                pingWorker.ProgressDetailUpdate += BubbleProgressDetailUpdate;
                _pingWorkers.Add(pingWorker);

                Task task = new Task(pingWorker.ProcessQueue, TaskCreationOptions.AttachedToParent | TaskCreationOptions.LongRunning);
                tasks[i] = task;
                task.Start();
            }

            Task.WaitAll(tasks, _cancelToken); 
            reportTimer.Dispose();
        }

        void BubbleProgressDetailUpdate(object sender, ScanProgressDetailEventArgs e)
        {
            if (ProgressDetailUpdate != null)
            {
                ProgressDetailUpdate(this, e);
            }
        }

        private void AlertProgress()
        {
            var progress = GetProgress();
            Log.Trace(m => m("{1} - {0}", progress, ScanVerb));

            string message = string.Format("{1} scan {0}% complete", progress.PercentComplete, ScanVerb);
            if (progress.PercentComplete == 100)
            {
                message += string.Format(". {0}/{1} hosts responded", progress.ReplyCount, progress.TotalToScan);
            }
            Log.Info(message);

            if (ProgressUpdate != null)
            {
                ProgressUpdate(this, progress);
            }
        }

        public PingProgress GetProgress()
        {
            var progress = new PingProgress();
            progress.CurrentTask = string.Format("{0} scanning range", ScanVerb);
            progress.TotalToScan = _pingRequests.Count;
            progress.ScansCompleted = _pingRequests.Where(r => r.State == PingRequestState.Complete).Count();
            progress.ReplyCount = _pingRequests.Where(r => r.Success).Count();
            return progress;
        }

        public  List<PingRequest> GetResults()
        {            
            _pingRequests.Sort((x, y) => x.Success.CompareTo(y.Success));
            return _pingRequests;
        }
        #endregion
    }
}