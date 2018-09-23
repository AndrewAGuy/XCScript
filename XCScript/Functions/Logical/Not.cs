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

        public object Execute(IArgument[] arguments, Engine context)
        {
            if (arguments.Length == 0)
            {
                throw new ArgumentCountException("'not' requires 1 argument");
            }
            else if (arguments.Length > 1)
            {
                context.Log($"'not' called with {arguments.Length} arguments, first will be used", Engine.Warning);
            }

            return !arguments[0].ToBool(context);
        }
    }
}
