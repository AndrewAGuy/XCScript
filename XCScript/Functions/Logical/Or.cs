using XCScript.Arguments;

namespace XCScript.Functions.Logical
{
    internal class Or : IFunction
    {
        public string Keyword
        {
            get
            {
                return "or";
            }
        }

        public object Execute(IArgument[] arguments, Engine context)
        {
            return Base.Or(arguments, context);
        }
    }
}
