using System;
using System.Collections.Generic;
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

        public object Execute(IArgument[] arguments, Dictionary<string, object> globals)
        {
            if (arguments.Length == 0)
            {
                throw new ArgumentCountException("Power takes at least one argument");
            }

            if (arguments.Length == 1)
            {
                return Math.Exp(Base.Get(arguments[0].Evaluate(globals)));
            }
            else
            {
                var pair = Base.Get(arguments, globals);
                return Math.Pow(pair.Item1, pair.Item2);
            }
        }
    }
}
