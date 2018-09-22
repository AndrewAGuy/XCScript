using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XCScript.Arguments
{
    internal class DictionaryLiteral : IArgument
    {
        private readonly Dictionary<string, IArgument> args = new Dictionary<string, IArgument>();

        public void Add(string key, IArgument value)
        {
            args[key] = value;
        }

        public object Literal
        {
            get
            {
                return args;
            }
        }

        public object Evaluate(Engine context)
        {
            return args;
        }

        public override string ToString()
        {
            var str = new StringBuilder().Append("{ ");
            foreach (var kv in args)
            {
                str.Append(kv.Key).Append(" = ").Append(kv.Value.ToString()).Append(", ");
            }
            return str.Remove(str.Length - 2, 2).Append(" }").ToString();
        }

        public object EvaluateChildren(Engine context)
        {
            var dict = new Dictionary<string, object>();
            foreach (var kv in args)
            {
                dict[kv.Key] = kv.Value.EvaluateChildren(context);
            }
            return dict;
        }
    }
}
