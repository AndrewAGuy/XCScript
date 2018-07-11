using XCScript.Arguments;

namespace XCScript.Functions.Numeric
{
    internal class Less : IFunction
    {
        public string Keyword
        {
            get
            {
                return "ls";
            }
        }

        public object Execute(IArgument[] arguments, Engine context)
        {
            var tup = Base.Get(arguments, context);
            return tup.Item1 < tup.Item2;
        }
    }
}
