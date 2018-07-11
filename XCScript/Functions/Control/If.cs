using XCScript.Arguments;
using XCScript.Functions.Exceptions;

namespace XCScript.Functions.Control
{
    internal class If : IFunction
    {
        public string Keyword
        {
            get
            {
                return "if";
            }
        }

        public object Execute(IArgument[] arguments, Engine context)
        {
            if (arguments.Length < 2)
            {
                throw new ArgumentCountException("'if' requires at least 2 arguments");
            }
            if (arguments.Length > 3)
            {
                context.Log($"'if' called with {arguments.Length} arguments, only first 3 will be used");
            }

            if (Logical.Base.Get(arguments[0].Evaluate(context)))
            {
                return arguments[1].Evaluate(context);
            }
            else
            {
                return arguments.Length > 2 ? arguments[2].Evaluate(context) : null;
            }
        }
    }
}
