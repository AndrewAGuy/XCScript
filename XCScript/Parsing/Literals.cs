using XCScript.Arguments;
using XCScript.Parsing.Exceptions;
using System;
using System.Text;
using System.Text.RegularExpressions;

namespace XCScript.Parsing
{
    /// <summary>
    /// Literals are parsed having been given the first character, and always end on the character after the argument
    /// </summary>
    internal static class Literals
    {
        public static IArgument Number(CharSource source, ref char chr)
        {
            var integral = source.ReadInt(ref chr);
            if (chr == '.')
            {
                if (!source.Advance(out chr))
                {
                    throw new FinalCharacterException("Expected fractional value after '.'");
                }
                else if (!char.IsDigit(chr))
                {
                    throw new InvalidCharacterException($"Expected fractional value after '.': found '{chr}'");
                }
                var fractional = source.ReadFractional(ref chr);
                var mantissa = integral >= 0 ? integral + fractional : integral - fractional;

                if (char.ToLower(chr) == 'e')
                {
                    if (!source.AdvanceWhiteSpace(out chr))
                    {
                        throw new FinalCharacterException("Expected exponent after 'e'");
                    }
                    else if (!CharSource.IsNumberStart(chr))
                    {
                        throw new InvalidCharacterException($"Expected exponent after 'e': found '{chr}'");
                    }
                    var exponent = source.ReadInt(ref chr);
                    mantissa *= Math.Pow(10, exponent);
                }

                return new DoubleLiteral()
                {
                    Value = mantissa
                };
            }
            return new IntegerLiteral()
            {
                Value = integral
            };
        }

        public static IArgument String(CharSource source, ref char chr)
        {
            var str = new StringBuilder();
            while (source.Advance(out chr, true))
            {
                if (chr == '\\')
                {
                    str.Append(chr);
                    if (!source.Advance(out chr, true))
                    {
                        throw new FinalCharacterException("Expected escaped character after '\\'");
                    }
                    str.Append(chr);
                }
                else if (chr == '"')
                {
                    // If this fails (i.e. the quote is the last character), future calls to skip space will fail
                    source.Advance(out chr);
                    return new StringLiteral()
                    {
                        Value = Regex.Unescape(str.ToString())
                    };
                }
                else
                {
                    str.Append(chr);
                }
            }
            throw new FinalCharacterException("Expected closing '\"'");
        }

        public static IArgument Default(CharSource source, ref char chr)
        {
            if (CharSource.IsNameStart(chr))
            {
                var name = source.ReadName(ref chr);
                if (bool.TryParse(name, out var bval))
                {
                    return new BooleanLiteral()
                    {
                        Value = bval
                    };
                }
                return new NameArgument()
                {
                    Value = name
                };
            }
            else if (CharSource.IsNumberStart(chr))
            {
                return Number(source, ref chr);
            }
            throw new InvalidCharacterException($"Not a valid name or number start: '{chr}'");
        }

        public static IArgument TypeName(CharSource source, ref char chr)
        {
            // Arrive on seeing '/', get collections of names separated by '.'
            if (!source.Advance(out chr))
            {
                throw new FinalCharacterException("Expected type name");
            }

            var str = new StringBuilder();
            while (true)
            {
                str.Append(source.ReadName(ref chr));
                if (chr != '.')
                {
                    return new TypeNameLiteral()
                    {
                        Value = str.ToString()
                    };
                }
                str.Append('.');
                // We now expect more
                if (!source.Advance(out chr))
                {
                    throw new FinalCharacterException("Expected further type name");
                }
                if (!CharSource.IsNameStart(chr))
                {
                    throw new InvalidCharacterException("Expected name start character: found " + chr);
                }
            }
        }
    }
}
