using System.Collections.Generic;
using XCScript.Arguments;
using XCScript.Functions.Exceptions;

namespace XCScript.Functions.Plugins
{
    internal class Alias : IFunction
    {
        public string Keyword
        {
            get
            {
                return "alias";
            }
        }

        public object Execute(IArgument[] arguments, Engine context)
        {
            if (arguments.Length == 0)
            {
                throw new ArgumentCountException("'alias' needs at least 1 argument");
            }
            else if (arguments.Length > 2)
            {
                context.Log($"'alias' called with {arguments.Length} arguments, only first 2 will be used", Engine.Warning);
            }
            
            if (arguments.Length == 1)
            {
                if (!(arguments[0].Evaluate(context) is Dictionary<string, IArgument> dict))
                {
                    throw new ArgumentTypeException("'alias' called with 1 argument takes a dictionary");
                }
                foreach (var kv in dict)
                {
                    if (kv.Value.Evaluate(context) is string s)
                    {
                        context.Plugins.Alias(kv.Key, s);
                    }
                }
            }
            else
            {
                if (arguments[0].Evaluate(context) is string name &&
                    arguments[1].Evaluate(context) is string type)
                {
                    context.Plugins.Alias(name, type);
                }
                else
                {
                    throw new ArgumentTypeException("'alias' maps 2 strings to each other");
                }
            }
            return null;
        }
    }
}
