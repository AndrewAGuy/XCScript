using System.Collections.Generic;
using XCScript.Arguments;

namespace XCScript.Functions.Logical
{
    internal class Or : IFunction
    {
        public string Keyword
        {
            get
            {
                return "or";
            }
        }

        public object Execute(IArgument[] arguments, Dictionary<string, object> globals)
        {
            var tup = Base.Get(arguments, globals);
            return tup.Item1 || tup.Item2;
        }
    }
}
