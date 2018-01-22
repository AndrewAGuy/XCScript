using System.IO;
using System.Text;

namespace XCScript.Parsing
{
    internal static class Utility
    {
        public static bool IsNameCharacter(char ch)
        {
            return char.IsLetterOrDigit(ch) || ch == '_';
        }

        public static bool IsNameStart(char ch)
        {
            return char.IsLetter(ch) || ch == '_';
        }

        public static bool IsNumberStart(char ch)
        {
            return char.IsDigit(ch) || ch == '-' || ch == '+';
        }

        public static bool SkipWhiteSpace(TextReader reader, ref char chr)
        {
            while (char.IsWhiteSpace(chr))
            {
                var ichr = reader.Read();
                if (ichr == -1)
                {
                    return false;
                }
                chr = (char)ichr;
            }
            return true;
        }

        public static bool AdvanceWhiteSpace(TextReader reader, out char chr)
        {
            chr = '\0';
            var ichr = reader.Read();
            while (ichr != -1)
            {
                chr = (char)ichr;
                if (!char.IsWhiteSpace(chr))
                {
                    return true;
                }
                ichr = reader.Read();
            }
            return false;
        }

        public static bool Advance(TextReader reader, out char chr)
        {
            var ichr = reader.Read();
            if (ichr == -1)
            {
                chr = ' ';
                return false;
            }
            chr = (char)ichr;
            return true;
        }

        public static string ReadName(TextReader reader, ref char chr)
        {
            var str = new StringBuilder().Append(chr);
            while (Advance(reader, out chr))
            {
                if (!IsNameCharacter(chr))
                {
                    break;
                }
                str.Append(chr);
            }
            return str.ToString();
        }

        public static int ReadInt(TextReader reader, ref char chr)
        {
            var str = new StringBuilder().Append(chr);
            while (Advance(reader, out chr))
            {
                if (!char.IsDigit(chr))
                {
                    break;
                }
                str.Append(chr);
            }
            return int.TryParse(str.ToString(), out var i) ? i : 0;
        }

        public static double ReadFractional(TextReader reader, ref char chr)
        {
            var str = new StringBuilder("0.").Append(chr);
            while (Advance(reader, out chr))
            {
                if (!char.IsDigit(chr))
                {
                    break;
                }
                str.Append(chr);
            }
            return double.Parse(str.ToString());
        }
    }
}
