using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kraken.Core.Tests.TestClasses;
using Kraken.Core;
using NUnit.Framework;

namespace Kraken.Core.Tests
{
    public class ObjectDumpFixture : Fixture
    {
        [Test]
        public void ToStringHelper()
        {
            DataEntity single = DataEntity.GetSeveral().First();

            string output = DataEntity.GetObjectDump(single).ToString();

            // CodeGen.GenerateAssertions(output, "output"); // The following assertions were generated on 21-Jun-2011
            #region CodeGen Assertions
            Assert.AreEqual("Id=1, IsCool=True, Description=Holy bat man boat monster, Created=2000-01-01, Amount=14.23", output);
            #endregion
        }

        [Test]
        public void Merge()
        {
            List<DataEntity> entities = DataEntity.GetSeveral();
            ObjectDump dump1 = DataEntity.GetObjectDump(entities[0]);
            ObjectDump dump2 = DataEntity.GetObjectDump(entities[1]);

            ObjectDump merged = ObjectDump.Merge(dump1, dump2);

            // CodeGen.GenerateAssertions(merged, "merged"); // The following assertions were generated on 22-Jun-2011
            #region CodeGen Assertions
            Assert.AreEqual(10, merged.Headers.Count);
            Assert.AreEqual("Id", merged.Headers[0]);
            Assert.AreEqual("IsCool", merged.Headers[1]);
            Assert.AreEqual("Description", merged.Headers[2]);
            Assert.AreEqual("Created", merged.Headers[3]);
            Assert.AreEqual("Amount", merged.Headers[4]);
            Assert.AreEqual("Id", merged.Headers[5]);
            Assert.AreEqual("IsCool", merged.Headers[6]);
            Assert.AreEqual("Description", merged.Headers[7]);
            Assert.AreEqual("Created", merged.Headers[8]);
            Assert.AreEqual("Amount", merged.Headers[9]);
            Assert.AreEqual(10, merged.Data.Count);
            Assert.AreEqual("1", merged.Data[0]);
            Assert.AreEqual("True", merged.Data[1]);
            Assert.AreEqual("Holy bat man boat monster", merged.Data[2]);
            Assert.AreEqual("2000-01-01", merged.Data[3]);
            Assert.AreEqual("14.23", merged.Data[4]);
            Assert.AreEqual("1", merged.Data[5]);
            Assert.AreEqual("False", merged.Data[6]);
            Assert.AreEqual("what boy wonder", merged.Data[7]);
            Assert.AreEqual("2000-03-01", merged.Data[8]);
            Assert.AreEqual("12314.23", merged.Data[9]);
            #endregion
        }
    }
}
