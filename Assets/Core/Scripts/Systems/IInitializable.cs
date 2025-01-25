namespace Core.Systems
{
    public interface IInitializable
    {
        /// <summary>
        /// Field signal about Init
        /// </summary>
        public bool IsInitialized { get; }

        /// <summary>
        /// Method to do some logic before using class
        /// </summary>
        public void Initialize();
    }

}