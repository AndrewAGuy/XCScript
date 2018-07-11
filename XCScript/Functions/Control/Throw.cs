using XCScript.Arguments;
using XCScript.Functions.Exceptions;

namespace XCScript.Functions.Control
{
    internal class Throw : IFunction
    {
        public string Keyword
        {
            get
            {
                return "throw";
            }
        }

        public object Execute(IArgument[] arguments, Engine context)
        {
            if (arguments.Length > 0)
            {
                if (arguments.Length > 1)
                {
                    context.Log($"'throw' called with {arguments.Length} arguments, only first will be used");
                }
                throw new ExecutionException(arguments[0].Evaluate(context).ToString());
            }
            throw new ExecutionException();
        }
    }
}
