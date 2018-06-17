using System;
using System.Collections.Generic;
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

        public object Execute(IArgument[] arguments, Dictionary<string, object> globals)
        {
            if (arguments.Length == 0)
            {
                throw new ArgumentCountException("Logarithm takes at least one argument");
            }

            if (arguments.Length == 1)
            {
                return Math.Log(Base.Get(arguments[0].Evaluate(globals)));
            }
            else
            {
                var pair = Base.Get(arguments, globals);
                return Math.Log(pair.Item2, pair.Item1);
            }
        }
    }
}
