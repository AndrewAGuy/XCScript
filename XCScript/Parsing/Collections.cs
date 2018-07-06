using XCScript.Arguments;
using XCScript.Functions;
using XCScript.Parsing.Exceptions;
using System.Collections.Generic;
using System;

namespace XCScript.Parsing
{
    internal static class Collections
    {
        private static Tuple<string, IArgument> NameValue(CharSource source, ref char chr, Dictionary<string, IFunction> funcs)
        {
            // Arrives with first character of name seen and verified:
            var name = source.ReadName(ref chr);
            if (!source.SkipWhiteSpace(ref chr))
            {
                throw new FinalCharacterException("Expected an assignment to name-value pair");
            }
            else if (chr != '=')
            {
                throw new InvalidCharacterException($"Expected an assignment to name-value pair ('='): found '{chr}'");
            }

            if (!source.AdvanceWhiteSpace(out chr))
            {
                throw new FinalCharacterException("Expected a value to assign");
            }
            var value = Arguments.Single(source, ref chr, funcs);
            return new Tuple<string, IArgument>(name, value);
        }

        public static IArgument Dictionary(CharSource source, ref char chr, Dictionary<string, IFunction> funcs)
        {
            var dict = new DictionaryLiteral();
            // We come here having seen ',' or '{', so advance before checking
            while (source.AdvanceWhiteSpace(out chr))
            {
                if (chr == '}')
                {
                    goto ACCEPT;
                }
                else if (CharSource.IsNameStart(chr))
                {
                    var entry = NameValue(source, ref chr, funcs);
                    dict.Add(entry.Item1, entry.Item2);
                    if (!source.SkipWhiteSpace(ref chr))
                    {
                        break;
                    }
                    if (chr == '}')
                    {
                        goto ACCEPT;
                    }
                    else if (chr != ',')
                    {
                        throw new InvalidCharacterException($"Expected continuation (',') or closing ('}}'): found '{chr}'");
                    }
                }
                else
                {
                    throw new InvalidCharacterException($"Expected a statement or closing '}}', found {chr}");
                }
            }
            throw new FinalCharacterException("Expected closing '}'");
            ACCEPT:
            source.Advance(out chr);
            return dict;
        }

        public static IArgument Array(CharSource source, ref char chr, Dictionary<string, IFunction> funcs)
        {
            var arr = new ArrayLiteral();
            // Arrive here having seen '[' or ','
            while (source.AdvanceWhiteSpace(out chr))
            {
                if (chr == ']')
                {
                    goto ACCEPT;
                }
                else
                {
                    var arg = Arguments.Single(source, ref chr, funcs);
                    arr.Add(arg);
                    if (!source.SkipWhiteSpace(ref chr))
                    {
                        break;
                    }
                    if (chr == ']')
                    {
                        goto ACCEPT;
                    }
                    else if (chr != ',')
                    {
                        throw new InvalidCharacterException($"Expected continuation (',') or closing (']'): found '{chr}'");
                    }
                }
            }
            throw new FinalCharacterException("Expected closing ']'");
            ACCEPT:
            source.Advance(out chr);
            return arr;
        }
    }
}
