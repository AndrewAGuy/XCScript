using System.Collections.Generic;
using XCScript.Arguments;

namespace XCScript.Functions.Numeric
{
    internal class Add : IFunction
    {
        public string Keyword
        {
            get
            {
                return "add";
            }
        }

        public object Execute(IArgument[] arguments, Dictionary<string, object> globals)
        {
            return Base.Evaluate(arguments, globals, (a, b) => a + b);
        }
    }
}
