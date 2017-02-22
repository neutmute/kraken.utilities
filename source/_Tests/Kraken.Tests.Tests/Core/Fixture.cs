using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Logging;
using NUnit.Framework;

namespace Kraken.Tests.Tests
{
    [TestFixture]
    public abstract class Fixture
    {
        protected static readonly ILog Log = LogManager.GetLogger<Fixture>();
        /// <summary>
        /// Default instance on the fixture for potentially easier syntax
        /// </summary>
        /// <remarks>Instantiate your own generic if you want it precast</remarks>
        public ObjectWalker<Object> ObjectWalkerDefault { get; private set; }

        /// <summary>
        /// Gets the <see cref="ObjectComparer"/> that was initialised during the <see cref="Setup"/> method.
        /// </summary>
        public ObjectComparer ObjectComparer { get; private set; }

        
        static Fixture()
        {
            TestFrameworkFacade.AssertEqual = (o1, o2, s) => { Assert.AreEqual(o1, o2, s); };
            TestFrameworkFacade.AssertFail = Assert.Fail;
        }

        [SetUp]
        public virtual void Setup()
        {
            ObjectWalkerDefault = new ObjectWalker<Object>();
            ObjectComparer = new ObjectComparer();
        }
    }
}
