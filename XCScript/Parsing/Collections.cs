using XCScript.Arguments;
using XCScript.Functions;
using XCScript.Parsing.Exceptions;
using System.Collections.Generic;
using System.IO;
using System;

namespace XCScript.Parsing
{
    internal static class Collections
    {
        private static Tuple<string, IArgument> NameValue(TextReader reader, ref char chr, Dictionary<string, IFunction> funcs)
        {
            // Arrives with first character of name seen and verified:
            var name = Utility.ReadName(reader, ref chr);
            if (!Utility.SkipWhiteSpace(reader, ref chr))
            {
                throw new FinalCharacterException("Expected an assignment to name-value pair");
            }
            else if (chr != '=')
            {
                throw new InvalidCharacterException($"Expected an assignment to name-value pair ('='): found '{chr}'");
            }

            if (!Utility.AdvanceWhiteSpace(reader, out chr))
            {
                throw new FinalCharacterException("Expected a value to assign");
            }
            var value = Arguments.Single(reader, ref chr, funcs);
            return new Tuple<string, IArgument>(name, value);
        }

        public static IArgument Dictionary(TextReader reader, ref char chr, Dictionary<string, IFunction> funcs)
        {
            var dict = new DictionaryLiteral();
            // We come here having seen ',' or '{', so advance before checking
            while (Utility.AdvanceWhiteSpace(reader, out chr))
            {
                if (chr == '}')
                {
                    goto ACCEPT;
                }
                else if (Utility.IsNameStart(chr))
                {
                    var entry = NameValue(reader, ref chr, funcs);
                    dict.Add(entry.Item1, entry.Item2);
                    if (!Utility.SkipWhiteSpace(reader, ref chr))
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
            Utility.Advance(reader, out chr);
            return dict;
        }

        public static IArgument Array(TextReader reader, ref char chr, Dictionary<string, IFunction> funcs)
        {
            var arr = new ArrayLiteral();
            // Arrive here having seen '[' or ','
            while (Utility.AdvanceWhiteSpace(reader, out chr))
            {
                if (chr == ']')
                {
                    goto ACCEPT;
                }
                else
                {
                    var arg = Arguments.Single(reader, ref chr, funcs);
                    arr.Add(arg);
                    if (!Utility.SkipWhiteSpace(reader, ref chr))
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
            Utility.Advance(reader, out chr);
            return arr;
        }
    }
}
