namespace XCScript.Plugins.Interaction
{
    /// <summary>
    /// 
    /// </summary>
    public class IntegerProperty : PropertyBase<int>, IProperty
    {
        bool positive;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="n"></param>
        /// <param name="d"></param>
        /// <param name="v"></param>
        /// <param name="p">Constrain to positive</param>
        public IntegerProperty(string n, string d, int v, bool p = false) : base(n, d)
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
                return "Integer";
            }
        }

        private Result Set(int i)
        {
            if (positive && i < 0)
            {
                return new Result(false, "Value must be positive");
            }
            value = i;
            return new Result();
        }

        /// <summary>
        /// For doubles, performs default conversion
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public Result TrySetValue(object o)
        {
            switch (o)
            {
                case double d:
                    return Set((int)d);
                case int i:
                    return Set(i);
                default:
                    var s = o.ToString();
                    return int.TryParse(s, out var v) ? Set(v) : new Result(false, "Can not make double from: " + s);
            }
        }
    }
}
