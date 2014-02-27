using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Kraken.Framework.Testing;
using Kraken.Framework.TestMonkey;
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
            TestFrameworkFacade.AssertEqual = Assert.AreEqual;
            TestFrameworkFacade.AssertFail = Assert.Fail;
            SetTestTempDirectory(@"D:\Logs\Tests\Framework.Core");
        }


        protected override void RegisterAutofacModules()
        {
        }
    }
}
