using System;
using System.Text;
using XCScript.Arguments;

namespace XCScript.Execution
{
    internal class Copy : Statement
    {
        public Copy(int line) : base(line)
        {
        }

        public IArgument[] From { get; set; }
        public string[] To { get; set; }

        public override string Description
        {
            get
            {
                return $"Copy: {this.From.Length} arguments, {this.To.Length} targets";
            }
        }

        public override void Execute(Engine context)
        {
            var length = Math.Min(this.From.Length, this.To.Length);
            for (var i = 0; i < length; ++i)
            {
                context.Globals[this.To[i]] = this.From[i].Evaluate(context);
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
