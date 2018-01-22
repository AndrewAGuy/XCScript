using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XCScript.Arguments;
using XCScript.Functions.Exceptions;

namespace XCScript.Functions.Access
{
    internal class Index : IFunction
    {
        public string Keyword
        {
            get
            {
                return "index";
            }
        }

        public object Execute(IArgument[] arguments, Dictionary<string, object> globals)
        {
            if (arguments.Length < 2)
            {
                throw new ArgumentCountException("'index' requires at least 2 arguments");
            }
            else if (arguments.Length > 3)
            {
                var res = globals[Engine.RKey] as Result;
                res.Messages.Add($"'index' called with {arguments.Length} arguments, only first 3 will be used");
            }

            var array = arguments[0].Evaluate(globals) as object[];
            var index = (int)arguments[1].Evaluate(globals);
            if (arguments.Length < 3)
            {
                return array[index];
            }
            else
            {
                var value = arguments[2].Evaluate(globals);
                array[index] = value;
                return value;
            }
        }
    }
}
