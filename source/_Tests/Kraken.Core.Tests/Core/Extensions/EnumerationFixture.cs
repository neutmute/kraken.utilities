using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Kraken.Core.Tests.TestClasses;
using Kraken.Core;
using Kraken.Core.Tests;
using NUnit.Framework;

namespace Kraken.Core.Tests.Extensions
{
    [TestFixture]
    public class EnumerationFixture : Fixture
    {
        [Test]
        public void FromValue()
        {
            Colour colour = Enumeration.FromValue<Colour>("Pink");
            Assert.AreEqual(colour, Colour.Pink);
        }

        [ExpectedException(typeof(ArgumentException))]
        [Test]
        public void FromValueBad()
        {
            Colour colour = Enumeration.FromValue<Colour>("Pi4nk");
        }

        //[Test]
        //public void FromCode()
        //{
        //    Colour colour = Enumeration.FromCode<Colour>("P");
        //    Assert.AreEqual(colour, Colour.Pink);
        //}

        //[Test]
        //public void FromCodeInvalid()
        //{
        //    Colour colour = Enumeration.FromCode<Colour>("foobar");
        //    Assert.AreEqual(colour, Colour.Unknown);
        //}

        [Test]
        public void GetCode()
        {
            string code = Colour.Orange.GetCode();
            Assert.AreEqual("O", code);
        }

        [Test]
        public void GetDisplayName_NoAttributeSeparatesCamelCase()
        {
            var displayName = SortLowerEnum.ValueFirstLogicalSecond.GetDisplayName();
            Assert.AreEqual("Value First Logical Second", displayName);
        }

        [Test]
        public void GetDisplayName_NoAttributeSimple()
        {
            var displayName = Colour.Orange.GetDisplayName();
            Assert.AreEqual("Orange", displayName);
        }

        

        [Test]
        public void GetDescription()
        {
            string code = Colour.Orange.GetDescription();
            Assert.AreEqual("Traffic lights are this colour", code);
        }

        [Test]
        public void GetCodeWhenNotSpecified()
        {
            string code = Colour.Green.GetCode();
            Assert.AreEqual("4", code);
        }

        [Test]
        public void GetDescriptionWhenNotSpecified()
        {
            string code = Colour.Green.GetDescription();
            Assert.AreEqual("Green", code);
        }

        [Test]
        public void GetAll()
        {
            List<Colour> colours = Enumeration.GetAll<Colour>(true);

            #region Generated Assertions
            Assert.AreEqual(8, colours.Count);
            Assert.AreEqual(Colour.Unknown, colours[0]);
            Assert.AreEqual(Colour.Red, colours[1]);
            Assert.AreEqual(Colour.Orange, colours[2]);
            Assert.AreEqual(Colour.Pink, colours[3]);
            Assert.AreEqual(Colour.Green, colours[4]);
            Assert.AreEqual(Colour.Blue, colours[5]);
            Assert.AreEqual(Colour.Yellow, colours[6]);
            Assert.AreEqual(Colour.Purple, colours[7]);
            #endregion
        }

        [Test]
        public void GetAllWithExclusion()
        {
            List<Colour> colours = Enumeration.GetAll<Colour>(false);

            #region Generated Assertions
            Assert.AreEqual(7, colours.Count);
            Assert.AreEqual(Colour.Red, colours[0]);
            Assert.AreEqual(Colour.Orange, colours[1]);
            Assert.AreEqual(Colour.Pink, colours[2]);
            Assert.AreEqual(Colour.Green, colours[3]);
            Assert.AreEqual(Colour.Blue, colours[4]);
            Assert.AreEqual(Colour.Yellow, colours[5]);
            Assert.AreEqual(Colour.Purple, colours[6]);
            #endregion
        }

        [Test]
        public void ShouldSortLowerIfRequestedBySortAttribute()
        {
            List<SortHigherEnum> items = Enumeration.GetAll<SortHigherEnum>(true);

            Assert.IsTrue(items[0] == SortHigherEnum.ValueSecondLogicalFirst, "Item with Low Logical Order hasn't been sorted first");
            Assert.IsTrue(items[1] == SortHigherEnum.ValueFirstLogicalSecond);
        }

        [Test]
        public void ShouldSortHigherIfRequestedBySortAttribute()
        {
            List<SortLowerEnum> items = Enumeration.GetAll<SortLowerEnum>(true);

            Assert.IsTrue(items[1] == SortLowerEnum.ValueFirstLogicalSecond, "Item with a high logical order hasn't been sorted last");
            Assert.IsTrue(items[0] == SortLowerEnum.ValueSecondLogicalFirst);
        }

        [Test]
        public void ShouldSortEnumsByTheirValueOrderIfNotAttributed()
        {
            List<NoSortEnum> items = Enumeration.GetAll<NoSortEnum>(true);

            Assert.IsTrue(items[0] == NoSortEnum.First);
            Assert.IsTrue(items[1] == NoSortEnum.Second);
            Assert.IsTrue(items[2] == NoSortEnum.Third);
        }

        [Test]
        public void ShouldSortEnumsByTheirValueOrderAndIgnoreDefinedOrder()
        {
            List<ValueUnorderedEnum> items = Enumeration.GetAll<ValueUnorderedEnum>(true);
            
            Assert.IsTrue(items[0] == ValueUnorderedEnum.First);
            Assert.IsTrue(items[1] == ValueUnorderedEnum.Second);
            Assert.IsTrue(items[2] == ValueUnorderedEnum.Third);
        }
    }
}
