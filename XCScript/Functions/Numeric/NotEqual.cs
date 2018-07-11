using System;
using XCScript.Arguments;

namespace XCScript.Functions.Numeric
{
    internal class NotEqual : IFunction
    {
        public string Keyword
        {
            get
            {
                return "ne";
            }
        }

        private double tolerance = 0.0;

        public object Execute(IArgument[] arguments, Engine context)
        {
            if (arguments.Length == 1)
            {
                tolerance = Base.Get(arguments[0].Evaluate(context));
                return null;
            }
            else
            {
                var tup = Base.Get(arguments, context, false);
                if (arguments.Length > 2)
                {
                    return Math.Abs(tup.Item1 - tup.Item2) > Base.Get(arguments[2].Evaluate(context));
                }
                else
                {
                    return Math.Abs(tup.Item1 - tup.Item2) > tolerance;
                }
            }
        }
    }
}
