using XCScript.Arguments;

namespace XCScript.Functions.Numeric
{
    internal class Multiply : IFunction
    {
        public string Keyword
        {
            get
            {
                return "mul";
            }
        }

        public object Execute(IArgument[] arguments, Engine context)
        {
            return Base.Evaluate(arguments, context, (a, b) => a * b);
        }
    }
}
