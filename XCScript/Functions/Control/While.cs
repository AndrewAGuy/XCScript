using XCScript.Arguments;
using XCScript.Functions.Exceptions;

namespace XCScript.Functions.Control
{
    internal class While : IFunction
    {
        public string Keyword
        {
            get
            {
                return "while";
            }
        }

        public object Execute(IArgument[] arguments, Engine context)
        {
            if (arguments.Length < 2)
            {
                throw new ArgumentCountException("'while' requires 2 arguments");
            }
            else if (arguments.Length > 2)
            {
                context.Log($"'while' called with {arguments.Length} arguments, only first 2 will be used", Engine.Warning);
            }

            object obj = null;
            while (Logical.Base.Get(arguments[0].Evaluate(context)))
            {
                obj = arguments[1].Evaluate(context);
            }
            return obj;
        }
    }
}
