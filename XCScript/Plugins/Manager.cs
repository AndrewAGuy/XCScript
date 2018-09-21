using System;
using System.Collections.Generic;
using System.Reflection;

namespace XCScript.Plugins
{
    /// <summary>
    /// Manages plugins, loading and creating
    /// </summary>
    public class Manager
    {
        private readonly Dictionary<string, Type> types = new Dictionary<string, Type>();

        private bool IsValidType(Type t)
        {
            return typeof(IPlugin).IsAssignableFrom(t) && !t.IsInterface;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="t"></param>
        public bool TryAdd(Type t)
        {
            if (IsValidType(t))
            {
                types[t.FullName] = t;
                types[t.Name] = t;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Loads all types in the assembly that are <see cref="IPlugin"/> implementations
        /// </summary>
        /// <param name="assy"></param>
        public void FromAssembly(Assembly assy)
        {
            foreach (var t in assy.DefinedTypes)
            {
                TryAdd(t);
            }
        }

        /// <summary>
        /// Aliases first value to mean second
        /// </summary>
        /// <param name="name"></param>
        /// <param name="type"></param>
        public void Alias(string name, string type)
        {
            if (types.TryGetValue(type, out var t) && !string.IsNullOrWhiteSpace(name))
            {
                types[name] = t;
            }
        }

        /// <summary>
        /// Creates an instance of the given type and sets the properties
        /// </summary>
        /// <param name="typestr"></param>
        /// <param name="props"></param>
        /// <returns></returns>
        public IPlugin Create(string typestr, Dictionary<string, object> props = null)
        {
            if (types.TryGetValue(typestr, out var type))
            {
                var inst = type.Assembly.CreateInstance(type.FullName) as IPlugin;
                if (props != null)
                {
                    foreach (var kv in props)
                    {
                        var prop = inst[kv.Key];
                        if (prop != null)
                        {
                            prop.TrySetValue(kv.Value);
                        }
                    }
                }
                return inst;
            }
            return null;
        }
    }
}
