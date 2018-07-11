using XCScript.Arguments;

namespace XCScript.Functions.Logical
{
    internal class Xor : IFunction
    {
        public string Keyword
        {
            get
            {
                return "xor";
            }
        }

        public object Execute(IArgument[] arguments, Engine context)
        {
            var tup = Base.Get(arguments, context);
            return tup.Item1 ^ tup.Item2;
        }
    }
}
