using XCScript.Arguments;

namespace XCScript.Functions.Numeric
{
    internal class Greater : IFunction
    {
        public string Keyword
        {
            get
            {
                return "gr";
            }
        }

        public object Execute(IArgument[] arguments, Engine context)
        {
            var tup = Base.Get(arguments, context);
            return tup.Item1 > tup.Item2;
        }
    }
}
