using XCScript.Arguments;

namespace XCScript.Functions.Logical
{
    internal class And : IFunction
    {
        public string Keyword
        {
            get
            {
                return "and";
            }
        }

        public object Execute(IArgument[] arguments, Engine context)
        {
            return Base.And(arguments, context);
        }
    }
}
