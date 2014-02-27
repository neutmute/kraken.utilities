using System;

namespace Kraken.Net.Scanner
{
    public  class NetworkTestResultEventArgs : EventArgs
    {
        public PingRequest Request { get; set; }

        public bool Success { get; set; }

        public string Message { get; set; }

        public object Tag { get; set; }
    }
}