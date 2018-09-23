using System;
using XCScript.Arguments;
using XCScript.Functions.Exceptions;

namespace XCScript.Functions.Numeric
{
    internal static class Base
    {
        public static double Evaluate(IArgument[] args, Engine context, Func<double, double, double> func)
        {
            if (args.Length < 2)
            {
                throw new ArgumentCountException("Numeric operations require at least 2 arguments");
            }

            var val = args[0].ToDouble(context);
            for (var i = 1; i < args.Length; ++i)
            {
                val = func(val, args[i].ToDouble(context));
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
                eng.Log($"Numeric operation called with {args.Length} arguments, first 2 will be used", Engine.Warning);
            }

            var d0 = args[0].ToDouble(eng);
            var d1 = args[1].ToDouble(eng);
            return new Tuple<double, double>(d0, d1);
        }
    }
}
