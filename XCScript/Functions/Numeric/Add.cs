using XCScript.Arguments;

namespace XCScript.Functions.Numeric
{
    internal class Add : IFunction
    {
        public string Keyword
        {
            get
            {
                return "add";
            }
        }

        public object Execute(IArgument[] arguments, Engine context)
        {
            return Base.Evaluate(arguments, context, (a, b) => a + b);
        }
    }
}
