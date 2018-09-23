using System;
using XCScript.Arguments;
using XCScript.Functions.Exceptions;

namespace XCScript.Functions.Logical
{
    internal static class Base
    {
        public static bool And(IArgument[] args, Engine context)
        {
            if (args.Length < 2)
            {
                throw new ArgumentCountException("Logical and operations require at least 2 arguments");
            }
            foreach (var arg in args)
            {
                if (!arg.ToBool(context))
                {
                    return false;
                }
            }
            return true; // All arguments true
        }

        public static bool Or(IArgument[] args, Engine context)
        {
            if (args.Length < 2)
            {
                throw new ArgumentCountException("Logical or operations require at least 2 arguments");
            }
            foreach (var arg in args)
            {
                if (arg.ToBool(context))
                {
                    return true;
                }
            }
            return false; // All false
        }

        public static Tuple<bool, bool> Get(IArgument[] args, Engine context)
        {
            if (args.Length < 2)
            {
                throw new ArgumentCountException("Logical operations require 2 arguments");
            }
            else if (args.Length > 2)
            {
                context.Log($"Logical operation called with {args.Length} arguments, first 2 will be used", Engine.Warning);
            }

            var b0 = args[0].ToBool(context);
            var b1 = args[1].ToBool(context);
            return new Tuple<bool, bool>(b0, b1);
        }
    }
}
