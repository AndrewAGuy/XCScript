namespace XCScript.Arguments
{
    /// <summary>
    /// Represents either a literal or a contextually resolved argument
    /// </summary>
    public interface IArgument
    {
        /// <summary>
        /// Given the current context, resolve this argument.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        object Evaluate(Engine context);

        /// <summary>
        /// Given the current context, resolve this argument and all children. <para/>
        /// Converts return types of <see cref="IArgument"/> to <see cref="object"/> (dictionaries and arrays).
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        object EvaluateChildren(Engine context);

        /// <summary>
        /// Gets the stored object without evaluation
        /// </summary>
        object Literal { get; }
    }
}
