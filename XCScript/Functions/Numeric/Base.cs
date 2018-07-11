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

        public static double Evaluate(IArgument[] args, Engine context, Func<double, double, double> func)
        {
            if (args.Length < 2)
            {
                throw new ArgumentCountException("Numeric operations require at least 2 arguments");
            }

            var val = Get(args[0].Evaluate(context));
            for (var i = 1; i < args.Length; ++i)
            {
                val = func(val, Get(args[i].Evaluate(context)));
            }
            return val;
        }

        public static Tuple<double, double> Get(IArgument[] args, Engine eng, bool only2 = true)
        {
            if (args.Length < 2)
            {
                throw new ArgumentCountException("Numeric comparisons require 2 arguments");
            }
            else if (args.Length > 2 && only2)
            {
                eng.Log($"Numeric operation called with {args.Length} arguments, first 2 will be used");
            }

            var d0 = Get(args[0].Evaluate(eng));
            var d1 = Get(args[1].Evaluate(eng));
            return new Tuple<double, double>(d0, d1);
        }
    }
}
