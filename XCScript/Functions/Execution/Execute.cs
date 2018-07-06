using System;
using System.Collections.Generic;
using System.IO;
using XCScript.Arguments;
using XCScript.Execution;
using XCScript.Functions.Exceptions;
using XCScript.Parsing;
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
                    CharSource source = null;
                    StreamReader file = null;
                    try
                    {
                        var functions = globals[Engine.FKey] as Dictionary<string, IFunction>;
                        file = File.OpenText(s);
                        source = new CharSource(file);
                        Evaluatable.Parse(source, functions).Execute(globals);
                    }
                    catch (ExecutionException) // Propagate these out to the caller
                    {
                        throw;
                    }
                    catch (ParsingException p)
                    {
                        throw new ExecutionException($"'exec' caught {p.GetType().Name} in '{s}' at line {source.Line}: {p.Message}");
                    }
                    catch (Exception ex)
                    {
                        throw new ExecutionException($"'exec' caught exception ({ex.GetType().Name}): {ex.Message}");
                    }
                    finally
                    {
                        file.Dispose();
                    }
                    break;

                default:
                    throw new ArgumentTypeException("'exec' requires either executable or string arguments");
            }
            return null;
        }
    }
}
