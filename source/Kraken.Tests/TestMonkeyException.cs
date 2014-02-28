using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kraken.Tests
{
    public class TestMonkeyException: Exception
    {
          #region Constructors
        /// <summary>
        /// Use the static constructors
        /// </summary>
        protected TestMonkeyException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Use the static constructors
        /// </summary>
        protected TestMonkeyException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
        #endregion

        #region Static Methods
        /// <summary>
        /// Create a new TestMonkeyException
        /// </summary>
        /// <remarks>
        /// Only use this exception if no suitable alternative exists - eg: use InvalidArgumentException if it is a better fit
        /// </remarks>
        public static TestMonkeyException Create(string format, params object[] args)
        {
            string message = string.Format(format, args);
            TestMonkeyException exception = new TestMonkeyException(message);
            return exception;
        }

        /// <summary>
        /// Create a new TestMonkeyException
        /// </summary>
        /// <remarks>
        /// Only use this exception if no suitable alternative exists - eg: use InvalidArgumentException if it is a better fit
        /// </remarks>
        public static TestMonkeyException Create(Exception innerException, string format, params object[] args)
        {
            string message = string.Format(format, args);
            TestMonkeyException exception = new TestMonkeyException(message, innerException);
            return exception;
        }
        #endregion
    }
}
