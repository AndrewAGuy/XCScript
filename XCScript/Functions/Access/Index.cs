using System.Collections.Generic;
using XCScript.Arguments;
using XCScript.Functions.Exceptions;

namespace XCScript.Functions.Access
{
    internal class Index : IFunction
    {
        public static int Get(object o)
        {
            switch (o)
            {
                case int i:
                    return i;
                case double d:
                    return (int)d;
                case float f:
                    return (int)f;
                default:
                    var s = o.ToString();
                    if (!int.TryParse(s, out var v))
                    {
                        throw new ArgumentTypeException($"Cannot convert to int ({o.GetType().FullName}): {s}");
                    }
                    return v;
            }
        }

        public string Keyword
        {
            get
            {
                return "index";
            }
        }

        public object Execute(IArgument[] arguments, Dictionary<string, object> globals)
        {
            if (arguments.Length < 2)
            {
                throw new ArgumentCountException("'index' requires at least 2 arguments");
            }
            else if (arguments.Length > 3)
            {
                var res = globals[Engine.RKey] as Result;
                res.Messages.Add($"'index' called with {arguments.Length} arguments, only first 3 will be used");
            }

            var array = arguments[0].Evaluate(globals) as object[];
            var index = Get(arguments[1].Evaluate(globals));
            if (arguments.Length < 3)
            {
                return array[index];
            }
            else
            {
                var value = arguments[2].Evaluate(globals);
                array[index] = value;
                return value;
            }
        }
    }
}
