﻿using XCScript.Functions.Exceptions;

namespace XCScript.Arguments
{
    /// <summary>
    /// 
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        /// <param name="context"></param>
        /// <returns></returns>
       public static object[] Evaluate(this IArgument[] args, Engine context)
        {
            var array = new object[args.Length];
            for (var i = 0; i < args.Length; ++i)
            {
                array[i] = args[i].Evaluate(context);
            }
            return array;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public static object[] EvaluateChildren(this IArgument[] args, Engine context)
        {
            var array = new object[args.Length];
            for (var i = 0; i < args.Length; ++i)
            {
                array[i] = args[i].EvaluateChildren(context);
            }
            return array;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="arg"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public static double ToDouble(this IArgument arg, Engine context)
        {
            var o = arg.Evaluate(context) ?? throw new ArgumentTypeException("Evaluated to null", arg);
            switch (o)
            {
                case double d:
                    return d;
                case int i:
                    return i;
                default:
                    var s = o.ToString();
                    if (!double.TryParse(s, out var v))
                    {
                        throw new ArgumentTypeException($"Cannot convert to double ({o.GetType().FullName}): {s}", arg);
                    }
                    return v;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="arg"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public static int ToInt(this IArgument arg, Engine context)
        {
            var o = arg.Evaluate(context) ?? throw new ArgumentTypeException("Evaluated to null", arg);
            switch (o)
            {
                case int i:
                    return i;
                case double d:
                    return (int)d;
                default:
                    var s = o.ToString();
                    if (!int.TryParse(s, out var v))
                    {
                        throw new ArgumentTypeException($"Cannot convert to int ({o.GetType().FullName}): {s}", arg);
                    }
                    return v;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="arg"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public static bool ToBool(this IArgument arg, Engine context)
        {
            var o = arg.Evaluate(context) ?? throw new ArgumentTypeException("Evaluated to null", arg);
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
                        throw new ArgumentTypeException($"Cannot convert to bool ({o.GetType().FullName}): {s}", arg);
                    }
                    return v;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="arg"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public static T To<T>(this IArgument arg, Engine context)
        {
            var o = arg.Evaluate(context) ?? throw new ArgumentTypeException("Evaluated to null", arg);
            if (o is T t)
            {
                return t;
            }
            throw new ArgumentTypeException($"Cannot convert to {typeof(T)} ({o.GetType()}): {o}", arg);
        }

        /// <summary>
        /// Returns report of argument type and value
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        public static string Report(this IArgument arg)
        {
            return $"{arg.GetType().Name}: {arg}";
        }
    }
}
