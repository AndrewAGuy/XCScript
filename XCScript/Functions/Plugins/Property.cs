using System.Collections.Generic;
using XCScript.Arguments;
using XCScript.Functions.Exceptions;
using XCScript.Plugins;

namespace XCScript.Functions.Plugins
{
    internal class Property : IFunction
    {
        public string Keyword
        {
            get
            {
                return "prop";
            }
        }

        public object Execute(IArgument[] arguments, Dictionary<string, object> globals)
        {
            // Variations:
            //      Get: IPlugin, string => object
            //      Set: IPlugin, string, object
            //      Set: IPlugin, dict {string => object}
            if (arguments.Length < 2)
            {
                throw new ArgumentCountException("'prop' requires at least 2 arguments");
            }
            else if (arguments.Length > 3)
            {
                var res = globals[Engine.RKey] as Result;
                res.Messages.Add($"'prop' called with {arguments.Length} arguments, only first 3 will be used");
            }

            var plugin = arguments[0].Evaluate(globals) as IPlugin;
            if (plugin == null)
            {
                throw new ArgumentTypeException("'prop' requires the first argument to be of type IPlugin");
            }

            var arg1 = arguments[1].Evaluate(globals);
            if (arg1 is Dictionary<string, IArgument> dict)
            {
                foreach (var kv in dict)
                {
                    plugin[kv.Key]?.TrySetValue(kv.Value.Evaluate(globals));
                }
                return plugin;
            }
            else if (arg1 is string name)
            {
                if (arguments.Length > 2)
                {
                    plugin[name]?.TrySetValue(arguments[2].Evaluate(globals));
                    return plugin;
                }
                else
                {
                    return plugin[name]?.Value;
                }
            }
            else
            {
                throw new ArgumentTypeException("'prop' requires string or dictionary arguments");
            }
        }
    }
}
