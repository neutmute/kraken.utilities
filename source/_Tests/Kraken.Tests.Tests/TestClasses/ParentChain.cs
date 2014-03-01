using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnitTests.TestClasses
{
    public class ParentChain
    {
        #region Properties
        public ParentChain Child { get; set; }

        public string Name { get; set; }
        #endregion

        #region Constructors
        public ParentChain(string name)
        {
            Name = name;
        }
        #endregion

        #region Static Methods
        public static ParentChain GetGrandFatherSample()
        {
            ParentChain greatGrandfather = new ParentChain("greatGrandfather");
            ParentChain grandfather = new ParentChain("grandfather");
            ParentChain father = new ParentChain("father");
            ParentChain son = new ParentChain("son");
            
            greatGrandfather.Child = grandfather;
            grandfather.Child = father;
            father.Child = son;

            return greatGrandfather;
        }
        #endregion
    }
}
