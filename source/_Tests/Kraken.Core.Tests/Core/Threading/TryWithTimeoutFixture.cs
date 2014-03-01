using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Kraken.Core.Threading;
using NUnit.Framework;

namespace Kraken.Core.Tests.Core.Threading
{
    public class TryWithTimeoutFixture : Fixture
    {
        [ExpectedException(ExpectedException= typeof(TimeoutException))]
        [Test]
        public void TryInvoke_ThrowsWithTimeout()
        {
            Action action = () => Thread.Sleep(500);
            var timeout = TimeSpan.FromMilliseconds(100);
            TryWithTimeout.TryInvoke(action, timeout);
        }

        [Test]
        public void TryInvoke_ExceptionContainableOnTimeout()
        {
            Action action = () => Thread.Sleep(500);
            var timeout = TimeSpan.FromMilliseconds(100);
            Exception exceptionCaught = null;
            try
            {
                TryWithTimeout.TryInvoke(action, timeout);
            }
            catch (Exception e)
            {
                exceptionCaught = e;
            }
            Assert.NotNull(exceptionCaught);
            Assert.That(exceptionCaught.GetType() == typeof(TimeoutException));
        }
        
        [Test]
        public void TryInvoke_ExceptionBeforeTimeout()
        {
            Action action = () => { throw new NotImplementedException(); };

            var timeout = TimeSpan.FromMilliseconds(1000);
            Exception exceptionCaught = null;
            try
            {
                TryWithTimeout.TryInvoke(action, timeout);
            }
            catch (Exception e)
            {
                exceptionCaught = e;
            }
            Assert.NotNull(exceptionCaught);
            Assert.That(exceptionCaught.GetType() == typeof(NotImplementedException));
        }
    }
}
