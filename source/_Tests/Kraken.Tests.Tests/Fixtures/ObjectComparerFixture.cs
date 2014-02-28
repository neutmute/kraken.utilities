using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kraken.Tests;
using Kraken.Tests.Tests;
using Kraken.Tests.Tests.TestClasses;
using NUnit.Framework;
using UnitTests.TestClasses;

namespace UnitTests
{
    [TestFixture]
    public class ObjectComparerFixture : Fixture
    {
        public override void Setup()
        {
            base.Setup();
            ObjectComparer.Options.LogToConsole = true;
        }


        [Test]
        public void AreEqualArray()
        {
            NetworkAddress address1 = new NetworkAddress(new byte[] { 1, 2 });
            NetworkAddress address2 = new NetworkAddress(new byte[] { 1, 2 });

            ObjectComparer.AssertEqual(address1, address2);
        }

        [Test]
        public void AreEqual()
        {
            ParentChain chain1 = ParentChain.GetGrandFatherSample();
            ParentChain chain2 = ParentChain.GetGrandFatherSample();
        
            ObjectComparer.AssertEqual(chain1, chain2);
        }

        [Test]
        public void AreEqualFailure()
        {
            ParentChain chain1 = ParentChain.GetGrandFatherSample();
            ParentChain chain2 = ParentChain.GetGrandFatherSample();

            ObjectComparer.AssertEqual(chain1, chain2);
        }

        [ExpectedException(typeof(AssertionException))]
        [Test]
        public void AreNotEqual()
        {
            ParentChain chain1 = ParentChain.GetGrandFatherSample();
            ParentChain chain2 = ParentChain.GetGrandFatherSample();
            chain2.Name = "bfff";

            ObjectComparer.AssertNotEqual(chain1, chain2);
        }
    }
}
