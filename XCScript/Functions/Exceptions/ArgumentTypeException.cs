using System;

namespace XCScript.Functions.Exceptions
{
    /// <summary>
    /// 
    /// </summary>
    public class ArgumentTypeException : ArgumentException
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public ArgumentTypeException(string message) : base(message)
        {
        }
    }
}
