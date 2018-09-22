using XCScript.Execution;

namespace XCScript.Arguments
{
    internal class FunctionArgument : IArgument
    {
        public FunctionCall Call { get; set; }

        public object Literal
        {
            get
            {
                // This is not public, but is mostly only good for evaluating anyway
                return this.Call;
            }
        }

        public object Evaluate(Engine context)
        {
            return this.Call.Evaluate(context);
        }

        public object EvaluateChildren(Engine context)
        {
            return this.Call.Evaluate(context);
        }

        public override string ToString()
        {
            return '(' + this.Call.ToString() + ')';
        }
    }
}
