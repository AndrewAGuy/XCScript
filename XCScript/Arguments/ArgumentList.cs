using System.Collections.Generic;
using System.Text;

namespace XCScript.Arguments
{
    internal class ArgumentList
    {
        private readonly List<IArgument> args = new List<IArgument>();

        public void Add(IArgument arg)
        {
            args.Add(arg);
        }

        public IArgument[] Evaluate()
        {
            return args.ToArray();
        }

        public int Count
        {
            get
            {
                return args.Count;
            }
        }

        public override string ToString()
        {
            var str = new StringBuilder();
            foreach (var arg in args)
            {
                str.Append(arg.ToString()).Append(", ");
            }
            return str.Remove(str.Length - 2, 2).ToString();
        }
    }
}
