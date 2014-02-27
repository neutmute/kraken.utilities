using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Web;

namespace Kraken.Web
{
    public static class WcfLogic
    {
        public static IPAddress GetRemoteIPAddress()
        {
            OperationContext context = OperationContext.Current;
            MessageProperties prop = context.IncomingMessageProperties;
            RemoteEndpointMessageProperty endpoint = prop[RemoteEndpointMessageProperty.Name] as RemoteEndpointMessageProperty;
            var ipAddress = IPAddress.Parse(endpoint.Address);
            return ipAddress;
        }

        
    }
}
