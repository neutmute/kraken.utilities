using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnitTests.TestClasses
{
    public class Zoo
    {
        #region Fields
        private AnimalCollection _Animals;
        #endregion

        #region Properties
        public AnimalCollection Animals { get {return _Animals;} }
        #endregion

        #region Constructors
        public Zoo()
        {
            _Animals = new AnimalCollection();
            Animal cow = new Animal { LegCount = 4 };
            Animal chicken = new Animal { LegCount = 2 };
            Animal dogHitByCar = new Animal { LegCount = 3 };

            _Animals.Add(cow);
            _Animals.Add(chicken);
            _Animals.Add(dogHitByCar);
        }
        #endregion

        public override string ToString()
        {
            return "this is a ToString() override";
        }
    }
}
