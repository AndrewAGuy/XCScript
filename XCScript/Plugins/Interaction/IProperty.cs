namespace XCScript.Plugins.Interaction
{
    /// <summary>
    /// Interface for properties, to be collected and used by modifiable objects
    /// </summary>
    public interface IProperty
    {
        /// <summary>
        /// Name, as seen by the containing class
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Value of the parameter
        /// </summary>
        object Value { get; }

        /// <summary>
        /// If applicable, suggest to a user suitable values
        /// </summary>
        object[] SuggestedValues { get; }

        /// <summary>
        /// Attempt to set the value
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        Result TrySetValue(object o);

        /// <summary>
        /// About this property's usage
        /// </summary>
        string Description { get; }

        /// <summary>
        /// Should describe the behaviour of <see cref="TrySetValue(object)"/> as succinctly as possible
        /// </summary>
        string TypeDesc { get; }
    }
}
