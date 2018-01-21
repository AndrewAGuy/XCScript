using System.Collections.Generic;

namespace XCScript
{
    /// <summary>
    /// Return value for methods that affect state, but may need to report back
    /// </summary>
    public class Result
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public Result()
        {

        }

        /// <summary>
        /// Constructor for only one message
        /// </summary>
        /// <param name="b"></param>
        /// <param name="s"></param>
        public Result(bool b, string s)
        {
            this.Success = b;
            this.Messages = new string[] { s };
        }

        /// <summary>
        /// Whether the operation was able to complete
        /// </summary>
        public bool Success { get; set; } = true;

        /// <summary>
        /// Info about the execution
        /// </summary>
        public string[] Messages { get; set; } = null;
    }
}
