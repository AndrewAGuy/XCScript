using System;
using System.Collections.Generic;
using XCScript.Arguments;
using XCScript.Functions.Exceptions;

namespace XCScript.Functions.Numeric
{
    internal static class Base
    {
        public static double Get(object o)
        {
            switch (o)
            {
                case double d:
                    return d;
                case int i:
                    return i;
                case float f:
                    return f;
                default:
                    var s = o.ToString();
                    if (!double.TryParse(s, out var v))
                    {
                        throw new ArgumentTypeException($"Cannot convert to double ({o.GetType().FullName}): {s}");
                    }
                    return v;
            }
        }

        public static Tuple<double, double> Get(IArgument[] args, Dictionary<string, object> glob)
        {
            if (args.Length < 2)
            {
                throw new ArgumentCountException("Numeric operations require 2 arguments");
            }
            else if (args.Length > 2)
            {
                (glob[Engine.RKey] as Result)?
                    .Messages.Add($"Numeric operation called with {args.Length} arguments, first 2 will be used");
            }

            var d0 = Get(args[0].Evaluate(glob));
            var d1 = Get(args[1].Evaluate(glob));
            return new Tuple<double, double>(d0, d1);
        }
    }
}
