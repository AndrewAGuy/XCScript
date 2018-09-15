using System.Text;
using XCScript.Arguments;

namespace XCScript.Functions.Text
{
    internal class Concatenate : IFunction
    {
        public string Keyword
        {
            get
            {
                return "strcat";
            }
        }

        public object Execute(IArgument[] arguments, Engine context)
        {
            var str = new StringBuilder();
            foreach (var arg in arguments)
            {
                str.Append(arg.Evaluate(context).ToString());
            }
            return str.ToString();
        }
    }
}
