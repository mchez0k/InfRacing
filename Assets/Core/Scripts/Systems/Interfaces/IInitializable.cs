namespace Core.Systems
{
    /// <summary>
    /// Interface for objects that require initialization before use.
    /// </summary>
    public interface IInitializable
    {
        /// <summary>
        /// Gets the initialization state of the object.
        /// </summary>
        public bool IsInitialized { get; }

        /// <summary>
        /// Method to do some logic before using class
        /// </summary>
        public void Initialize();
    }
}