using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using LukeSkywalker.IPNetwork;
using Common.Logging;
using IPAddressCollection = LukeSkywalker.IPNetwork.IPAddressCollection;

namespace Kraken.Net
{
    public class IpContainer
    {
        public IPAddress Address {get;set;}

        public IPAddress Subnet { get; set; }

        public override string ToString()
        {
            return string.Format("ip={0}, subnet={1}", Address, Subnet);
        }

    }

    public class NetworkService
    {
        #region Fields

        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        #endregion
        
        #region Methods

        public List<NetworkInterface> GetActiveNetworkInterfaces()
        {
            return NetworkInterface.GetAllNetworkInterfaces().Where(
                n => n.OperationalStatus != OperationalStatus.Down
                     && n.NetworkInterfaceType != NetworkInterfaceType.Tunnel
                     && n.NetworkInterfaceType != NetworkInterfaceType.Loopback
                     && n.GetIPv4Statistics().UnicastPacketsReceived > 0).ToList();
        }

        public List<IpContainer> GetPrimaryIPAddresses()
        {
            var mostPrimaryNic = GetMostPrimaryNic();
            return GetPrimaryIPAddresses(mostPrimaryNic);
        }

        public List<IpContainer> GetPrimaryIPAddresses(NetworkInterface networkInterface)
        {
            return GetPrimaryIPAddresses(new List<NetworkInterface>{networkInterface});
        }

        public List<IpContainer> GetPrimaryIPAddresses(List<NetworkInterface> networkInterfaces)
        {
            List<IpContainer> ipAddresses = new List<IpContainer>();

            foreach (var networkInterface in networkInterfaces)
            {
                IPInterfaceProperties properties = networkInterface.GetIPProperties();

                var thisNicsIPs = properties.UnicastAddresses
                    .Where(ua => ua.Address.AddressFamily == AddressFamily.InterNetwork)
                    .Select(ua => new IpContainer {Address = ua.Address, Subnet = ua.IPv4Mask})
                    .ToList();

                ipAddresses.AddRange(thisNicsIPs);
            }

            return ipAddresses;
        }

        public PhysicalAddress GetPrimaryPhysicalAddress()
        {
            return GetMostPrimaryNic().GetPhysicalAddress();
        }

        private NetworkInterface GetMostPrimaryNic()
        {
            var primaryNics = GetActiveNetworkInterfaces();
            var mostPrimaryNic = primaryNics.OrderByDescending(n => n.GetIPv4Statistics().UnicastPacketsReceived).First();

            if (primaryNics.Count > 0)
            {
                Log.Trace(m => m("Selected '{0}' as primary NIC", mostPrimaryNic.Name));
            }
            return mostPrimaryNic;
        }

        public IPAddressCollection GetIpRange(IpContainer ipContainer)
        {
            IPNetwork ipnetwork = GetIPNetwork(ipContainer);
            return IPNetwork.ListIPAddress(ipnetwork);
        }

        public IPNetwork GetIPNetwork(IpContainer ipContainer)
        {
            return IPNetwork.Parse(ipContainer.Address, ipContainer.Subnet);
        }

        public List<NetworkAddress> GetRange(string from, string to)
        {
            return GetRange(new NetworkAddress(from), new NetworkAddress(to));
        }

        public List<NetworkAddress> GetRange(NetworkAddress from, NetworkAddress to)
        {
            RangeFinder rangeFinder = new RangeFinder();
            List<NetworkAddress> addresses = new List<NetworkAddress>();
            foreach (IPAddress address in rangeFinder.GetIPRange(from.Address, to.Address))
            {
                addresses.Add(new NetworkAddress(address));
            }
            return addresses;
        }

        /// <summary>
        /// Safe way of obtaining a hostname
        /// </summary>
        public static string GetHostname(IPAddress ipAddress)
        {
            string hostname;
            try
            {
                IPHostEntry hostInfo = Dns.GetHostEntry(ipAddress);
                hostname = hostInfo.HostName;
            }
            catch (SocketException e)
            {
                Log.Debug(e.Message);
                hostname = string.Format("{0} ({1})", ipAddress, e.Message);
            }
            return hostname;
        }

        public static string GetHostname(string ipAddressString)
        {
            IPAddress ipAddress;
            var parsedOk = IPAddress.TryParse(ipAddressString, out ipAddress);
            if (parsedOk)
            {
                return GetHostname(ipAddress);
            }
            return ipAddressString;
        }

        #endregion
    }
}
