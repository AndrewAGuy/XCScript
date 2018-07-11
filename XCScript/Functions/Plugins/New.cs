using System.Collections.Generic;
using XCScript.Arguments;
using XCScript.Functions.Exceptions;
using XCScript.Plugins;

namespace XCScript.Functions.Plugins
{
    internal class New : IFunction
    {
        public string Keyword
        {
            get
            {
                return "new";
            }
        }

        public object Execute(IArgument[] arguments, Engine context)
        {
            if (arguments.Length == 0)
            {
                throw new ArgumentCountException("'new' requires at least the type to instantiate");
            }
            else if (arguments.Length > 2)
            {
                context.Log($"'new' called with {arguments.Length} arguments, only first 2 will be used");
            }

            var typename = arguments[0].Evaluate(context) as string;
            if (typename == null)
            {
                throw new ArgumentTypeException("First argument to 'new' must be or evaluate to string");
            }

            var dict = new Dictionary<string, object>();
            if (arguments.Length > 1)
            {
                var argdict = arguments[1].Evaluate(context) as Dictionary<string, IArgument>;
                if (argdict == null)
                {
                    throw new ArgumentTypeException("'new' second argument must be a dictionary");
                }
                foreach (var arg in argdict)
                {
                    dict[arg.Key] = arg.Value.Evaluate(context);
                }
            }
            return context.Plugins.Create(typename, dict);
        }
    }
}
