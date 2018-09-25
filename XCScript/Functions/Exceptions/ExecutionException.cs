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

        /// <summary>
        /// Attached by <see cref="XCScript.Execution.Executable"/> on intercept
        /// </summary>
        public int Line { get; internal set; } = 0;

        /// <summary>
        /// Attached by <see cref="XCScript.Execution.Executable"/> on intercept
        /// </summary>
        public string Description { get; internal set; } = "";
    }
}
