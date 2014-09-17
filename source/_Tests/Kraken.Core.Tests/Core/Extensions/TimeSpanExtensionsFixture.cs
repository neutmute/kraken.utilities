using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kraken.Core;
using Kraken.Core.Tests;
using NUnit.Framework;

namespace Kraken.Core.Tests.Business.ExtensionMethods
{
    [TestFixture]
    public class TimeSpanExtensionsFixture : Fixture
    {
        [Test]
        public void ToHumanReadableVerbose()
        {
            TimeSpan span = new TimeSpan(1, 0, 0, 0);
            Assert.AreEqual("1 day", span.ToHumanReadable());
                      
            span = new TimeSpan(2, 0, 0, 0);
            Assert.AreEqual("2 days", span.ToHumanReadable());

            span = new TimeSpan(1, 5, 0, 0);
            Assert.AreEqual("1 day, 5 hours", span.ToHumanReadable());

            span = new TimeSpan(2, 0, 30, 0);
            Assert.AreEqual("2 days, 30 minutes", span.ToHumanReadable());

            span = new TimeSpan(2, 1, 1, 1);
            Assert.AreEqual("2 days, 1 hour, 1 minute, 1 second", span.ToHumanReadable());

            span = TimeSpan.FromMilliseconds(999);
            Assert.AreEqual("999 milliseconds", span.ToHumanReadable());

            span = TimeSpan.FromMilliseconds(1609);
            Assert.AreEqual("1.61 seconds", span.ToHumanReadable());
        }

        [Test]
        public void ToHumanReadableAbbreviated()
        {
            TimeSpan span = new TimeSpan(1, 0, 0, 0);
            Assert.AreEqual("1 day", span.ToHumanReadable(HumanReadableTimeSpanOptions.Abbreviated));

            span = new TimeSpan(2, 0, 0, 0);
            Assert.AreEqual("2 days", span.ToHumanReadable(HumanReadableTimeSpanOptions.Abbreviated));

            span = new TimeSpan(1, 5, 0, 0);
            Assert.AreEqual("1 day, 5 hrs", span.ToHumanReadable(HumanReadableTimeSpanOptions.Abbreviated));

            span = new TimeSpan(2, 0, 30, 0);
            Assert.AreEqual("2 days, 30 mins", span.ToHumanReadable(HumanReadableTimeSpanOptions.Abbreviated));

            span = new TimeSpan(2, 1, 1, 1);
            Assert.AreEqual("2 days, 1 hr, 1 min, 1 sec", span.ToHumanReadable(HumanReadableTimeSpanOptions.Abbreviated));
            
            span = TimeSpan.FromMilliseconds(999);
            Assert.AreEqual("999 ms", span.ToHumanReadable());
        }
    }
}
