using System;

namespace XCScript.Parsing.Exceptions
{
    internal class FinalCharacterException : Exception
    {
        public FinalCharacterException(string message) : base(message)
        {
        }
    }
}
