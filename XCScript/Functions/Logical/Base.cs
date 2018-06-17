using System;
using System.Collections.Generic;
using XCScript.Arguments;
using XCScript.Functions.Exceptions;

namespace XCScript.Functions.Logical
{
    internal static class Base
    {
        public static bool Get(object o)
        {
            switch (o)
            {
                case bool b:
                    return b;
                case double d:
                    return d != 0.0;
                case int i:
                    return i != 0;
                default:
                    var s = o.ToString();
                    if (!bool.TryParse(s, out var v))
                    {
                        throw new ArgumentTypeException($"Cannot convert to bool ({o.GetType().FullName}): {s}");
                    }
                    return v;
            }
        }

        public static Tuple<bool, bool> Get(IArgument[] args, Dictionary<string, object> glob)
        {
            if (args.Length < 2)
            {
                throw new ArgumentCountException("Logical operations require 2 arguments");
            }
            else if (args.Length > 2)
            {
                (glob[Engine.RKey] as Result)?
                    .Messages.Add($"Logical operation called with {args.Length} arguments, first 2 will be used");
            }

            var b0 = Get(args[0].Evaluate(glob));
            var b1 = Get(args[1].Evaluate(glob));
            return new Tuple<bool, bool>(b0, b1);
        }
    }
}
