using System;

namespace XCScript.Parsing.Exceptions
{
    internal class InvalidCharacterException : Exception
    {
        public InvalidCharacterException(string message) : base(message)
        {
        }
    }
}
