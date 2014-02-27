using System;
using System.Net.NetworkInformation;

namespace Kraken.Net.Scanner
{
    public class PingTestWorker : NetworkTestWorker
    {
        protected override void DoPing(PingRequest pingRequest)
        {
            Ping ping = new Ping();
            bool success;
            string message;
            try
            {
                var reply = ping.Send(pingRequest.NetworkAddress.Address);
                success = reply.Status == IPStatus.Success;
                message = reply.Status.ToString();
            }
            catch (Exception e)
            {
                success = false;
                message = e.Message;
            }
            finally
            {
                ping.Dispose();
            }

            pingRequest.Success = success;
            pingRequest.Error = message;

            FireCompleted(pingRequest, success, message);
        }

        public override void Dispose()
        {
        }
    }
}