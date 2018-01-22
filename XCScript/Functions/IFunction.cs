using System.Collections.Generic;
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
        /// <param name="globals"></param>
        /// <returns></returns>
        object Execute(IArgument[] arguments, Dictionary<string, object> globals);

        /// <summary>
        /// The keyword that will be used in <see cref="Engine.Functions"/>
        /// </summary>
        string Keyword { get; }

        /// <summary>
        /// Allows the function to initialise itself, by getting and setting context elements
        /// </summary>
        /// <param name="context"></param>
        void Initialise(Dictionary<string, object> context);
    }
}
