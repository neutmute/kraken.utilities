using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kraken.Tests.Tests.TestClasses
{
    public class MonthArray
    {
        public List<int> Numbers { get; set; }

        public MonthArray()
        {
            Numbers = new List<int> { 1, 2, 3, 4 };
        }
    }
}
