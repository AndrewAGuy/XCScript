using System;

namespace XCScript.Functions.Exceptions
{
    class ArgumentTypeException : ArgumentException
    {
        public ArgumentTypeException(string message) : base(message)
        {
        }
    }
}
