using System.Collections.Generic;

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
        /// <param name="globals"></param>
        /// <returns></returns>
        object Evaluate(Dictionary<string, object> globals);
    }
}
