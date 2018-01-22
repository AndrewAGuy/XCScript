using XCScript.Functions;
using XCScript.Arguments;
using System.Collections.Generic;
using System.IO;
using XCScript.Execution;

namespace XCScript.Parsing
{
    internal static class Arguments
    {
        public static ArgumentList Multiple(TextReader reader, ref char chr, Dictionary<string, IFunction> funcs)
        {
            var args = new ArgumentList();
            // Typically get here having seen nothing or the first character already
            // Running out of text here is not an error, unless specified as more to come
            while (Utility.SkipWhiteSpace(reader, ref chr))
            {
                var arg = Single(reader, ref chr, funcs);
                args.Add(arg);
                if (!Utility.SkipWhiteSpace(reader, ref chr))
                {
                    return args;
                }
                else if (chr != ',')
                {
                    return args;
                }
                Utility.Advance(reader, out chr);
            }
            // No need to throw here, as if more expected single argument parsing will catch
            return args;
        }
        
        public static IArgument Single(TextReader reader, ref char chr, Dictionary<string, IFunction> funcs)
        {
            switch (chr)
            {
                case '"':
                    return Literals.String(reader, ref chr);
                case '{':
                    return Collections.Dictionary(reader, ref chr, funcs);
                case '[':
                    return Collections.Array(reader, ref chr, funcs);
                case '(':
                    return Evaluatable.Function(reader, ref chr, funcs);
                case '<':
                    return Evaluatable.Executable(reader, ref chr, funcs);
                case '/':
                    return Literals.TypeName(reader, ref chr);
                default:
                    return Literals.Default(reader, ref chr);
            }
        }
    }
}
