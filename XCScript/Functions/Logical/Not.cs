using System.Collections.Generic;
using XCScript.Arguments;
using XCScript.Functions.Exceptions;

namespace XCScript.Functions.Logical
{
    internal class Not : IFunction
    {
        public string Keyword
        {
            get
            {
                return "not";
            }
        }

        public object Execute(IArgument[] arguments, Dictionary<string, object> globals)
        {
            if (arguments.Length == 0)
            {
                throw new ArgumentCountException("'not' requires 1 argument");
            }
            else if (arguments.Length > 1)
            {
                (globals[Engine.RKey] as Result)?
                    .Messages.Add($"'not' called with {arguments.Length} arguments, first will be used");
            }

            return !Base.Get(arguments[0].Evaluate(globals));
        }
    }
}
