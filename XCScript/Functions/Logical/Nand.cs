using XCScript.Arguments;

namespace XCScript.Functions.Logical
{
    internal class Nand : IFunction
    {
        public string Keyword
        {
            get
            {
                return "nand";
            }
        }

        public object Execute(IArgument[] arguments, Engine context)
        {
            return !Base.And(arguments, context);
        }
    }
}
