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

        public object Execute(IArgument[] arguments, Engine context)
        {
            if (arguments.Length == 1)
            {
                tolerance = arguments[0].ToDouble(context);
                return null;
            }
            else
            {
                var tup = Base.Get(arguments, context, false);
                if (arguments.Length > 2)
                {
                    return Math.Abs(tup.Item1 - tup.Item2) <= arguments[2].ToDouble(context);
                }
                else
                {
                    return Math.Abs(tup.Item1 - tup.Item2) <= tolerance;
                }
            }
        }
    }
}
