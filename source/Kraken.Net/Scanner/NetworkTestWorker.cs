using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using NLog;

namespace Kraken.Net.Scanner
{
    public abstract class NetworkTestWorker : IDisposable
    {
        #region Fields/Events

        private static readonly Logger _log = LogManager.GetCurrentClassLogger();

        public event EventHandler<NetworkTestResultEventArgs> Completed;
        protected PingRequest PingRequest { get; set; }
        #endregion

        #region Properties

        protected Logger Log
        {
            get { return _log; }
        }

        #endregion
        
        #region Methods
        public void Execute(PingRequest request)
        {
            PingRequest = request;

            DoPing(request);
        }

        protected abstract void DoPing(PingRequest request);

        protected void FireCompleted(PingRequest request, bool success, string message)
        {
            FireCompleted(new NetworkTestResultEventArgs {Request = request, Success = success, Message = message});
        }

        protected void FireCompleted(NetworkTestResultEventArgs eventArgs)
        {
            if (Completed != null)
            {
                Completed(this, eventArgs);
            }
        }

        public abstract void Dispose();
        #endregion
    }
}
