using System.Collections.Generic;

namespace XCScript.Arguments
{
    internal class TypeNameLiteral : IArgument
    {
        public string Value { get; set; }

        public object Evaluate(Dictionary<string, object> globals)
        {
            return this.Value;
        }

        public override string ToString()
        {
            return "/" + this.Value;
        }
    }
}
