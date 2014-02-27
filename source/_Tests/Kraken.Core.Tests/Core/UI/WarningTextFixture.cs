using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace Kraken.Core.Tests.Core.UI
{
    [TestFixture]
    public class WarningTextFixture : Fixture
    {
        [Test]
        public void Equals()
        {
            WarningText warning1 = new WarningText(WarningLevel.Critical, "broken!");
            WarningText warning2 = new WarningText(WarningLevel.Information, "almost!");
            WarningText warning3 = new WarningText(WarningLevel.Information, "almost!");

            Assert.IsTrue(warning2.Equals(warning3));
            Assert.IsFalse(warning1.Equals(warning3));
        }

        [Test]
        public void TheToString()
        {
            WarningText text = new WarningText(WarningLevel.Critical, "lorum ipsum");
            string toString = text.ToString();

            // CodeGen.GenerateAssertions(toString, "toString"); // The following assertions were generated on 28-Sep-2010
            #region CodeGen Assertions
            Assert.AreEqual("[Critical] lorum ipsum", toString);
            #endregion
        }
    }
}
