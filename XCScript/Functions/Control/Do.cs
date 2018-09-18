using XCScript.Arguments;
using XCScript.Functions.Exceptions;

namespace XCScript.Functions.Control
{
    internal class Do : IFunction
    {
        public string Keyword
        {
            get
            {
                return "do";
            }
        }

        public object Execute(IArgument[] arguments, Engine context)
        {
            if (arguments.Length < 2)
            {
                throw new ArgumentCountException("'do' requires 2 arguments");
            }
            else if (arguments.Length > 3)
            {
                context.Log($"'do' called with {arguments.Length} arguments, only first 3 will be used", Engine.Warning);
            }

            string name = null;
            if (arguments.Length > 2)
            {
                name = arguments[2].Evaluate(context) as string;
            }

            var end = Access.Index.Get(arguments[0].Evaluate(context));
            object obj = null;
            for (var i = 0; i < end; ++i)
            {
                if (name != null)
                {
                    context.Globals[name] = i;
                }
                obj = arguments[1].Evaluate(context);
            }
            return obj;
        }
    }
}
