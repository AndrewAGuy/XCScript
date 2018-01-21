namespace XCScript.Plugins.Interaction
{
    /// <summary>
    /// Double, clamped between two values
    /// </summary>
    public class PropertyRangedDouble : PropertyBase<double>, IProperty
    {
        private double max;
        private double min;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="n"></param>
        /// <param name="d"></param>
        /// <param name="v"></param>
        /// <param name="lo"></param>
        /// <param name="hi"></param>
        public PropertyRangedDouble(string n, string d, double v, double lo, double hi) : base(n, d)
        {
            max = hi;
            min = lo;
            value = v > max ? max : v < min ? min : v;
        }

        /// <summary>
        /// 
        /// </summary>
        public string TypeDesc
        {
            get
            {
                return $"Clamped double: [{min},{max}]";
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public new object[] SuggestedValues
        {
            get
            {
                return new object[] { min, max };
            }
        }

        private Result Set(double d)
        {
            if (d < min || d > max)
            {
                return new Result(false, "Value outside of range");
            }
            value = d;
            return new Result();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public Result TrySetValue(object o)
        {
            switch (o)
            {
                case double d:
                    return Set(d);
                case int i:
                    return Set(i);
                default:
                    var s = o.ToString();
                    return double.TryParse(s, out var v) ? Set(v) : new Result(false, "Can not make double from: " + s);
            }
        }
    }
}
