using System;
using System.Collections.Generic;
using System.Text;
using XCScript.Arguments;

namespace XCScript.Execution
{
    internal class Copy : IStatement
    {
        public IArgument[] From { get; set; }
        public string[] To { get; set; }

        public void Execute(Dictionary<string, object> globals)
        {
            var length = Math.Min(this.From.Length, this.To.Length);
            for (var i = 0; i < length; ++i)
            {
                globals[this.To[i]] = this.From[i].Evaluate(globals);
            }
        }

        public override string ToString()
        {
            var str = new StringBuilder();
            var last = this.From.Length - 1;
            for (var i = 0; i < last; ++i)
            {
                str.Append(this.From[i].ToString());
                str.Append(", ");
            }
            str.Append(this.From[last].ToString());
            str.Append(" => ");
            last = this.To.Length - 1;
            for (var i = 0; i < last; ++i)
            {
                str.Append(this.To[i]);
                str.Append(", ");
            }
            str.Append(this.To[last]);
            return str.ToString();
        }
    }
}
