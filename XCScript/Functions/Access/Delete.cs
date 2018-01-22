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
                switch (arg)
                {
                    case NameArgument name:
                        globals.Remove(name.Value);
                        break;
                    case ArrayLiteral arr:
                        DeleteArray(arr.Evaluate(globals) as IArgument[], globals);
                        break;
                    default:
                        if (arg.Evaluate(globals) is string str)
                        {
                            globals.Remove(str);
                        }
                        break;
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
