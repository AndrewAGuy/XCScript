using System.Collections.Generic;

namespace XCScript.Arguments
{
    internal class NameArgument : IArgument
    {
        public string Value { get; set; }

        public object Evaluate(Dictionary<string, object> globals)
        {
            return globals.TryGetValue(this.Value, out var val) ? val : null;
        }

        public override string ToString()
        {
            return this.Value;
        }
    }
}
