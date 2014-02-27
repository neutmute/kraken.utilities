﻿using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml.Serialization;
using Kraken.Core;
using Kraken.Core.Tests;
using Kraken.Framework.TestMonkey;
using NUnit.Framework;

namespace Kraken.Core.Tests
{
    [TestFixture]
    public class KelvinFixture : Fixture
    {
        [Test]
        public void FromXmlFile()
        {
            Animal cat = Kelvin<Animal>.FromXml(Resource.ExportToString("Kraken.Framework.Core.Tests.Resources.Kelvin.plainCat.txt"));
            // CodeGen.GenerateAssertions(cat, "cat"); // The following assertions were generated on 30-Jun-2011
            #region CodeGen Assertions
            Assert.AreEqual(4, cat.Legs);
            Assert.AreEqual("Cat", cat.Name);
            #endregion
        }

        [Ignore("different in opencover for some reason")]
        [Test]
        public void ToXmlFile()
        {
            string testFile = Path.Combine(TestTempDirectory, "test.xml");
            Animal cat = Animal.GetCat();
            Kelvin<Animal>.ToXmlFile(cat, testFile);

            AssertTool.ResourceEquals("Kraken.Framework.Core.Tests.Resources.Kelvin.plainCat.txt", new FileInfo(testFile));
        }

        [Test]
        public void ToXmlFileWithNamespace()
        {
            string testFile = Path.Combine(TestTempDirectory, "test.xml");
            Animal cat = Animal.GetCat();


            XmlSerializerNamespaces nameSpaces = new XmlSerializerNamespaces();
            nameSpaces.Add("ak", "urn:kingdom:animal:vertebrate");
            Kelvin<Animal>.ToXmlFile(cat, testFile, nameSpaces);

            AssertTool.ResourceEquals("Kraken.Framework.Core.Tests.Resources.Kelvin.namespacedCat.txt", new FileInfo(testFile));
        }
    }
}
