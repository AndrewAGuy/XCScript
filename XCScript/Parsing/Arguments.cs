using XCScript.Functions;
using XCScript.Arguments;
using System.Collections.Generic;

namespace XCScript.Parsing
{
    internal static class Arguments
    {
        public static ArgumentList Multiple(CharSource source, ref char chr, Dictionary<string, IFunction> funcs)
        {
            var args = new ArgumentList();
            // Typically get here having seen nothing or the first character already
            // Running out of text here is not an error, unless specified as more to come
            while (source.SkipWhiteSpace(ref chr))
            {
                var arg = Single(source, ref chr, funcs);
                args.Add(arg);
                if (!source.SkipWhiteSpace(ref chr))
                {
                    return args;
                }
                else if (chr != ',')
                {
                    return args;
                }
                source.Advance(out chr);
            }
            // No need to throw here, as if more expected single argument parsing will catch
            return args;
        }

        public static IArgument Single(CharSource source, ref char chr, Dictionary<string, IFunction> funcs)
        {
            switch (chr)
            {
                case '"':
                    return Literals.String(source, ref chr);
                case '{':
                    return Collections.Dictionary(source, ref chr, funcs);
                case '[':
                    return Collections.Array(source, ref chr, funcs);
                case '(':
                    return Evaluatable.Function(source, ref chr, funcs);
                case '<':
                    return Evaluatable.Executable(source, ref chr, funcs);
                case '/':
                    return Literals.TypeName(source, ref chr);
                case '!':
                    return new ResultArgument(source, ref chr);
                default:
                    return Literals.Default(source, ref chr);
            }
        }
    }
}
