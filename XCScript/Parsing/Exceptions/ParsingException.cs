using System;

namespace XCScript.Parsing.Exceptions
{
    class ParsingException : Exception
    {
        public ParsingException()
        {
        }

        public ParsingException(string message) : base(message)
        {
        }
    }
}
