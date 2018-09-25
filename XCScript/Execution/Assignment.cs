using System;
using System.Text;

namespace XCScript.Execution
{
    internal class Assignment : Statement
    {
        public Assignment(int line) : base(line)
        {
        }

        public FunctionCall Call { get; set; }

        public string[] Names { get; set; }

        public override void Execute(Engine context)
        {
            var res = this.Call.Evaluate(context);
            if (res is object[] obj)
            {
                var length = Math.Min(obj.Length, this.Names.Length);
                for (var i = 0; i < length; ++i)
                {
                    context.Globals[this.Names[i]] = obj[i];
                }
            }
            else
            {
                context.Globals[this.Names[0]] = res;
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
