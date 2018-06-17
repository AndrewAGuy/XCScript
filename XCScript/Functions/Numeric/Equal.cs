using System;
using System.Collections.Generic;
using XCScript.Arguments;

namespace XCScript.Functions.Numeric
{
    internal class Equal : IFunction
    {
        public string Keyword
        {
            get
            {
                return "eq";
            }
        }

        private double tolerance = 0.0;

        public object Execute(IArgument[] arguments, Dictionary<string, object> globals)
        {
            if (arguments.Length == 1)
            {
                tolerance = Base.Get(arguments[0].Evaluate(globals));
                return null;
            }
            else
            {
                var tup = Base.Get(arguments, globals, false);
                if (arguments.Length > 2)
                {
                    return Math.Abs(tup.Item1 - tup.Item2) <= Base.Get(arguments[2].Evaluate(globals));
                }
                else
                {
                    return Math.Abs(tup.Item1 - tup.Item2) <= tolerance;
                }
            }
        }
    }
}
