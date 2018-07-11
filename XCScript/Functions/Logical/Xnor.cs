using XCScript.Arguments;

namespace XCScript.Functions.Logical
{
    internal class Xnor : IFunction
    {
        public string Keyword
        {
            get
            {
                return "xnor";
            }
        }

        public object Execute(IArgument[] arguments, Engine context)
        {
            var tup = Base.Get(arguments, context);
            return !(tup.Item1 ^ tup.Item2);
        }
    }
}
