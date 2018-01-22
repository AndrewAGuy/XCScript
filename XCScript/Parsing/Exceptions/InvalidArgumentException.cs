using System;

namespace XCScript.Parsing.Exceptions
{
    internal class InvalidArgumentException : Exception
    {
        public InvalidArgumentException(string message) : base(message)
        {
        }
    }
}
