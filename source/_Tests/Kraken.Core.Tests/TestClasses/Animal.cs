using System;

namespace Kraken.Core.Tests
{
    /// <summary>
    /// Animal is a class that can be used to test the code generator
    /// </summary>

    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:kingdom:animal:vertebrate")]
    public class Animal : IAnimal
    {
        #region Fields
        private int _Legs;
        private string _Name;
        #endregion

        #region Properties
        /// <summary>
        /// Public property
        /// </summary>
        public int Legs
        {
            get { return _Legs; }
            set { _Legs = value; }
        }

        public string Name
        {
            get { return _Name; }
            set { _Name = value; }
        }
        #endregion

        #region Constructors
        public Animal() : this("anonymal", 24)
        {
        }

        public Animal(string name, int legs)
        {
            Name = name;
            Legs = legs;
        }
        #endregion

        #region Static Methods
        public static Animal GetCat()
        {
            return new Animal {Legs = 4, Name = "Cat"};
        }
        #endregion
    }
}
