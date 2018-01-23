using System.Collections.Generic;
using XCScript.Arguments;

namespace XCScript.Functions.Access
{
    internal class Delete : IFunction
    {
        public string Keyword
        {
            get
            {
                return "del";
            }
        }

        public void DeleteArray(IArgument[] array, Dictionary<string, object> globals)
        {
            foreach (var arg in array)
            {
                if (arg.Literal is string str)
                {
                    globals.Remove(str);
                }
                else
                {
                    switch (arg.Evaluate(globals))
                    {
                        case string s:
                            globals.Remove(s);
                            break;
                        case IArgument[] a:
                            DeleteArray(a, globals);
                            break;
                    }
                }
            }
        }

        public object Execute(IArgument[] arguments, Dictionary<string, object> globals)
        {
            if (arguments.Length == 0)
            {
                var res = globals[Engine.RKey] as Result;
                res.Messages.Add($"'del' called with 0 targets");
            }
            DeleteArray(arguments, globals);
            return null;
        }
    }
}
