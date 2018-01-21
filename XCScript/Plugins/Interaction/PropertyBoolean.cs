using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XCScript.Plugins.Interaction
{
    /// <summary>
    /// Boolean values
    /// </summary>
    public class PropertyBoolean : PropertyBase<bool>, IProperty
    {
        /// <summary>
        /// Constructs with initial state
        /// </summary>
        /// <param name="n"></param>
        /// <param name="d"></param>
        /// <param name="v"></param>
        public PropertyBoolean(string n, string d, bool v) : base(n, d)
        {
            value = v;
        }

        /// <summary>
        /// Direct access
        /// </summary>
        public new bool Data
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
        public new object[] SuggestedValues
        {
            get
            {
                return new object[] { true, false };
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string TypeDesc
        {
            get
            {
                return "Boolean";
            }
        }

        /// <summary>
        /// For numeric types, condition is C-style (non-zero)
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public Result TrySetValue(object o)
        {
            switch (o)
            {
                case bool b:
                    value = b;
                    break;
                case int i:
                    value = i != 0;
                    break;
                case double d:
                    value = d != 0;
                    break;
                default:
                    var s = o.ToString();
                    if (bool.TryParse(s, out var v))
                    {
                        value = v;
                        break;
                    }
                    return new Result(false, "Cannot make boolean from: " + s);
            }
            return new Result();
        }
    }
}
