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

        public void DeleteArray(IArgument[] array, Engine context)
        {
            foreach (var arg in array)
            {
                // To remove an object with the name xyz, call del:/xyz
                // Otherwise it tries to get a string pointed stored as
                switch (arg.Evaluate(context))
                {
                    case string s:
                        context.Globals.Remove(s);
                        break;
                    case IArgument[] a:
                        DeleteArray(a, context);
                        break;
                }
            }
        }

        public object Execute(IArgument[] arguments, Engine context)
        {
            if (arguments.Length == 0)
            {
                context.Log($"'del' called with 0 targets");
            }
            DeleteArray(arguments, context);
            return null;
        }
    }
}
