namespace XCScript.Plugins.Interaction
{
    /// <summary>
    /// Template for generic gettable and settable properties
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ObjectProperty<T> : PropertyBase<T>, IProperty
    {
        private bool immutable;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="n"></param>
        /// <param name="d"></param>
        /// <param name="v"></param>
        /// <param name="i">Immutable</param>
        public ObjectProperty(string n, string d, T v, bool i = false) : base(n, d)
        {
            value = v;
            immutable = i;
        }

        /// <summary>
        /// Shortcut for consumers
        /// </summary>
        public new T Data
        {
            get
            {
                return value;
            }
            set
            {
                if (!immutable)
                {
                    this.value = value;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string TypeDesc
        {
            get
            {
                return (immutable ? "Immutable " : "") + typeof(T).FullName;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public Result TrySetValue(object o)
        {
            if (immutable)
            {
                return new Result(false, "Cannot set immutable property");
            }
            if (o is T t)
            {
                value = t;
                return new Result();
            }
            return new Result(false, $"Cannot set {typeof(T)} from {o.GetType()}");
        }
    }
}
