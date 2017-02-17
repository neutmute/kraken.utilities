using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kraken.Tests;
using Xunit;

namespace Sample.Tests.Xunit
{
    public class TestBase
    {
        public ObjectComparer ObjectComparer { get; private set; }

        public AssertBuilder AssertBuilder {get; private set;}

        public TestBase()
        {

            AssertBuilder = new AssertBuilder();
            ObjectComparer = new ObjectComparer();

            AssertBuilder.Options.AssertSignatures.AreEqual = "Equal";
            AssertBuilder.Options.AssertSignatures.IsNull = "Null";
            AssertBuilder.AppendEmittedCodeToFailMessage = true;

            TestFrameworkFacade.AssertEqual = (o1, o2, m) =>
            {
                if (o1 is string && o2 is string)
                {
                    // get original messages through
                    var o1AsString = o1 as string;
                    var o2AsString = o2 as string;
                    if (!o1AsString.Equals(o2AsString))
                    {
                        Assert.False(true, m);
                    }
                }
                else
                {
                    Assert.Equal(o1, o2);
                }
            };
            TestFrameworkFacade.AssertNotEqual = (o1, o2, m) => { Assert.NotEqual(o1, o2); };
            TestFrameworkFacade.AssertFail = (mf, args) => {
               // Log.InfoFormat(mf, args);
                if (args.Length == 0)
                {
                    Assert.False(true, mf);
                }
                else
                {
                    Assert.False(true, string.Format(mf, args));
                }
            };
        }
    }
}
