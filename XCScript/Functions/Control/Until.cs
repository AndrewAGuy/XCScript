using XCScript.Arguments;
using XCScript.Functions.Exceptions;

namespace XCScript.Functions.Control
{
    internal class Until : IFunction
    {
        public string Keyword
        {
            get
            {
                return "until";
            }
        }

        public object Execute(IArgument[] arguments, Engine context)
        {
            if (arguments.Length < 2)
            {
                throw new ArgumentCountException("'until' requires 2 arguments");
            }
            else if (arguments.Length > 2)
            {
                context.Log($"'until' called with {arguments.Length} arguments, only first 2 will be used", Engine.Warning);
            }

            object obj = null;
            do
            {
                obj = arguments[1].Evaluate(context);
            } while (arguments[0].ToBool(context));
            return obj;
        }
    }
}
