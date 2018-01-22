namespace XCScript.Functions.Exceptions
{
    class ArgumentCountException : ExecutionException
    {
        public ArgumentCountException(string message) : base(message)
        {
        }
    }
}
