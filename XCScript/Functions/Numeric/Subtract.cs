using XCScript.Arguments;

namespace XCScript.Functions.Numeric
{
    internal class Subtract : IFunction
    {
        public string Keyword
        {
            get
            {
                return "sub";
            }
        }

        public object Execute(IArgument[] arguments, Engine context)
        {
            return Base.Evaluate(arguments, context, (a, b) => a - b);
        }
    }
}
