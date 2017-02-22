using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kraken.Core
{
    public static class Guard
    {
        public static void NullOrEmpty(string value, string message)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new KrakenException(message);
            }
        }

        public static void EnumIsZero(Enum value)
        {
            if (Convert.ToInt32(value) == 0)
            {
                throw new KrakenException("Cannot be zero");
            }
        }

        public static void Against(bool conditionThatShouldBeFalse, string message)
        {
            if (conditionThatShouldBeFalse)
            {
                throw new KrakenException(message);
            }
        }

        public static void That(bool conditionThatShouldBeTrue, string message)
        {
            if (!conditionThatShouldBeTrue)
            {
                throw new KrakenException(message);
            }
        }


        public static void Null(object value, string message)
        {
            if (value == null)
            {
                throw new KrakenException(message);
            }
        }
    }
}
