using System.Collections.Generic;
using XCScript.Arguments;
using XCScript.Functions.Exceptions;

namespace XCScript.Functions.Control
{
    internal class Do : IFunction
    {
        public string Keyword
        {
            get
            {
                return "do";
            }
        }

        public object Execute(IArgument[] arguments, Dictionary<string, object> globals)
        {
            if (arguments.Length < 2)
            {
                throw new ArgumentCountException("'do' requires 2 arguments");
            }
            else if (arguments.Length > 2)
            {
                var res = globals[Engine.RKey] as Result;
                res.Messages.Add($"'do' called with {arguments.Length} arguments, only first 2 will be used");
            }

            var end = (int)arguments[0].Evaluate(globals);
            object obj = null;
            for (var i = 0; i < end; ++i)
            {
                obj = arguments[1].Evaluate(globals);
            }
            return obj;
        }
    }
}
