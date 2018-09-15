using System.Collections.Generic;
using System.Linq;

namespace XCScript.Plugins.Interaction
{
    /// <summary>
    /// Supports changing value modes based on a key and a pre-defined map
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    public class ModeProperty<TKey, TValue> : PropertyBase<TValue>, IProperty
    {
        private IDictionary<TKey, TValue> modes;

        /// <summary>
        /// Access to modes dictionary
        /// </summary>
        public IDictionary<TKey, TValue> Modes
        {
            get
            {
                return modes;
            }
        }

        /// <summary>
        /// Constructor that takes map and default value, which need not be in the map
        /// </summary>
        /// <param name="n"></param>
        /// <param name="d"></param>
        /// <param name="m"></param>
        /// <param name="v"></param>
        public ModeProperty(string n, string d, IDictionary<TKey, TValue> m, TValue v = default(TValue)) : base(n, d)
        {
            modes = m;
            value = v;
        }

        /// <summary>
        /// Constructor that takes map and default key
        /// </summary>
        /// <param name="n"></param>
        /// <param name="d"></param>
        /// <param name="m"></param>
        /// <param name="k"></param>
        public ModeProperty(string n, string d, IDictionary<TKey, TValue> m, TKey k) : base(n, d)
        {
            modes = m;
            modes.TryGetValue(k, out value);
        }

        /// <summary>
        /// Hides base member, allows set
        /// </summary>
        public new TValue Data
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
                return $"Mode: {typeof(TKey)} -> {typeof(TValue)}";
            }
        }

        /// <summary>
        /// Returns the keys, which may need to be boxed
        /// </summary>
        public new object[] SuggestedValues
        {
            get
            {
                return modes.Keys.Cast<object>().ToArray();
            }
        }

        /// <summary>
        /// Only acts if passed object is of type <typeparamref name="TKey"/> or <typeparamref name="TValue"/>.
        /// <para/>
        /// In cases where the key is a <see cref="string"/>, you may want to create a more specialised implementation 
        /// that attempts to use <see cref="object.ToString()"/>
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public Result TrySetValue(object o)
        {
            switch (o)
            {
                case TValue v:
                    value = v;
                    break;
                case TKey k:
                    if (!modes.TryGetValue(k, out var val))
                    {
                        return new Result(false, $"No mode present with key '{k}'");
                    }
                    value = val;
                    break;
                default:
                    return new Result(false, $"Type provided ({o.GetType()}) is not of key ({typeof(TKey)}) or value ({typeof(TValue)})");
            }
            return new Result();
        }
    }
}
