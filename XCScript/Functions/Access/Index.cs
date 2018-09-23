using System.Collections.Generic;
using XCScript.Arguments;
using XCScript.Functions.Exceptions;

namespace XCScript.Functions.Access
{
    internal class Index : IFunction
    {
        public string Keyword
        {
            get
            {
                return "index";
            }
        }

        public object Execute(IArgument[] arguments, Engine context)
        {
            if (arguments.Length < 2)
            {
                throw new ArgumentCountException("'index' requires at least 2 arguments");
            }
            else if (arguments.Length > 3)
            {
                context.Log($"'index' called with {arguments.Length} arguments, only first 3 will be used", Engine.Warning);
            }

            var array = arguments[0].Evaluate(context) as object[];
            var index = arguments[1].ToInt(context);
            if (arguments.Length < 3)
            {
                return array[index];
            }
            else
            {
                var value = arguments[2].Evaluate(context);
                array[index] = value;
                return value;
            }
        }
    }
}
