using System.Collections.Generic;
using XCScript.Arguments;
using XCScript.Functions.Exceptions;

namespace XCScript.Functions.Logical
{
    internal class And : IFunction
    {
        public string Keyword
        {
            get
            {
                return "and";
            }
        }

        public object Execute(IArgument[] arguments, Dictionary<string, object> globals)
        {
            return Base.And(arguments, globals);
        }
    }
}
