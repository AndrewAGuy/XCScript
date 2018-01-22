using System.Collections.Generic;
using XCScript.Arguments;
using XCScript.Functions.Exceptions;

namespace XCScript.Functions.Access
{
    internal class Map : IFunction
    {
        public string Keyword
        {
            get
            {
                return "map";
            }
        }

        public object Execute(IArgument[] arguments, Dictionary<string, object> globals)
        {
            if (arguments.Length < 2)
            {
                throw new ArgumentCountException("'map' requires at least 2 arguments");
            }
            else if (arguments.Length > 3)
            {
                var res = globals[Engine.RKey] as Result;
                res.Messages.Add($"'map' called with {arguments.Length} arguments, only first 3 will be used");
            }

            var dict = arguments[0].Evaluate(globals) as Dictionary<string, object>;
            var key = arguments[1].Evaluate(globals) as string;
            if (arguments.Length < 3)
            {
                return dict[key];
            }
            else
            {
                var value = arguments[2].Evaluate(globals);
                dict[key] = value;
                return value;
            }
        }
    }
}
