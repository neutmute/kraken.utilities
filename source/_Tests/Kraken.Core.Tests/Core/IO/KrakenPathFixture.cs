using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace Kraken.Core.Tests.Core.IO
{
    public class KrakenPathFixture : Fixture
    {
        [Test]
        public void CoerceValidFileName_SimpleValid()
        {
            var filename = @"thisIsValid.txt";
            var result = KrakenPath.CoerceValidFileName(filename);
            Assert.AreEqual(filename, result);
        }

        [Test]
        public void CoerceValidFileName_SimpleInvalid()
        {
            var filename = @"thisIsNotValid\3\\_3.txt";
            var result = KrakenPath.CoerceValidFileName(filename);
            Assert.AreEqual("thisIsNotValid_3__3.txt", result);
        }

        [Test]
        public void CoerceValidFileName_InvalidExtension()
        {
            var filename = @"thisIsNotValid.t\xt";
            var result = KrakenPath.CoerceValidFileName(filename);
            Assert.AreEqual("thisIsNotValid.t_xt", result);
        }

        [Test]
        public void CoerceValidFileName_KeywordInvalid()
        {
            var filename = "aUx.txt";
            var result = KrakenPath.CoerceValidFileName(filename);
            Assert.AreEqual("_reserverdWord_.txt", result);
        }

        [Test]
        public void CoerceValidFileName_KeywordValid()
        {
            var filename = "auxillary.txt";
            var result = KrakenPath.CoerceValidFileName(filename);
            Assert.AreEqual("auxillary.txt", result);
        }

    }
}
