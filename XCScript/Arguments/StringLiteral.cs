using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace XCScript.Arguments
{
    internal class StringLiteral : IArgument
    {
        public string Value { get; set; }

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
            var str = new StringBuilder().Append('"');
            str.Append(Regex.Escape(this.Value));
            return str.Append('"').ToString();
        }
    }
}
