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
            this.Messages = new List<string> { s };
        }

        /// <summary>
        /// Whether the operation was able to complete
        /// </summary>
        public bool Success { get; set; } = true;

        /// <summary>
        /// Info about the execution
        /// </summary>
        public List<string> Messages { get; set; } = new List<string>();

        /// <summary>
        /// Appends the messages and determines success. If fatal, failure is conserved, else success is conserved
        /// </summary>
        /// <param name="other"></param>
        /// <param name="fatal"></param>
        /// <returns></returns>
        public Result Append(Result other, bool fatal = false)
        {
            this.Messages.AddRange(other.Messages);
            this.Success = fatal ? this.Success && other.Success : this.Success || other.Success;
            return this;
        }
    }
}
