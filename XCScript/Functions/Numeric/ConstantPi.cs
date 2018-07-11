using System;
using XCScript.Arguments;

namespace XCScript.Functions.Numeric
{
    internal class ConstantPi : IFunction
    {
        public string Keyword
        {
            get
            {
                return "pi";
            }
        }

        public object Execute(IArgument[] arguments, Engine context)
        {
            return Math.PI;
        }
    }
}
