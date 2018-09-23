using XCScript.Arguments;

namespace XCScript.Functions.Exceptions
{
    /// <summary>
    /// 
    /// </summary>
    public class ArgumentTypeException : ExecutionException
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="argument"></param>
        public ArgumentTypeException(string message, IArgument argument = null) : base(message)
        {
            this.Argument = argument;
        }

        /// <summary>
        /// 
        /// </summary>
        public IArgument Argument { get; private set; }
    }
}
