using System;
using System.Collections.Generic;
using System.IO;
using XCScript.Arguments;
using XCScript.Execution;
using XCScript.Functions.Exceptions;
using XCScript.Parsing.Exceptions;

namespace XCScript.Functions.Execution
{
    internal class Execute : IFunction
    {
        public string Keyword
        {
            get
            {
                return "exec";
            }
        }

        object IFunction.Execute(IArgument[] arguments, Dictionary<string, object> globals)
        {
            if (arguments.Length == 0)
            {
                throw new ArgumentCountException("'exec' requires 1 argument");
            }
            else if (arguments.Length > 1)
            {
                var res = globals[Engine.RKey] as Result;
                res.Messages.Add($"'exec' called with {arguments.Length} arguments, only first will be used");
            }

            switch (arguments[0].Evaluate(globals))
            {
                case Executable e:
                    e.Execute(globals);
                    break;
                case string s:
                    try
                    {
                        var functions = globals[Engine.FKey] as Dictionary<string, IFunction>;
                        var file = File.OpenText(s);
                        var exec = Parsing.Evaluatable.Parse(file, functions);
                        exec.Execute(globals);
                    }
                    catch (ExecutionException) // Propagate these out to the caller
                    {
                        throw;
                    }
                    catch (ParsingException p)
                    {
                        throw new ExecutionException($"'exec' caught parsing error in '{s}' ({p.GetType().Name}): {p.Message}");
                    }
                    catch (Exception ex)
                    {
                        throw new ExecutionException($"'exec' caught exception ({ex.GetType().Name}): {ex.Message}");
                    }
                    break;
                default:
                    throw new ArgumentTypeException("'exec' requires either executable or string arguments");
            }
            return null;
        }
    }
}
