using System;

namespace Kraken.Core
{
    /// <summary>
    /// Doesn't do anything special apart from simplified syntax in order to use string.format
    /// (see static methods)
    /// </summary>
    /// <remarks>
    /// The pure static throw was not being handled by exception management properly
    /// change to the thread static singleton fixed this
    /// </remarks>
    [Serializable]
    public class KrakenException : Exception
    {
        #region Constructors
        /// <summary>
        /// Use the static constructors
        /// </summary>
        protected KrakenException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Use the static constructors
        /// </summary>
        protected KrakenException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
        #endregion

        #region Static Methods
        /// <summary>
        /// Create a new KrakenException
        /// </summary>
        /// <remarks>
        /// Only use this exception if no suitable alternative exists - eg: use InvalidArgumentException if it is a better fit
        /// </remarks>
        public static KrakenException Create(string format, params object[] args)
        {
            string message = string.Format(format, args);
            KrakenException exception = new KrakenException(message);
            return exception;
        }

        /// <summary>
        /// Create a new KrakenException
        /// </summary>
        /// <remarks>
        /// Only use this exception if no suitable alternative exists - eg: use InvalidArgumentException if it is a better fit
        /// </remarks>
        public static KrakenException Create(Exception innerException, string format, params object[] args)
        {
            string message = string.Format(format, args);
            KrakenException exception = new KrakenException(message, innerException);
            return exception;
        }
        #endregion
    }
}