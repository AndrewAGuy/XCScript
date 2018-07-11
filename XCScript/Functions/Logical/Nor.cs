using XCScript.Arguments;

namespace XCScript.Functions.Logical
{
    internal class Nor : IFunction
    {
        public string Keyword
        {
            get
            {
                return "nor";
            }
        }

        public object Execute(IArgument[] arguments, Engine context)
        {
            return !Base.Or(arguments, context);
        }
    }
}
