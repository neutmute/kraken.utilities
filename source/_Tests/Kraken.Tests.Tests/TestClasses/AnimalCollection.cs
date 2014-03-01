using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnitTests.TestClasses
{
    public class AnimalCollection : List<IAnimal>
    {
        /// <summary>
        /// Tests the code gen emits against this property
        /// </summary>
        public int TotalLegs
        {
            get
            {
                return this.Sum(a => a.LegCount);
            }
        }
    }
}
