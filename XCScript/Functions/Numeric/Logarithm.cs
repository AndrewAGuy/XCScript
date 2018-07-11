using System;
using XCScript.Arguments;
using XCScript.Functions.Exceptions;

namespace XCScript.Functions.Numeric
{
    internal class Logarithm : IFunction
    {
        public string Keyword
        {
            get
            {
                return "log";
            }
        }

        public object Execute(IArgument[] arguments, Engine context)
        {
            if (arguments.Length == 0)
            {
                throw new ArgumentCountException("Logarithm takes at least one argument");
            }

            if (arguments.Length == 1)
            {
                return Math.Log(Base.Get(arguments[0].Evaluate(context)));
            }
            else
            {
                var pair = Base.Get(arguments, context);
                return Math.Log(pair.Item2, pair.Item1);
            }
        }
    }
}
