using System;

namespace XCScript.Functions.Exceptions
{
    /// <summary>
    /// Base type for execution exceptions, which are caught by <see cref="Engine"/>
    /// </summary>
    public class ExecutionException : Exception
    {
        /// <summary>
        /// Default ctor
        /// </summary>
        public ExecutionException()
        {
        }

        /// <summary>
        /// Message ctor
        /// </summary>
        /// <param name="message"></param>
        public ExecutionException(string message) : base(message)
        {
        }
    }
}
