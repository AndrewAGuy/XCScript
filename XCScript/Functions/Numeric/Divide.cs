using XCScript.Arguments;
using XCScript.Functions.Exceptions;

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

        public object Execute(IArgument[] arguments, Engine context)
        {
            return Base.Evaluate(arguments, context, (a, b) =>
            {
                if (b == 0.0)
                {
                    throw new ExecutionException("Division by 0");
                }
                return a / b;
            });
        }
    }
}
