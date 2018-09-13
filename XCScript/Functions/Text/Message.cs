using System.Collections.Generic;
using XCScript.Arguments;

namespace XCScript.Functions.Text
{
    internal class Message : IFunction
    {
        public string Keyword
        {
            get
            {
                return "msg";
            }
        }

        private Dictionary<string, int> codes = new Dictionary<string, int>()
        {
            { "information", Engine.Information },
            { "info", Engine.Information },
            { "i", Engine.Information },
            { "error", Engine.Error },
            { "err", Engine.Error },
            { "e", Engine.Error },
            { "warning", Engine.Warning },
            { "warn", Engine.Warning },
            { "w", Engine.Warning }
        };

        public object Execute(IArgument[] arguments, Engine context)
        {
            if (arguments.Length == 0)
            {
                context.Log(null); // Sends a timestamp only
            }

            var code = Engine.Information;
            if (arguments.Length > 1)
            {
                switch (arguments[1].Evaluate(context))
                {
                    case int i:
                        code = i;
                        break;
                    case string s:
                        if (codes.TryGetValue(s.ToLower(), out var c))
                        {
                            code = c;
                        }
                        break;
                }
            }

            context.Log(arguments[0].Evaluate(context).ToString(), code);
            return null;
        }
    }
}
