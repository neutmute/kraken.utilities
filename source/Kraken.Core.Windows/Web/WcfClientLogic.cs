using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Channels;
using System.Text;

namespace Kraken.Web
{
    public class MessageMetadata
    {
        public string Operation { get; set; }

        public string Interface { get; set; }
    }

    public static class WcfChannelLogic
    {
        public static MessageMetadata GetMessageMetadata(Message request)
        {
            string action = request.Headers.Action;
            return GetMessageMetadata(action);
        }

        public static MessageMetadata GetMessageMetadata(string action)
        {
            var context = new MessageMetadata();

            string[] split = action.Split('/');
            context.Operation = split[split.Length - 1];
            context.Interface = split[split.Length - 2];

            //int lastIndexSlash = action.LastIndexOf("/");
            //int secondLastSlash = action.LastIndexOf("/", lastIndexSlash);

            //context.Operation = action.Substring(lastIndexSlash + 1);
            //context.Operation = action.Substring(lastIndexSlash + 1);
            return context;
        }
    }
}
