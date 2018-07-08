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

        public object Execute(IArgument[] arguments, Engine context)
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
                context.Log($"'prop' called with {arguments.Length} arguments, only first 3 will be used");
            }

            var plugin = arguments[0].Evaluate(context.Globals) as IPlugin;
            if (plugin == null)
            {
                throw new ArgumentTypeException("'prop' requires the first argument to be of type IPlugin");
            }

            var arg1 = arguments[1].Evaluate(context.Globals);
            if (arg1 is Dictionary<string, IArgument> dict)
            {
                foreach (var kv in dict)
                {
                    var prop = plugin[kv.Key];
                    if (prop == null)
                    {
                        context.Log($"No such property: '{kv.Key}' on object of type {plugin.GetType().FullName}");
                    }
                    else
                    {
                        var res = prop.TrySetValue(kv.Value.Evaluate(context.Globals));
                        if (!res.Success)
                        {
                            context.Log($"In property: '{kv.Key}' on object of type {plugin.GetType().FullName}" + res.Messages[0]);
                        }
                    }
                }
                return plugin;
            }
            else if (arg1 is string name)
            {
                var prop = plugin[name];
                if (prop == null)
                {
                    context.Log($"No such property: '{name}' on object of type {plugin.GetType().FullName}");
                    return null;
                }

                if (arguments.Length == 2)
                {
                    return prop.Value;
                }

                var res = prop.TrySetValue(arguments[2].Evaluate(context.Globals));
                if (!res.Success)
                {
                    context.Log($"In property: '{name}' on object of type {plugin.GetType().FullName} - " + res.Messages[0]);
                }
                return plugin;
            }
            else
            {
                throw new ArgumentTypeException("'prop' requires string or dictionary arguments");
            }
        }
    }
}
