namespace XCScript.Arguments
{
    /// <summary>
    /// Represents either a literal or a contextually resolved argument
    /// </summary>
    public interface IArgument
    {
        /// <summary>
        /// Given the current global contex, resolve this argument
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        object Evaluate(Engine context);

        /// <summary>
        /// Gets the stored object without evaluation
        /// </summary>
        object Literal { get; }
    }
}
