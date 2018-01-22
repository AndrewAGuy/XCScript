using System.Collections.Generic;
using XCScript.Arguments;
using XCScript.Functions.Exceptions;

namespace XCScript.Functions.Control
{
    internal class Throw : IFunction
    {
        public string Keyword
        {
            get
            {
                return "throw";
            }
        }

        public object Execute(IArgument[] arguments, Dictionary<string, object> globals)
        {
            if (arguments.Length > 0)
            {
                if (arguments.Length > 1)
                {
                    var res = globals[Engine.RKey] as Result;
                    res.Messages.Add($"'throw' called with {arguments.Length} arguments, only first will be used");
                }
                throw new ExecutionException(arguments[0].Evaluate(globals).ToString());
            }
            throw new ExecutionException();
        }
    }
}
