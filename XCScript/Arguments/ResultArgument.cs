using XCScript.Parsing;

namespace XCScript.Arguments
{
    internal class ResultArgument : IArgument
    {
        public ResultArgument(CharSource source, ref char chr)
        {
            source.Advance(out chr);
        }

        public object Literal
        {
            get
            {
                return null;
            }
        }

        public object Evaluate(Engine context)
        {
            return context.Result;
        }

        public object EvaluateChildren(Engine context)
        {
            return context.Result;
        }
    }
}
