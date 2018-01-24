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

        public object Execute(IArgument[] arguments, Dictionary<string, object> globals)
        {
            if (arguments.Length == 0)
            {
                throw new ArgumentCountException("'new' requires at least the type to instantiate");
            }
            else if (arguments.Length > 2)
            {
                var res = globals[Engine.RKey] as Result;
                res.Messages.Add($"'new' called with {arguments.Length} arguments, only first 2 will be used");
            }

            var typename = arguments[0].Evaluate(globals) as string;
            if (typename == null)
            {
                throw new ArgumentTypeException("First argument to 'new' must be or evaluate to string");
            }

            var dict = new Dictionary<string, object>();
            if (arguments.Length > 1)
            {
                var argdict = arguments[1].Evaluate(globals) as Dictionary<string, IArgument>;
                if (argdict == null)
                {
                    throw new ArgumentTypeException("'new' second argument must be a dictionary");
                }
                foreach (var arg in argdict)
                {
                    dict[arg.Key] = arg.Value.Evaluate(globals);
                }
            }
            return (globals[Engine.PKey] as Manager)?.Create(typename, dict);
        }
    }
}
