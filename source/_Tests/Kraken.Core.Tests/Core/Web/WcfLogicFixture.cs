using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Channels;
using System.Text;
using Kraken.Web;
using NUnit.Framework;

namespace Kraken.Core.Tests.Core.Web
{
    public class WcfLogicFixture : Fixture
    {
        [Test]
        public void GetOperationContext_SplitsOk()
        {
            var action = "http://github.com/neutmute/cerberus/IUserService/Get";
            var context = WcfChannelLogic.GetMessageMetadata(action);

            // AssertBuilder.Generate(context, "context"); // The following assertions were generated on 15-May-2012
            #region CodeGen Assertions
            Assert.AreEqual("Get", context.Operation);
            Assert.AreEqual("IUserService", context.Interface);
            #endregion
        }
    }
}
