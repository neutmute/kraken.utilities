using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kraken.Core
{
    public static class Guard
    {
        public static void NullOrEmpty(string value, string messageFormat = null, params object[] messageArgs)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw KrakenException.Create(GetMessage("String cannot be null or empty", messageFormat, messageArgs));
            }
        }

        public static void EnumIsZero(Enum value)
        {
            if (Convert.ToInt32(value) == 0)
            {
                throw KrakenException.Create("Cannot be zero");
            }
        }

        public static void Against(bool conditionThatShouldBeFalse, string messageFormat = null, params object[] messageArgs)
        {
            if (conditionThatShouldBeFalse)
            {
                throw KrakenException.Create(messageFormat, messageArgs);
            }
        }

        public static void That(bool conditionThatShouldBeTrue, string messageFormat = null, params object[] messageArgs)
        {
            if (!conditionThatShouldBeTrue)
            {
                throw KrakenException.Create(messageFormat, messageArgs);
            }
        }


        public static void Null(object value, string messageFormat = null, params object[] messageArgs)
        {
            if (value == null)
            {
                throw KrakenException.Create(GetMessage("object cannot be null", messageFormat, messageArgs));
            }
        }

        private static string GetMessage(string defaultMessage, string messageFormat, params object[] messageArgs)
        {
            if (messageFormat != null)
            {
                defaultMessage = string.Format(messageFormat, messageArgs);
            }
            return defaultMessage;
        }
    }
}
