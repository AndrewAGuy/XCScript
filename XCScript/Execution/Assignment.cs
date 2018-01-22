using System;
using System.Collections.Generic;
using System.Text;

namespace XCScript.Execution
{
    internal class Assignment : IStatement
    {
        public FunctionCall Call { get; set; }

        public string[] Names { get; set; }

        public void Execute(Dictionary<string, object> globals)
        {
            var res = this.Call.Evaluate(globals);
            if (res is object[] obj)
            {
                var length = Math.Min(obj.Length, this.Names.Length);
                for (var i = 0; i < length; ++i)
                {
                    globals[this.Names[i]] = obj[i];
                }
            }
            else
            {
                globals[this.Names[0]] = res;
            }
        }

        public override string ToString()
        {
            var str = new StringBuilder(this.Call.ToString());
            str.Append(" => ");
            var last = this.Names.Length - 1;
            for (var i = 0; i < last; ++i)
            {
                str.Append(this.Names[i]);
                str.Append(", ");
            }
            str.Append(this.Names[last]);
            return str.ToString();
        }
    }
}
