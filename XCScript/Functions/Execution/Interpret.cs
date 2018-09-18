using System;
using System.IO;
using XCScript.Arguments;
using XCScript.Execution;
using XCScript.Functions.Exceptions;
using XCScript.Parsing;
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

        public object Execute(IArgument[] arguments, Engine context)
        {
            if (arguments.Length == 0)
            {
                throw new ArgumentCountException("'interp' requires 1 argument");
            }
            else if (arguments.Length > 1)
            {
                context.Log($"'interp' called with {arguments.Length} arguments, only first will be used", Engine.Warning);
            }

            if (arguments[0].Literal is Executable e)
            {
                return e;
            }
            else if (arguments[0].Evaluate(context) is string s)
            {
                CharSource source = null;
                StreamReader file = null;
                try
                {
                    file = File.OpenText(s);
                    source = new CharSource(file);
                    return Evaluatable.Parse(source, context.Functions);
                }
                catch (ParsingException p)
                {
                    throw new ExecutionException($"'interp' caught {p.GetType().Name} in '{s}' at line {source.Line}: {p.Message}");
                }
                catch (Exception ex)
                {
                    throw new ExecutionException($"'interp' caught {ex.GetType().Name}: {ex.Message}");
                }
                finally
                {
                    file.Dispose();
                }
            }
            else
            {
                throw new ArgumentTypeException("'interp' requires either executable or string arguments");
            }
        }
    }
}
