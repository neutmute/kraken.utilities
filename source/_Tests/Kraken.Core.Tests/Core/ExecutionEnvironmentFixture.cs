using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Kraken.Core;
using Kraken.Core.Tests;
using NUnit.Framework;
using Kraken.Core.Tests.TestClasses;

namespace Kraken.Core.Tests.Business
{
    [TestFixture]
    public class ExecutionEnvironmentFixture : Fixture
    {
        [Ignore("Fails from nunit console")]
        [Test]
        public void AssertIsDebug()
        {
            ExecutionEnvironment.AssertIsDebug("Expected debug");
        }
        
    }
}
