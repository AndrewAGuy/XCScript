using System;
using System.Collections.Generic;
using System.IO;
using XCScript.Arguments;
using XCScript.Execution;
using XCScript.Functions.Exceptions;
using XCScript.Parsing.Exceptions;

namespace XCScript.Functions.Execution
{
    internal class Interpret : IFunction
    {
        public string Keyword
        {
            get
            {
                return "interp";
            }
        }

        public object Execute(IArgument[] arguments, Dictionary<string, object> globals)
        {
            if (arguments.Length == 0)
            {
                throw new ArgumentCountException("'interp' requires 1 argument");
            }
            else if (arguments.Length > 1)
            {
                var res = globals[Engine.RKey] as Result;
                res.Messages.Add($"'interp' called with {arguments.Length} arguments, only first will be used");
            }

            if (arguments[0].Literal is Executable e)
            {
                return e;
            }
            else if (arguments[0].Evaluate(globals) is string s)
            {
                try
                {
                    var functions = globals[Engine.FKey] as Dictionary<string, IFunction>;
                    var file = File.OpenText(s);
                    var exec = Parsing.Evaluatable.Parse(file, functions);
                    return exec;
                }
                catch (ParsingException p)
                {
                    throw new ExecutionException($"'interp' caught parsing error in '{s}' ({p.GetType().Name}): {p.Message}");
                }
                catch (Exception ex)
                {
                    throw new ExecutionException($"'interp' caught exception ({ex.GetType().Name}): {ex.Message}");
                }
            }
            else
            {
                throw new ArgumentTypeException("'interp' requires either executable or string arguments");
            }
        }
    }
}
