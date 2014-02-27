using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kraken.Core
{
    public static class Guard
    {
        public static void NullArgument(string name, object argument)
        {
            if (argument == null)
            {
                throw new ArgumentNullException(name);
            }
        }
    }
}
