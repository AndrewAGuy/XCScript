using System.IO;

namespace XCScript.Plugins.Interaction
{
    /// <summary>
    /// Paths to files and directories
    /// </summary>
    public class PathProperty : PropertyBase<string>, IProperty
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="n"></param>
        /// <param name="d"></param>
        /// <param name="v"></param>
        public PathProperty(string n, string d, string v) : base(n, d)
        {
            TrySetValue(v);
        }

        /// <summary>
        /// 
        /// </summary>
        public string TypeDesc
        {
            get
            {
                return "Path";
            }
        }

        /// <summary>
        /// Checks that string is valid path, but not that file exists
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public Result TrySetValue(object o)
        {
            var s = o?.ToString();
            if (string.IsNullOrWhiteSpace(s) || s.IndexOfAny(Path.GetInvalidPathChars()) != -1)
            {
                return new Result(false, "String is not valid path: " + (s ?? ""));
            }
            value = s;
            return new Result();
        }
    }
}
