namespace XCScript.Arguments
{
    /// <summary>
    /// 
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        /// <param name="context"></param>
        /// <returns></returns>
       public static object[] Evaluate(this IArgument[] args, Engine context)
        {
            var array = new object[args.Length];
            for (var i = 0; i < args.Length; ++i)
            {
                array[i] = args[i].Evaluate(context);
            }
            return array;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public static object[] EvaluateChildren(this IArgument[] args, Engine context)
        {
            var array = new object[args.Length];
            for (var i = 0; i < args.Length; ++i)
            {
                array[i] = args[i].EvaluateChildren(context);
            }
            return array;
        }
    }
}
