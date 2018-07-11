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

        public object Execute(IArgument[] arguments, Engine context)
        {
            if (arguments.Length < 2)
            {
                throw new ArgumentCountException("'map' requires at least 2 arguments");
            }
            else if (arguments.Length > 3)
            {
                context.Log($"'map' called with {arguments.Length} arguments, only first 3 will be used");
            }

            var dict = arguments[0].Evaluate(context) as Dictionary<string, object>;
            var key = arguments[1].Evaluate(context) as string;
            if (arguments.Length < 3)
            {
                return dict[key];
            }
            else
            {
                var value = arguments[2].Evaluate(context);
                dict[key] = value;
                return value;
            }
        }
    }
}
