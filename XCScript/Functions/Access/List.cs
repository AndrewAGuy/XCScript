using System;
using System.Collections;
using System.Collections.Generic;
using XCScript.Arguments;
using XCScript.Functions.Exceptions;

namespace XCScript.Functions.Access
{
    internal class List : IFunction
    {
        public string Keyword
        {
            get
            {
                return "list";
            }
        }

        private int GetIndex(IArgument[] args, Engine context)
        {
            if (args[2].Evaluate(context) is int i)
            {
                return i;
            }
            throw new ArgumentTypeException("'list' - expected index");
        }

        /// <summary>
        /// Used to create, set, get, insert, append and remove lists of objects
        /// </summary>
        /// <param name="arguments"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public object Execute(IArgument[] arguments, Engine context)
        {
            switch (arguments.Length)
            {
                case 0:
                    return new List<object>();

                case 1:
                    var count = 0;
                    if (arguments[0].Evaluate(context) is int i)
                    {
                        count = i;
                    }
                    return new List<object>(count);

                case 2:
                    throw new ArgumentCountException("'list' - no mode takes 2 arguments");

                default:
                    var mode = arguments[0].Evaluate(context).ToString();
                    if (!(arguments[1].Evaluate(context) is IList list))
                    {
                        throw new ArgumentTypeException("'list' - second argument must be IList");
                    }
                    switch (mode)
                    {
                        case "a":
                            list.Add(arguments[2].Evaluate(context));
                            break;

                        case "g":
                            return list[GetIndex(arguments, context)];

                        case "i":
                            if (arguments.Length < 4)
                            {
                                throw new ArgumentCountException("'list' - insert requires 4 arguments");
                            }
                            list.Insert(GetIndex(arguments, context), arguments[3].Evaluate(context));
                            break;

                        case "r":
                            list.RemoveAt(GetIndex(arguments, context));
                            break;

                        case "s":
                            if (arguments.Length < 4)
                            {
                                throw new ArgumentCountException("'list' - set requires 4 arguments");
                            }
                            list[GetIndex(arguments, context)] = arguments[3].Evaluate(context);
                            break;

                        default:
                            throw new ArgumentException($"'list' - mode not recognised ({mode})." +
                                "Supported modes: (a)ppend,(g)et,(i)nsert,(r)emove,(s)et");
                    }
                    return null;
            }
        }
    }
}
