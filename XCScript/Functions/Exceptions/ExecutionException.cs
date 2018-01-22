using System;

namespace XCScript.Functions.Exceptions
{
    class ExecutionException : Exception
    {
        public ExecutionException()
        {
        }

        public ExecutionException(string message) : base(message)
        {
        }
    }
}
