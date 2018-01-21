namespace XCScript.Plugins.Interaction
{
    /// <summary>
    /// 
    /// </summary>
    public class PropertyDouble : PropertyBase<double>, IProperty
    {
        private bool positive;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="n"></param>
        /// <param name="d"></param>
        /// <param name="v"></param>
        /// <param name="p">Constrain to be positive</param>
        public PropertyDouble(string n, string d, double v, bool p = false) : base(n, d)
        {
            positive = p;
            if (!Set(v).Success)
            {
                value = 0;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string TypeDesc
        {
            get
            {
                return "Double";
            }
        }

        private Result Set(double d)
        {
            if (positive && d < 0)
            {
                return new Result(false, "Value must be positive");
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
