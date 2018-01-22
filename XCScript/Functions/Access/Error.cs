using System.Collections.Generic;
using XCScript.Arguments;
using XCScript.Functions.Exceptions;

namespace XCScript.Functions.Access
{
    internal class Error : IFunction
    {
        public string Keyword
        {
            get
            {
                return "err";
            }
        }

        public object Execute(IArgument[] arguments, Dictionary<string, object> globals)
        {
            if (arguments.Length == 0)
            {
                throw new ArgumentCountException($"'err' requires 1 argument");
            }
            else if (arguments.Length > 1)
            {
                var res = globals[Engine.RKey] as Result;
                res.Messages.Add($"'err' called with {arguments.Length} arguments, only first will be used");
            }
            
            if (arguments[0].Evaluate(globals) is string s)
            {
                (globals[Engine.RKey] as Result)?.Messages.Add(s);
                return s;
            }
            else
            {
                throw new ArgumentTypeException($"'error' requires argument evaluation of type {typeof(string).FullName}");
            }
        }
    }
}
