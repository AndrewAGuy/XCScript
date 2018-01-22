using System.Collections.Generic;
using XCScript.Arguments;
using XCScript.Functions.Exceptions;

namespace XCScript.Functions.Control
{
    internal class Until : IFunction
    {
        public string Keyword
        {
            get
            {
                return "until";
            }
        }

        public object Execute(IArgument[] arguments, Dictionary<string, object> globals)
        {
            if (arguments.Length < 2)
            {
                throw new ArgumentCountException("'until' requires 2 arguments");
            }
            else if (arguments.Length > 2)
            {
                var res = globals[Engine.RKey] as Result;
                res.Messages.Add($"'until' called with {arguments.Length} arguments, only first 2 will be used");
            }

            object obj = null;
            do
            {
                obj = arguments[1].Evaluate(globals);
            } while ((bool)arguments[0].Evaluate(globals));
            return obj;
        }
    }
}
