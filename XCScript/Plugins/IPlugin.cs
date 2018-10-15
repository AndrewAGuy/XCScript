using System.Collections.Generic;
using XCScript.Plugins.Interaction;

namespace XCScript.Plugins
{
    /// <summary>
    /// Represents a type that can be loaded dynamically, and modified using <see cref="IProperty"/>
    /// </summary>
    public interface IPlugin : IEnumerable<IProperty>
    {
        /// <summary>
        /// Gets the property with the given name, or null
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        IProperty this[string name] { get; }
    }
}
