namespace XCScript.Functions.Exceptions
{
    /// <summary>
    /// 
    /// </summary>
    public class ArgumentCountException : ExecutionException
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public ArgumentCountException(string message) : base(message)
        {
        }
    }
}
