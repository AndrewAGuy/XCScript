using System;
using XCScript.Arguments;
using XCScript.Functions.Exceptions;

namespace XCScript.Functions.Numeric
{
    internal class Power : IFunction
    {
        public string Keyword
        {
            get
            {
                return "pow";
            }
        }

        public object Execute(IArgument[] arguments, Engine context)
        {
            if (arguments.Length == 0)
            {
                throw new ArgumentCountException("Power takes at least one argument");
            }

            if (arguments.Length == 1)
            {
                return Math.Exp(arguments[0].ToDouble(context));
            }
            else
            {
                var pair = Base.Get(arguments, context);
                return Math.Pow(pair.Item1, pair.Item2);
            }
        }
    }
}
