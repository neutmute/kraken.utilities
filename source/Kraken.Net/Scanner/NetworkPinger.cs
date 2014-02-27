using Kraken.Net.Scanner;

namespace Kraken.Net
{
    public class NetworkPinger : NetworkScanner
    {
        public NetworkPinger()
        {
            ScanVerb = "Ping";
            NetworkTestFactory = () => { return new PingTestWorker(); };
        }
    }
}