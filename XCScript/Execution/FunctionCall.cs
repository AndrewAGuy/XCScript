using System.Text;
using XCScript.Arguments;
using XCScript.Functions;

namespace XCScript.Execution
{
    internal class FunctionCall : IStatement
    {
        public ArgumentList Arguments { get; set; } = new ArgumentList();

        public IFunction Function { get; set; }

        public object Evaluate(Engine context)
        {
            return this.Function.Execute(this.Arguments.Evaluate(), context);
        }

        public void Execute(Engine context)
        {
            Evaluate(context);
        }

        public override string ToString()
        {
            var str = new StringBuilder().Append(this.Function.Keyword);
            var args = this.Arguments.Evaluate();
            if (args.Length > 0)
            {
                str.Append(": ");
                str.Append(args.ToString());
            }
            return str.ToString();
        }
    }
}
