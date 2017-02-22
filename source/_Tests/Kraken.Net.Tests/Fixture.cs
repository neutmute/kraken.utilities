using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Kraken.Tests.NUnit;
using Kraken.Tests;
using NUnit.Framework;

namespace Kraken.Core.Tests
{
    [TestFixture]
    public abstract class Fixture : KrakenFixture
    {
        protected ResourceExtractor Resource
        {
            get { return new ResourceExtractor(); }
        }

        static Fixture()
        {
            TestFrameworkFacade.AssertEqual = (o1, o2, s) => { Assert.AreEqual(o1, o2, s); };
            TestFrameworkFacade.AssertFail = Assert.Fail;
            SetTestTempDirectory(@"D:\Logs\Tests\Framework.Core");
        }


        protected override void RegisterAutofacModules()
        {
        }
    }
}
