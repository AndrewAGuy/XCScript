using XCScript.Arguments;

namespace XCScript.Functions
{
    /// <summary>
    /// Represents a function, which maps multiple arguments to one object, given a context
    /// </summary>
    public interface IFunction
    {
        /// <summary>
        /// Execute the function with the given arguments and context
        /// </summary>
        /// <param name="arguments"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        object Execute(IArgument[] arguments, Engine context);

        /// <summary>
        /// The keyword that will be used in <see cref="Engine.Functions"/>
        /// </summary>
        string Keyword { get; }
    }
}
