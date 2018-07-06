using System.IO;
using System.Text;

namespace XCScript.Parsing
{
    internal class CharSource
    {
        private TextReader reader;
        private int line = 1;

        public CharSource(TextReader source)
        {
            reader = source;
        }

        public int Line
        {
            get
            {
                return line;
            }
        }

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

        public bool SkipWhiteSpace(ref char chr)
        {
            while (char.IsWhiteSpace(chr))
            {
                if (!Advance(out chr))
                {
                    return false;
                }
            }
            return true;
        }

        public bool AdvanceWhiteSpace(out char chr)
        {
            while (Advance(out chr))
            {
                if (!char.IsWhiteSpace(chr))
                {
                    return true;
                }
            }
            return false;
        }

        public bool AdvanceLine(out char chr)
        {
            chr = '\n';
            var ichr = reader.Read();
            while (ichr != -1)
            {
                if ((char)ichr == '\n')
                {
                    // Don't advance another, else we might get phrase continuations over lines
                    line++;
                    return true;
                }
                ichr = reader.Read();
            }
            return false;
        }

        public bool Advance(out char chr, bool stringParse = false)
        {
            var ichr = reader.Read();
            if (ichr == -1)
            {
                chr = ' ';
                return false;
            }
            chr = (char)ichr;
            if (chr == '#' && !stringParse)
            {
                // Remainder of line is a comment
                return AdvanceLine(out chr);
            }
            if (chr == '\n')
            {
                line++;
            }
            return true;
        }

        public string ReadName(ref char chr)
        {
            var str = new StringBuilder().Append(chr);
            while (Advance(out chr))
            {
                if (!IsNameCharacter(chr))
                {
                    break;
                }
                str.Append(chr);
            }
            return str.ToString();
        }

        public int ReadInt(ref char chr)
        {
            var str = new StringBuilder().Append(chr);
            while (Advance(out chr))
            {
                if (!char.IsDigit(chr))
                {
                    break;
                }
                str.Append(chr);
            }
            return int.TryParse(str.ToString(), out var i) ? i : 0;
        }

        public double ReadFractional(ref char chr)
        {
            var str = new StringBuilder("0.").Append(chr);
            while (Advance(out chr))
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
