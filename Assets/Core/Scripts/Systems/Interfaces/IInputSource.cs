namespace Core.Systems
{
    public interface IInputSource
    {
        /// <summary>
        /// Returns acceleration (from -1 to 1)
        /// </summary>
        public float GetAcceleration();

        /// <summary>
        /// Returns breaking state
        /// </summary>
        public bool GetBrake();

        /// <summary>
        /// Returns the turn (from -1 to 1)
        /// </summary>
        public float GetSteering();

        /// <summary>
        /// Update all inputs
        /// </summary>
        public void UpdateInput();
    }
}