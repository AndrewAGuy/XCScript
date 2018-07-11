using System;
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

        object IFunction.Execute(IArgument[] arguments, Engine context)
        {
            if (arguments.Length == 0)
            {
                throw new ArgumentCountException("'exec' requires 1 argument");
            }
            else if (arguments.Length > 1)
            {
               context.Log($"'exec' called with {arguments.Length} arguments, only first will be used");
            }

            switch (arguments[0].Evaluate(context))
            {
                case Executable e:
                    e.Execute(context);
                    break;

                case string s:
                    CharSource source = null;
                    StreamReader file = null;
                    try
                    {
                        file = File.OpenText(s);
                        source = new CharSource(file);
                        Evaluatable.Parse(source, context.Functions).Execute(context);
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
