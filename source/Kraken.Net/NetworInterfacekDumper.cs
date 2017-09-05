using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;

namespace Kraken.Net
{
    public class NetworInterfacekDumper
    {
        private readonly List<NetworkInterface> _networkInterfaces;
        private StringBuilder _accumulatedDump;

        public NetworInterfacekDumper(IEnumerable<NetworkInterface> networkInterfaces)
        {
            _networkInterfaces = networkInterfaces.ToList();
        }

        public NetworInterfacekDumper()
        {
            _networkInterfaces = NetworkInterface.GetAllNetworkInterfaces().ToList();
        }

        public string Dump()
        {
            _accumulatedDump = new StringBuilder();

            Emit("Number of Network Interfaces .................... : {0}", _networkInterfaces.Count);
            foreach (NetworkInterface adapter in _networkInterfaces)
            {
                ShowAdapter(adapter);
            }

            return _accumulatedDump.ToString();
        }
        
        private void ShowAdapter(NetworkInterface adapter)
        {
            IPInterfaceProperties properties = adapter.GetIPProperties();
            Emit();
            Emit("{0} - {1}", adapter.Name, adapter.Description);
            Emit(String.Empty.PadLeft(adapter.Description.Length, '='));
            Emit("  Id ........... .......................... : {0}", adapter.Id);
            Emit("  Interface type .......................... : {0}", adapter.NetworkInterfaceType);
            Emit("  Physical Address ........................ : {0}",
                 adapter.GetPhysicalAddress().ToString(true));
            Emit("  Operational status ...................... : {0}",
                 adapter.OperationalStatus);
            string versions = "";


            // Create a display string for the supported IP versions.
            if (adapter.Supports(NetworkInterfaceComponent.IPv4))
            {
                versions = "IPv4";
            }
            if (adapter.Supports(NetworkInterfaceComponent.IPv6))
            {
                if (versions.Length > 0)
                {
                    versions += ", ";
                }
                versions += "IPv6";
            }
            Emit("  IP version .............................. : {0}", versions);
            ShowIPAddresses(properties);


            ShowGatewayAddresses(properties.GatewayAddresses);

            // The following information is not useful for loopback adapters.
            if (adapter.NetworkInterfaceType == NetworkInterfaceType.Loopback)
            {
                return;
            }
            Emit("  DNS suffix .............................. : {0}",
                 properties.DnsSuffix);

            string label;
            if (adapter.Supports(NetworkInterfaceComponent.IPv4))
            {
                IPv4InterfaceProperties ipv4 = properties.GetIPv4Properties();
                if (ipv4 != null)
                {
                    Emit("  MTU...................................... : {0}", ipv4.Mtu);
                    if (ipv4.UsesWins)
                    {

                        //var winsServers = properties.WinsServersAddresses;
                        //if (winsServers.Count > 0)
                        //{
                        //    label = "  WINS Servers ............................ :";
                        //    ShowIPAddresses(label, winsServers);
                        //}
                    }
                }
            }

            Emit("  DNS enabled ............................. : {0}",
                 properties.IsDnsEnabled);
            Emit("  Dynamically configured DNS .............. : {0}",
                 properties.IsDynamicDnsEnabled);
            Emit("  Receive Only ............................ : {0}",
                 adapter.IsReceiveOnly);
            Emit("  Multicast ............................... : {0}",
                 adapter.SupportsMulticast);
            ShowInterfaceStatistics(adapter);

            Emit();
        }

        private void ShowGatewayAddresses(GatewayIPAddressInformationCollection gateways)
        {
            foreach (GatewayIPAddressInformation gatewayAddress in gateways)
            {
                Emit("  Gateway.................................. : {0}", gatewayAddress.Address);
            }
        }

        private void ShowIPAddresses(string label, System.Net.IPAddressCollection winsServers)
        {
            Emit("-----" + label + "-----");
            ShowIPAddressCollection(winsServers);
        }

        private void ShowIPAddressCollection(System.Net.IPAddressCollection collection)
        {
            for (int i = 0; i < collection.Count; i++)
            {
                Emit(collection[i].ToString(true));
            }
        }
        
        private void ShowInterfaceStatistics(NetworkInterface adapter)
        {
            IPv4InterfaceStatistics stats = adapter.GetIPv4Statistics();
            Emit(" Stats:");
            Emit("  Packets Received ....... : {0}",
            stats.UnicastPacketsReceived);
            Emit("  Bytes Sent ............. : {0}",
            stats.BytesSent);
        }

        private void ShowIPAddresses(IPInterfaceProperties properties)
        {
            foreach (UnicastIPAddressInformation ipInfo in properties.UnicastAddresses)
            {
                Emit("  IPAddress ({1})................. : {0}",
                ipInfo.Address
                , ipInfo.Address.AddressFamily);

                if (ipInfo.Address.AddressFamily == AddressFamily.InterNetwork)
                {
                    Emit("  Subnet................................... : {0}", ipInfo.IPv4Mask);
                }
            }
        }

        private void Emit()
        {
            Emit(string.Empty);
        }

        private void Emit(string format, params object[] args)
        {
            string message = string.Format(format, args);
            _accumulatedDump.AppendLine(message);
        }
    }
}
