using System.Collections.Generic;
using System.Linq;

namespace XCScript.Plugins.Interaction
{
    /// <summary>
    /// 
    /// </summary>
    public class StringProperty : PropertyBase<string>, IProperty
    {
        private SortedSet<string> permitted = null;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="n">Name</param>
        /// <param name="d">Description</param>
        /// <param name="v">Value</param>
        /// <param name="p">Permitted values</param>
        public StringProperty(string n, string d, string v, IEnumerable<string> p = null) : base(n, d)
        {
            if (p != null)
            {
                permitted = new SortedSet<string>(p);
            }
            if (!TrySetValue(v).Success)
            {
                v = permitted.FirstOrDefault();
            }
        }

        /// <summary>
        /// If enumeration, return values
        /// </summary>
        public new object[] SuggestedValues
        {
            get
            {
                return permitted != null ? permitted.ToArray() : new object[] { };
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string TypeDesc
        {
            get
            {
                return "String" + (permitted != null ? "Enum" : "");
            }
        }

        /// <summary>
        /// If enumeration, ensures that value is in permitted set
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public Result TrySetValue(object o)
        {
            if (o == null)
            {
                return new Result(false, "Value is null");
            }
            var str = o.ToString();
            if (permitted != null)
            {
                if (!permitted.Contains(str))
                {
                    return new Result(false, "Value is not permitted: " + str);
                }
            }
            value = str;
            return new Result();
        }
    }
}
