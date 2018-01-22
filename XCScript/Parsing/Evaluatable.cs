using XCScript.Arguments;
using XCScript.Functions;
using XCScript.Parsing.Exceptions;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using XCScript.Execution;

namespace XCScript.Parsing
{
    internal static class Evaluatable
    {
        public static Executable Parse(TextReader reader, Dictionary<string, IFunction> funcs)
        {
            // Get to first character
            var exec = new Executable();
            if (!Utility.Advance(reader, out var chr))
            {
                return exec;
            }
            // Always ends on first character of next statement, if possible
            while (Utility.SkipWhiteSpace(reader, ref chr))
            {
                var statement = Statement(reader, ref chr, funcs);
                exec.Add(statement);
            }
            return exec;
        }

        public static IArgument Function(TextReader reader, ref char chr, Dictionary<string, IFunction> funcs)
        {
            // Arrive here having seen '('
            if (!Utility.AdvanceWhiteSpace(reader, out chr))
            {
                throw new FinalCharacterException("Expected a function name");
            }
            var name = Arguments.Single(reader, ref chr, funcs);
            if (!(name is NameArgument nameArg))
            {
                throw new InvalidArgumentException($"Function name type error: Type = {name.GetType().Name}, Value = {name.ToString()}");
            }
            if (!funcs.TryGetValue(nameArg.Value, out var func))
            {
                throw new InvalidArgumentException($"Function does not exist: \"{nameArg.Value}\"");
            }
            // Get next character:
            if (!Utility.SkipWhiteSpace(reader, ref chr))
            {
                throw new FinalCharacterException("Expected closing ')'");
            }
            var funcArg = new FunctionArgument()
            {
                Call = new FunctionCall()
                {
                    Function = func
                }
            };
            if (chr == ':')
            {
                // Arguments to go with it
                if (!Utility.AdvanceWhiteSpace(reader, out chr))
                {
                    throw new FinalCharacterException("Expected argument list");
                }
                funcArg.Call.Arguments = Arguments.Multiple(reader, ref chr, funcs);
                // Get closing bracket
                if (!Utility.SkipWhiteSpace(reader, ref chr))
                {
                    throw new FinalCharacterException("Expected closing ')'");
                }
                else if (chr != ')')
                {
                    throw new InvalidCharacterException($"Expected closing ')': found '{chr}'");
                }
            }
            else if (chr != ')')
            {
                throw new InvalidCharacterException($"Expected closing ')' or arguments (':'): found '{chr}'");
            }
            Utility.Advance(reader, out chr);
            return funcArg;
        }

        public static IArgument Executable(TextReader reader, ref char chr, Dictionary<string, IFunction> funcs)
        {
            // Having seen '<'
            if (!Utility.AdvanceWhiteSpace(reader, out chr))
            {
                throw new FinalCharacterException("Expected statements, or closing '>'");
            }
            var exec = new ExecutableArgument();
            // Statement reading takes us to the first character of the next potential
            while (true)
            {
                if (chr == '>')
                {
                    Utility.Advance(reader, out chr);
                    return exec;
                }
                var statement = Statement(reader, ref chr, funcs);
                exec.Value.Add(statement);
            }
        }

        private static string[] AssignTargets(TextReader reader, ref char chr, Dictionary<string, IFunction> funcs)
        {
            // Seen '='
            if (!Utility.Advance(reader, out chr))
            {
                throw new FinalCharacterException("Expected assignment token, \"=>\"");
            }
            else if (chr != '>')
            {
                throw new InvalidCharacterException($"Expected assignment token, \"=>\": found \"={chr}\"");
            }
            if (!Utility.AdvanceWhiteSpace(reader, out chr))
            {
                throw new FinalCharacterException("Expected assignment");
            }
            var right = Arguments.Multiple(reader, ref chr, funcs).Evaluate();
            if (!right.All(t => t is NameArgument))
            {
                throw new InvalidArgumentException($"Assigned types must be names");
            }
            var names = right.Select(t => (t as NameArgument).Value).ToArray();
            return names;
        }

        private static IStatement Copy(TextReader reader, ref char chr, Dictionary<string, IFunction> funcs, IArgument[] left)
        {
            // Arrive having seen '=' from "=>"
            var names = AssignTargets(reader, ref chr, funcs);
            return new Copy() { From = left, To = names };
        }

        private static IStatement Assign(TextReader reader, ref char chr, Dictionary<string, IFunction> funcs, FunctionCall call)
        {
            // Arrives on '='
            var names = AssignTargets(reader, ref chr, funcs);
            return new Assignment() { Call = call, Names = names };
        }

        private static IStatement CallArguments(TextReader reader, ref char chr, Dictionary<string, IFunction> funcs, IFunction func)
        {
            // Arrive on ':'
            if (!Utility.AdvanceWhiteSpace(reader, out chr))
            {
                throw new FinalCharacterException("Expected function arguments");
            }
            var args = Arguments.Multiple(reader, ref chr, funcs);
            var fcall = new FunctionCall() { Function = func, Arguments = args };
            return chr == '=' ? Assign(reader, ref chr, funcs, fcall) : fcall;
        }

        public static IStatement Statement(TextReader reader, ref char chr, Dictionary<string, IFunction> funcs)
        {
            // Always arrive having seen a character
            var left = Arguments.Multiple(reader, ref chr, funcs).Evaluate();
            if (chr == '=')
            {
                return Copy(reader, ref chr, funcs, left);
            }
            else
            {
                // Sanity check the function input
                if (left.Length != 1)
                {
                    throw new InvalidArgumentException("Expected single function name");
                }
                if (!(left[0] is NameArgument fname))
                {
                    throw new InvalidArgumentException("Expected name argument type for function");
                }
                if (!funcs.TryGetValue(fname.Value, out var func))
                {
                    throw new InvalidArgumentException("Function name does not exist");
                }

                if (chr == ':')
                {
                    // Call syntax: func:arg,arg=>target == (func:arg,arg)=>target
                    return CallArguments(reader, ref chr, funcs, func);
                }
                else
                {
                    // To assign the outputs of an argumentless script to a target, use (func)=>target
                    return new FunctionCall() { Function = func };
                }
            }
        }
    }
}
