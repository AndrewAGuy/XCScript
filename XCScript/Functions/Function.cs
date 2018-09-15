using System;
using XCScript.Arguments;
using XCScript.Parsing;

namespace XCScript.Functions
{
    /// <summary>
    /// Wraps free functions
    /// </summary>
    public class Function : IFunction
    {
        private readonly string keyword;
        private readonly Func<IArgument[], Engine, object> function;

        /// <summary>
        /// Creates function wrapper
        /// </summary>
        /// <param name="kw">Keyword, which is checked for validity</param>
        /// <param name="f">Function</param>
        public Function(string kw, Func<IArgument[], Engine, object> f)
        {
            if (string.IsNullOrWhiteSpace(kw))
            {
                throw new ArgumentException("Keyword cannot be empty");
            }
            if (!CharSource.IsNameStart(kw[0]))
            {
                throw new ArgumentException("Keyword must start with letter or underscore");
            }
            keyword = kw;
            function = f ?? ((a, e) => null);
        }

        /// <summary>
        /// 
        /// </summary>
        public string Keyword
        {
            get
            {
                return keyword;
            }
        }

        /// <summary>
        /// Evaluates stored function
        /// </summary>
        /// <param name="arguments"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public object Execute(IArgument[] arguments, Engine context)
        {
            return function(arguments, context);
        }
    }
}
