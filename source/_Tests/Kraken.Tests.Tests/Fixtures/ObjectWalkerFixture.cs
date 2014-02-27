using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kraken.Framework.TestMonkey;
using Kraken.Framework.TestMonkey.Tests;
using Kraken.Framework.TestMonkey.Tests.TestClasses;
using NUnit.Framework;
using UnitTests.TestClasses;

namespace UnitTests
{
    [TestFixture]
    public class ObjectWalkerFixture : Fixture
    {
        [Ignore("Shows FAilure")]
        [Test]
        public void IntArray()
        {
            MonthArray months = new MonthArray();
            StringBuilder walkLog = new StringBuilder();

            ObjectWalkerDefault.GetValue(months, "Numbers", o => walkLog.Append(o.ToString() + ","));

            Assert.AreEqual("son,father,grandfather,greatGrandfather,", walkLog.ToString());
        }

        [Ignore("Shows FAilure")]
        [Test]
        public void ByteArray()
        {
            NetworkAddress address1 = new NetworkAddress (new byte[] {1, 2});
            StringBuilder walkLog = new StringBuilder();

            ObjectWalkerDefault.GetValue(address1, "Bytes", o => walkLog.Append(o.ToString() + ","));

            Assert.AreEqual("son,father,grandfather,greatGrandfather,", walkLog.ToString());
        }

        [Test]
        public void DefaultGenericOnFixture()
        {
            ParentChain chain = ParentChain.GetGrandFatherSample();
            StringBuilder walkLog = new StringBuilder();
            
            ObjectWalkerDefault.GetValue(chain, "Name", o => walkLog.Append(o.ToString() + ","));

            Assert.AreEqual("son,father,grandfather,greatGrandfather,", walkLog.ToString());
        }

        [Test]
        public void OwnObjectWalkerInstance()
        {
            ParentChain chain = ParentChain.GetGrandFatherSample();
            StringBuilder walkLog = new StringBuilder();

            ObjectWalker<string> walker = new ObjectWalker<string>();
            walker.Options.LogToConsole = true;

            walker.GetValue(chain, "Name", s => walkLog.Append(s + ","));

            Assert.AreEqual("son,father,grandfather,greatGrandfather,", walkLog.ToString());
        }
    }
}
