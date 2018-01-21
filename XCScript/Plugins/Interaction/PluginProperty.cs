namespace XCScript.Plugins.Interaction
{
    /// <summary>
    /// For properties that take IPlugin inheriting types
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PluginProperty<T> : PropertyBase<T>, IProperty where T : IPlugin
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="n"></param>
        /// <param name="d"></param>
        /// <param name="v"></param>
        public PluginProperty(string n, string d, T v) : base(n, d)
        {
            value = v;
        }

        /// <summary>
        /// Gettable and settable by calling objects
        /// </summary>
        public new T Data
        {
            get
            {
                return value;
            }
            set
            {
                this.value = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string TypeDesc
        {
            get
            {
                return typeof(T).FullName;
            }
        }

        /// <summary>
        /// Tries to cast to current type
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public Result TrySetValue(object o)
        {
            if (o is T t)
            {
                value = t;
                return new Result();
            }
            return new Result(false, $"Can't convert {o.GetType().FullName} to {this.TypeDesc}");
        }
    }
}
