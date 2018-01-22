using System.Collections.Generic;
using XCScript.Arguments;
using XCScript.Functions.Exceptions;

namespace XCScript.Functions.Control
{
    internal class If : IFunction
    {
        public string Keyword
        {
            get
            {
                return "if";
            }
        }

        public object Execute(IArgument[] arguments, Dictionary<string, object> globals)
        {
            if (arguments.Length < 2)
            {
                throw new ArgumentCountException("'if' requires at least 2 arguments");
            }
            if (arguments.Length > 3)
            {
                var res = globals[Engine.RKey] as Result;
                res.Messages.Add($"'if' called with {arguments.Length} arguments, only first 3 will be used");
            }

            if ((bool)arguments[0].Evaluate(globals))
            {
                return arguments[1].Evaluate(globals);
            }
            else
            {
                return arguments.Length > 2 ? arguments[2].Evaluate(globals) : null;
            }
        }
    }
}
