using System;
using System.Collections.Generic;
using XCScript.Arguments;
using XCScript.Functions.Exceptions;

namespace XCScript.Functions.Control
{
    internal class Foreach : IFunction
    {
        public string Keyword
        {
            get
            {
                return "foreach";
            }
        }

        public object Execute(IArgument[] arguments, Dictionary<string, object> globals)
        {
            if (arguments.Length < 3)
            {
                throw new ArgumentCountException("Foreach requires at least 3 arguments: values, name, statement");
            }

            if (!(arguments[0].Evaluate(globals) is Array arr))
            {
                throw new ArgumentTypeException("Foreach requires array as first argument");
            }
            if (!(arguments[1].Evaluate(globals) is string name))
            {
                throw new ArgumentTypeException("Foreach requires target name as second argument");
            }

            object res = null;
            foreach (var obj in arr)
            {
                globals[name] = (obj is IArgument arg) ? arg.Evaluate(globals) : obj;
                res = arguments[2].Evaluate(globals);
            }

            return res;
        }
    }
}
