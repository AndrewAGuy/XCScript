using System.Collections.Generic;
using XCScript.Arguments;
using XCScript.Functions.Exceptions;
using XCScript.Plugins;

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

        public object Execute(IArgument[] arguments, Dictionary<string, object> globals)
        {
            if (arguments.Length == 0)
            {
                throw new ArgumentCountException("'alias' needs at least 1 argument");
            }
            else if (arguments.Length > 2)
            {
                var res = globals[Engine.RKey] as Result;
                res.Messages.Add($"'alias' called with {arguments.Length} arguments, only first 2 will be used");
            }

            var manager = globals[Engine.PKey] as Manager;
            if (arguments.Length == 1)
            {
                if (!(arguments[0].Evaluate(globals) is Dictionary<string, IArgument> dict))
                {
                    throw new ArgumentTypeException("'alias' called with 1 argument takes a dictionary");
                }
                foreach (var kv in dict)
                {
                    if (kv.Value.Evaluate(globals) is string s)
                    {
                        manager.Alias(kv.Key, s);
                    }
                }
            }
            else
            {
                if (arguments[0].Evaluate(globals) is string name &&
                    arguments[1].Evaluate(globals) is string type)
                {
                    manager.Alias(name, type);
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
