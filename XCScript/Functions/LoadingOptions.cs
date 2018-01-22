namespace XCScript.Functions
{
    /// <summary>
    /// Passed to <see cref="Engine"/> for loading default state
    /// </summary>
    public class LoadingOptions
    {
        /// <summary>
        /// Control flow functions: {if,do,while,until,throw}
        /// </summary>
        public bool Control { get; set; } = true;

        /// <summary>
        /// Accessors and manipulators of global state: {map,index,del,err}
        /// </summary>
        public bool Access { get; set; } = true;

        /// <summary>
        /// Interpretation and execution in global state: {interp,exec}
        /// </summary>
        public bool Interpret { get; set; } = true;

        /// <summary>
        /// Plugin interaction: {new,prop,alias}
        /// </summary>
        public bool Plugins { get; set; } = true;
    }
}
