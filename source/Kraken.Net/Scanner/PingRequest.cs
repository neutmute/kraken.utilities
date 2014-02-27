namespace Kraken.Net
{
    public enum PingRequestState
    {
        Unactioned = 0,
        InProgress = 1,
        Complete = 2
    }

    public class PingRequest
    {
        public bool Success {get;set;}

        public PingRequestState State { get; set; }

        public NetworkAddress NetworkAddress { get; set; }

        public string Error {get;set;}

        public object Tag { get; set; }

        public PingRequest(NetworkAddress networkAddress)
        {
            NetworkAddress = networkAddress;
        }

        public override string ToString()
        {
            return string.Format("{0}, State={1}, Success={2} {3}", NetworkAddress, State, Success, Error);
        }
    }
}