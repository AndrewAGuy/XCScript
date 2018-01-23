using XCScript.Arguments;
using XCScript.Parsing.Exceptions;
using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace XCScript.Parsing
{
    /// <summary>
    /// Literals are parsed having been given the first character, and always end on the character after the argument
    /// </summary>
    internal static class Literals
    {
        public static IArgument Number(TextReader reader, ref char chr)
        {
            var integral = Utility.ReadInt(reader, ref chr);
            if (chr == '.')
            {
                if (!Utility.Advance(reader, out chr))
                {
                    throw new FinalCharacterException("Expected fractional value after '.'");
                }
                else if (!char.IsDigit(chr))
                {
                    throw new InvalidCharacterException($"Expected fractional value after '.': found '{chr}'");
                }
                var fractional = Utility.ReadFractional(reader, ref chr);
                var mantissa = integral + fractional;

                if (char.ToLower(chr) == 'e')
                {
                    if (!Utility.AdvanceWhiteSpace(reader, out chr))
                    {
                        throw new FinalCharacterException("Expected exponent after 'e'");
                    }
                    else if (!Utility.IsNumberStart(chr))
                    {
                        throw new InvalidCharacterException($"Expected exponent after 'e': found '{chr}'");
                    }
                    var exponent = Utility.ReadInt(reader, ref chr);
                    mantissa *= Math.Pow(10, exponent);
                }

                return new DoubleLiteral() { Value = mantissa };
            }
            return new IntegerLiteral() { Value = integral };
        }

        public static IArgument String(TextReader reader, ref char chr)
        {
            var str = new StringBuilder();
            while (Utility.Advance(reader, out chr))
            {
                if (chr == '\\')
                {
                    str.Append(chr);
                    if (!Utility.Advance(reader, out chr))
                    {
                        throw new FinalCharacterException("Expected escaped character after '\\'");
                    }
                    str.Append(chr);
                }
                else if (chr == '"')
                {
                    // If this fails (i.e. the quote is the last character), future calls to skip space will fail
                    Utility.Advance(reader, out chr);
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

        public static IArgument Default(TextReader reader, ref char chr)
        {
            if (Utility.IsNameStart(chr))
            {
                var name = Utility.ReadName(reader, ref chr);
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
            else if (Utility.IsNumberStart(chr))
            {
                return Number(reader, ref chr);
            }
            throw new InvalidCharacterException($"Not a valid name or number start: '{chr}'");
        }

        public static IArgument TypeName(TextReader reader, ref char chr)
        {
            // Arrive on seeing '/', get collections of names separated by '.'
            if (!Utility.Advance(reader, out chr))
            {
                throw new FinalCharacterException("Expected type name");
            }

            var str = new StringBuilder();
            while (true)
            {
                str.Append(Utility.ReadName(reader, ref chr));
                if (chr != '.')
                {
                    return new TypeNameLiteral() { Value = str.ToString() };
                }
                str.Append('.');
                // We now expect more
                if (!Utility.Advance(reader, out chr))
                {
                    throw new FinalCharacterException("Expected further type name");
                }
                if (!Utility.IsNameStart(chr))
                {
                    throw new InvalidCharacterException("Expected name start character: found " + chr);
                }
            }
        }
    }
}
