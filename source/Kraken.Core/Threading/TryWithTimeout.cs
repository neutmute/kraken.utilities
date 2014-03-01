using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using Common.Logging;

namespace Kraken.Core.Threading
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// http://stackoverflow.com/questions/299198/implement-c-sharp-generic-timeout
    /// </remarks>
    public static class TryWithTimeout
    {
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        public static T Invoke<T>(Func<CancelEventArgs, T> function, TimeSpan timeout)
        {
            if (timeout.TotalMilliseconds <= 0)
                throw new ArgumentOutOfRangeException("timeout");

            CancelEventArgs args = new CancelEventArgs(false);
            IAsyncResult functionResult = function.BeginInvoke(args, null, null);
            WaitHandle waitHandle = functionResult.AsyncWaitHandle;
            if (!waitHandle.WaitOne(timeout))
            {
                args.Cancel = true; // flag to worker that it should cancel!
                /* •————————————————————————————————————————————————————————————————————————•
                   | IMPORTANT: Always call EndInvoke to complete your asynchronous call.   |
                   | http://msdn.microsoft.com/en-us/library/2e08f6yc(VS.80).aspx           |
                   | (even though we arn't interested in the result)                        |
                   •————————————————————————————————————————————————————————————————————————• */
                ThreadPool.UnsafeRegisterWaitForSingleObject(waitHandle,
                    (state, timedOut) => function.EndInvoke(functionResult),
                    null, -1, true);

                throw new TimeoutException(string.Format("Timeout of {0} exceeded for attempted operation", timeout.ToHumanReadable()));
            }
            else
                return function.EndInvoke(functionResult);
        }

        public static T Invoke<T>(Func<T> function, TimeSpan timeout)
        {
            return Invoke(args => function(), timeout); // ignore CancelEventArgs
        }

        public static void Invoke(Action<CancelEventArgs> action, TimeSpan timeout)
        {
            Invoke<int>(args =>
            { // pass a function that returns 0 & ignore result
                action(args);
                return 0;
            }, timeout);
        }

        public static void TryInvoke(Action action, TimeSpan timeout)
        {
            Invoke(args => action(), timeout); // ignore CancelEventArgs
        }

    }
}
