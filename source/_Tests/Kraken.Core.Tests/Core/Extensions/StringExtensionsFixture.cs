using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kraken.Core.Extensions;
using Kraken.Core.Tests;
using NUnit.Framework;

namespace Kraken.Core.Tests.Business.ExtensionMethods
{

    public class StringExtensionsFixture : Fixture
    {
        [Test]
        public void EqualsCaseInsensitive()
        {
            Assert.That("123".EqualsCaseInsensitive("123"));
            Assert.That("".EqualsCaseInsensitive(""));
            Assert.That(((string)null).EqualsCaseInsensitive(null));
            Assert.That("Foo".EqualsCaseInsensitive("foo"));

            Assert.That(!("Foo1".EqualsCaseInsensitive("foo")));
            Assert.That(!((string)null).EqualsCaseInsensitive("klkl"));
        }

        [Test]
        public void TrimToMax()
        {
            var input = "123456789";
            Assert.AreEqual("12...", input.TrimToMax(5));
            Assert.AreEqual("12345", input.TrimToMax(5, StringExtensions.TrimToMaxOptions.NoElipsis));
        }
    }
}
