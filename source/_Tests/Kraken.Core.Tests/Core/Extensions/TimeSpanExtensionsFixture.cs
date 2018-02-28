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
        public void ToHumanReadableWithCustomLabel()
        {
            var ts = TimeSpan.Parse("6:12:14:45.3448");
            var options = new HumanReadableTimeSpanOptions();
            options.LabelMode = HumanReadableLabelMode.Custom;
            options.CustomLabels = new HumanReadableTimeLabels();
            options.CustomLabels.Day = "D";
            options.CustomLabels.Hour = "H";
            options.CustomLabels.Minute = "M";
            options.CustomLabels.Second = "S";
            options.CustomLabels.Millisecond = "MILS";

            Assert.AreEqual("6 Ds, 12 Hs, 14 Ms, 45 Ss", ts.ToHumanReadable(options));
        }

        [Test]
        public void ToHumanReadableWhenMax()
        {
            TimeSpan ts = TimeSpan.MaxValue;
            Assert.AreEqual("TimeSpan.Max", ts.ToHumanReadable());
        }


        [Test]
        public void ToHumanReadableWhenMin()
        {
            TimeSpan ts = TimeSpan.MinValue;
            Assert.AreEqual("TimeSpan.Min", ts.ToHumanReadable());
        }

        [Test]
        public void ToHumanReadableWhenNull()
        {
            TimeSpan? ts = null;
            Assert.AreEqual("<null>", ts.ToHumanReadable());
        }

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

            span = TimeSpan.FromTicks(120003); // milliseconds with 4 decimal places
            Assert.AreEqual("12 milliseconds", span.ToHumanReadable());

            span = TimeSpan.FromMilliseconds(1609);
            Assert.AreEqual("1.61 seconds", span.ToHumanReadable());

            span = TimeSpan.FromTicks(500);
            Assert.AreEqual("0.05 milliseconds", span.ToHumanReadable());
        }

        [Test]
        public void ToHumanReadableAbbreviated()
        {
            TimeSpan span = new TimeSpan(1, 0, 0, 0);
            Assert.AreEqual("1 day", span.ToHumanReadable(HumanReadableLabelMode.Abbreviated));

            span = new TimeSpan(2, 0, 0, 0);
            Assert.AreEqual("2 days", span.ToHumanReadable(HumanReadableLabelMode.Abbreviated));

            span = new TimeSpan(1, 5, 0, 0);
            Assert.AreEqual("1 day, 5 hrs", span.ToHumanReadable(HumanReadableLabelMode.Abbreviated));

            span = new TimeSpan(2, 0, 30, 0);
            Assert.AreEqual("2 days, 30 mins", span.ToHumanReadable(HumanReadableLabelMode.Abbreviated));

            span = new TimeSpan(2, 1, 1, 1);
            Assert.AreEqual("2 days, 1 hr, 1 min, 1 sec", span.ToHumanReadable(HumanReadableLabelMode.Abbreviated));

            span = TimeSpan.FromMilliseconds(999);
            Assert.AreEqual("999 ms", span.ToHumanReadable(HumanReadableLabelMode.Abbreviated));
        }
    }
}
