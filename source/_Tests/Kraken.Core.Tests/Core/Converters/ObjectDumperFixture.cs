using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Kraken.Core.Tests.TestClasses;
using Kraken.Core;
using Kraken.Core.Tests.Core.Extensions;
using NUnit.Framework;

namespace Kraken.Core.Tests
{
    [TestFixture]
    public class ObjectDumperFixture : Fixture
    {
        [Test]
        public void DumpRight()
        {
            List<DataEntity> items = DataEntity.GetSeveral();
            ObjectDumper<DataEntity> entityDumper = new ObjectDumper<DataEntity>(DataEntity.GetObjectDump);

            entityDumper.KrakenHorizontalAlignDefault = KrakenHorizontalAlign.Right;
            string dump = entityDumper.Dump(items);

            Assert.AreEqual(entityDumper.KrakenHorizontalAlignDefault, KrakenHorizontalAlign.Right);

            // CodeGen.GenerateAssertions(dump, "dump"); // The following assertions were generated on 24-Jan-2011
             #region Generated Assertions
             Assert.AreEqual(@"  Id  IsCool                Description     Created        Amount
  --  ------  -------------------------  ----------  ------------
   1    True  Holy bat man boat monster  2000-01-01         14.23
   1   False            what boy wonder  2000-03-01      12314.23
   1    True                     (null)  2010-01-01  14.213333333
".NormaliseCrlf(), dump.NormaliseCrlf());
            #endregion

        }

        [Test]
        public void DumpLeft()
        {
            List<DataEntity> items = DataEntity.GetSeveral();
            ObjectDumper<DataEntity> entityDumper = new ObjectDumper<DataEntity>(DataEntity.GetObjectDump);
            string dump = entityDumper.Dump(items);

            // CodeGen.GenerateAssertions(dump, "dump"); // The following assertions were generated on 24-Jan-2011
            #region Generated Assertions
            Assert.AreEqual(@"  Id  IsCool  Description                Created     Amount      
  --  ------  -------------------------  ----------  ------------
  1   True    Holy bat man boat monster  2000-01-01  14.23       
  1   False   what boy wonder            2000-03-01  12314.23    
  1   True    (null)                     2010-01-01  14.213333333
".NormaliseCrlf(), dump.NormaliseCrlf());
            #endregion

        }
    }
}
