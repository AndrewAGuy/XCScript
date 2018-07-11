using XCScript.Arguments;
using XCScript.Functions.Exceptions;

namespace XCScript.Functions.Access
{
    internal class Error : IFunction
    {
        public string Keyword
        {
            get
            {
                return "err";
            }
        }

        public object Execute(IArgument[] arguments, Engine context)
        {
            if (arguments.Length == 0)
            {
                throw new ArgumentCountException($"'err' requires 1 argument");
            }
            else if (arguments.Length > 1)
            {
                context.Log($"'err' called with {arguments.Length} arguments, only first will be used");
            }
            
            if (arguments[0].Evaluate(context) is string s)
            {
                context.Log(s, -1);
                return s;
            }
            else
            {
                throw new ArgumentTypeException($"'err' requires argument evaluation of type {typeof(string).FullName}");
            }
        }
    }
}
