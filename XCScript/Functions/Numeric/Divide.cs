using System.Collections.Generic;
using XCScript.Arguments;

namespace XCScript.Functions.Numeric
{
    internal class Divide : IFunction
    {
        public string Keyword
        {
            get
            {
                return "div";
            }
        }

        public object Execute(IArgument[] arguments, Dictionary<string, object> globals)
        {
            var tup = Base.Get(arguments, globals);
            return tup.Item1 / tup.Item2;
        }
    }
}
