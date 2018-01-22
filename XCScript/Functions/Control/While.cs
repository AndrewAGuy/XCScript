using System.Collections.Generic;
using XCScript.Arguments;
using XCScript.Functions.Exceptions;

namespace XCScript.Functions.Control
{
    internal class While : IFunction
    {
        public string Keyword
        {
            get
            {
                return "while";
            }
        }

        public object Execute(IArgument[] arguments, Dictionary<string, object> globals)
        {
            if (arguments.Length < 2)
            {
                throw new ArgumentCountException("'while' requires 2 arguments");
            }
            else if (arguments.Length > 2)
            {
                var res = globals[Engine.RKey] as Result;
                res.Messages.Add($"'while' called with {arguments.Length} arguments, only first 2 will be used");
            }

            object obj = null;
            while ((bool)arguments[0].Evaluate(globals))
            {
                obj = arguments[1].Evaluate(globals);
            }
            return obj;
        }
    }
}
