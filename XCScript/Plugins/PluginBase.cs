using System.Collections;
using System.Collections.Generic;
using System.Linq;
using XCScript.Plugins.Interaction;

namespace XCScript.Plugins
{
    /// <summary>
    /// Useful base implementation for <see cref="IPlugin"/>
    /// </summary>
    public class PluginBase : IEnumerable
    {
        /// <summary>
        /// Property set, as accessible by general purpose users
        /// </summary>
        protected Dictionary<string, IProperty> Properties { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerator<IProperty> GetEnumerator()
        {
            return this.Properties.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="property"></param>
        protected void Add(IProperty property)
        {
            this.Properties[property.Name] = property;
        }

        /// <summary>
        /// Defaults to blank set
        /// </summary>
        public PluginBase()
        {
            this.Properties = new Dictionary<string, IProperty>();
        }

        /// <summary>
        /// Makes property set from collection
        /// </summary>
        /// <param name="props"></param>
        public PluginBase(IEnumerable<IProperty> props)
        {
            this.Properties = props.ToDictionary(p => p.Name);
        }

        /// <summary>
        /// Gets property with given name, or null
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public IProperty this[string name]
        {
            get
            {
                this.Properties.TryGetValue(name, out var prop);
                return prop;
            }
        }
    }
}
