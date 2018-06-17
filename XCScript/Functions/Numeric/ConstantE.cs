using System;
using System.Collections.Generic;
using XCScript.Arguments;

namespace XCScript.Functions.Numeric
{
    internal class ConstantE : IFunction
    {
        public string Keyword
        {
            get
            {
                return "e";
            }
        }

        public object Execute(IArgument[] arguments, Dictionary<string, object> globals)
        {
            return Math.E;
        }
    }
}
