using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kraken.Core.Tests.TestClasses;
using NUnit.Framework;

namespace Kraken.Core.Tests.Core
{
    public class GuardFixture : Fixture
    {
        [ExpectedException(typeof(KrakenException))]
        [Test]
        public void EnumZero()
        {
            Guard.EnumIsZero(Colour.Unknown);
        }

        [ExpectedException(typeof(KrakenException))]
        [Test]
        public void That()
        {
            var value = -1;
            Guard.That(value > 0, "WTF?");
        }

        [ExpectedException(typeof(KrakenException))]
        [Test]
        public void Against()
        {
            var uninitialised = int.MinValue;
            Guard.Against(uninitialised == int.MinValue, "WTF?");
        }

        [Test]
        public void EnumOk()
        {
            Guard.EnumIsZero(Colour.Red);
            // shouldn't throw
        }


        [Test]
        [ExpectedException(typeof(KrakenException), ExpectedMessage = "taskInfo.InputParam is required")]
        public void NullOrEmpty()
        {
            Guard.NullOrEmpty("", "taskInfo.InputParam is required");
        }

    }
}
