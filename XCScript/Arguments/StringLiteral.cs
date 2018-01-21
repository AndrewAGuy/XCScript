using System.Collections.Generic;
using System.Text;

namespace XCScript.Arguments
{
    internal class StringLiteral : IArgument
    {
        public string Value { get; set; }

        public object Evaluate(Dictionary<string, object> globals)
        {
            return this.Value;
        }

        public override string ToString()
        {
            var str = new StringBuilder().Append('"');
            for (var i = 0; i < this.Value.Length; ++i)
            {
                if (this.Value[i] == '"')
                {
                    str.Append("\\\"");
                }
                else if (this.Value[i] == '\\')
                {
                    str.Append("\\\\");
                }
                else
                {
                    str.Append(this.Value[i]);
                }
            }
            return str.Append('"').ToString();
        }
    }
}
