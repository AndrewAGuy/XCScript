using System.Collections.Generic;
using XCScript.Execution;

namespace XCScript.Arguments
{
    internal class FunctionArgument : IArgument
    {
        public FunctionCall Call { get; set; }

        public object Evaluate(Dictionary<string, object> globals)
        {
            return this.Call.Evaluate(globals);
        }

        public override string ToString()
        {
            return '(' + this.Call.ToString() + ')';
        }
    }
}
