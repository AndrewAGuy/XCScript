using XCScript.Arguments;
using XCScript.Functions;
using XCScript.Parsing.Exceptions;
using System.Collections.Generic;
using System.Linq;
using XCScript.Execution;

namespace XCScript.Parsing
{
    internal static class Evaluatable
    {
        public static Executable Parse(CharSource source, Dictionary<string, IFunction> funcs)
        {
            // Get to first character
            var exec = new Executable();
            if (!source.Advance(out var chr))
            {
                return exec;
            }
            // Always ends on first character of next statement, if possible
            while (source.SkipWhiteSpace(ref chr))
            {
                var statement = Statement(source, ref chr, funcs);
                exec.Add(statement);
            }
            return exec;
        }

        public static IArgument Function(CharSource source, ref char chr, Dictionary<string, IFunction> funcs)
        {
            // Arrive here having seen '('
            if (!source.AdvanceWhiteSpace(out chr))
            {
                throw new FinalCharacterException("Expected a function name");
            }
            var name = Arguments.Single(source, ref chr, funcs);
            if (!(name is NameArgument nameArg))
            {
                throw new InvalidArgumentException($"Function name type error: Type = {name.GetType().Name}, Value = {name.ToString()}");
            }
            if (!funcs.TryGetValue(nameArg.Value, out var func))
            {
                throw new InvalidArgumentException($"Function does not exist: \"{nameArg.Value}\"");
            }
            // Get next character:
            if (!source.SkipWhiteSpace(ref chr))
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
                if (!source.AdvanceWhiteSpace(out chr))
                {
                    throw new FinalCharacterException("Expected argument list");
                }
                funcArg.Call.Arguments = Arguments.Multiple(source, ref chr, funcs);
                // Get closing bracket
                if (!source.SkipWhiteSpace(ref chr))
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
            source.Advance(out chr);
            return funcArg;
        }

        public static IArgument Executable(CharSource source, ref char chr, Dictionary<string, IFunction> funcs)
        {
            // Having seen '<'
            if (!source.AdvanceWhiteSpace(out chr))
            {
                throw new FinalCharacterException("Expected statements, or closing '>'");
            }
            var exec = new ExecutableArgument();
            // Statement reading takes us to the first character of the next potential
            while (true)
            {
                if (chr == '>')
                {
                    source.Advance(out chr);
                    return exec;
                }
                var statement = Statement(source, ref chr, funcs);
                exec.Value.Add(statement);
            }
        }

        private static string[] AssignTargets(CharSource source, ref char chr, Dictionary<string, IFunction> funcs)
        {
            // Seen '='
            if (!source.Advance(out chr))
            {
                throw new FinalCharacterException("Expected assignment token, \"=>\"");
            }
            else if (chr != '>')
            {
                throw new InvalidCharacterException($"Expected assignment token, \"=>\": found \"={chr}\"");
            }
            if (!source.AdvanceWhiteSpace(out chr))
            {
                throw new FinalCharacterException("Expected assignment");
            }
            var right = Arguments.Multiple(source, ref chr, funcs).Evaluate();
            if (!right.All(t => t is NameArgument))
            {
                throw new InvalidArgumentException($"Assigned types must be names");
            }
            var names = right.Select(t => (t as NameArgument).Value).ToArray();
            return names;
        }

        private static IStatement Copy(CharSource source, ref char chr, Dictionary<string, IFunction> funcs, IArgument[] left)
        {
            // Arrive having seen '=' from "=>"
            var names = AssignTargets(source, ref chr, funcs);
            return new Copy()
            {
                From = left,
                To = names
            };
        }

        private static IStatement Assign(CharSource source, ref char chr, Dictionary<string, IFunction> funcs, FunctionCall call)
        {
            // Arrives on '='
            var names = AssignTargets(source, ref chr, funcs);
            return new Assignment()
            {
                Call = call,
                Names = names
            };
        }

        private static IStatement CallArguments(CharSource source, ref char chr, Dictionary<string, IFunction> funcs, IFunction func)
        {
            // Arrive on ':'
            if (!source.AdvanceWhiteSpace(out chr))
            {
                throw new FinalCharacterException("Expected function arguments");
            }
            var args = Arguments.Multiple(source, ref chr, funcs);
            var fcall = new FunctionCall()
            {
                Function = func,
                Arguments = args
            };
            return chr == '=' ? Assign(source, ref chr, funcs, fcall) : fcall;
        }

        public static IStatement Statement(CharSource source, ref char chr, Dictionary<string, IFunction> funcs)
        {
            // Always arrive having seen a character
            var left = Arguments.Multiple(source, ref chr, funcs).Evaluate();
            if (chr == '=')
            {
                return Copy(source, ref chr, funcs, left);
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
                    return CallArguments(source, ref chr, funcs, func);
                }
                else
                {
                    // To assign the outputs of an argumentless script to a target, use (func)=>target
                    return new FunctionCall()
                    {
                        Function = func
                    };
                }
            }
        }
    }
}
