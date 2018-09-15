namespace XCScript.Plugins.Interaction
{
    /// <summary>
    /// Base type for properties with shortcut access for better performance, without needing to validate property sets
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PropertyBase<T>
    {
        /// <summary>
        /// The value
        /// </summary>
        protected T value = default(T);

        /// <summary>
        /// Shortcut access for consuming classes
        /// </summary>
        public T Data
        {
            get
            {
                return value;
            }
        }

        /// <summary>
        /// Even shorter cut for getters
        /// </summary>
        /// <param name="op"></param>
        public static implicit operator T(PropertyBase<T> op)
        {
            return op.value;
        }

        /// <summary>
        /// General purpose access (scripting context)
        /// </summary>
        public object Value
        {
            get
            {
                return value;
            }
        }

        private string name = "Name";
        private string desc = "Desc";

        /// <summary>
        /// Constructs name and description
        /// </summary>
        /// <param name="n"></param>
        /// <param name="d"></param>
        public PropertyBase(string n, string d)
        {
            if (!string.IsNullOrWhiteSpace(n))
            {
                name = n;
            }
            if (!string.IsNullOrWhiteSpace(d))
            {
                desc = d;
            }
        }

        /// <summary>
        /// Fulfills <see cref="IProperty.Name"/>
        /// </summary>
        public string Name
        {
            get
            {
                return name;
            }
        }

        /// <summary>
        /// Fulfills <see cref="IProperty.Description"/>
        /// </summary>
        public string Description
        {
            get
            {
                return desc;
            }
        }

        /// <summary>
        /// Fulfills <see cref="IProperty.SuggestedValues"/>
        /// </summary>
        public object[] SuggestedValues
        {
            get
            {
                return new object[] { };
            }
        }
    }
}
