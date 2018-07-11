using XCScript.Arguments;

namespace XCScript.Functions.Numeric
{
    internal class LessEqual : IFunction
    {
        public string Keyword
        {
            get
            {
                return "le";
            }
        }

        public object Execute(IArgument[] arguments, Engine context)
        {
            var tup = Base.Get(arguments, context);
            return tup.Item1 <= tup.Item2;
        }
    }
}
