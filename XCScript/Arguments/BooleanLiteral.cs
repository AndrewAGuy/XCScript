using System.Collections.Generic;

namespace XCScript.Arguments
{
    internal class BooleanLiteral : IArgument
    {
        public bool Value { get; set; }

        public object Literal
        {
            get
            {
                return this.Value;
            }
        }

        public object Evaluate(Dictionary<string, object> globals)
        {
            return this.Value;
        }

        public override string ToString()
        {
            return this.Value.ToString().ToLower();
        }
    }
}
