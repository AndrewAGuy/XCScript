﻿using System.Collections.Generic;
using System.Text;

namespace XCScript.Arguments
{
    internal class ArrayLiteral : IArgument
    {
        private readonly List<IArgument> args = new List<IArgument>();

        public void Add(IArgument arg)
        {
            args.Add(arg);
        }

        public object Evaluate(Dictionary<string, object> globals)
        {
            return args.ToArray();
        }

        public override string ToString()
        {
            var str = new StringBuilder().Append("[ ");
            foreach (var a in args)
            {
                str.Append(a.ToString()).Append(", ");
            }
            return str.Remove(str.Length - 2, 2).Append(" ]").ToString();
        }
    }
}