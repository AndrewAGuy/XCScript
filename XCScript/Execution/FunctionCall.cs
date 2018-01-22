using System.Collections.Generic;
using System.Text;
using XCScript.Arguments;
using XCScript.Commands;

namespace XCScript.Execution
{
    internal class FunctionCall : IStatement
    {
        public ArgumentList Arguments { get; set; } = new ArgumentList();

        public IFunction Function { get; set; }

        public object Evaluate(Dictionary<string, object> globals)
        {
            return this.Function.Execute(this.Arguments.Evaluate(), globals);
        }

        public void Execute(Dictionary<string, object> globals)
        {
            Evaluate(globals);
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
