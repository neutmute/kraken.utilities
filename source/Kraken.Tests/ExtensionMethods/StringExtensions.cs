using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kraken.Tests.Extensions
{
    public static class StringExtensions
    {
        public static string NormaliseCrlf(this string target)
        {
            return target.Replace("\r\n", "\n");
        }
    }
}
