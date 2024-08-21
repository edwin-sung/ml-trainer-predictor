namespace MLTrainer.PredictionTesterUI
{
    /// <summary>
    /// Interface for prediction tester data input item
    /// </summary>
    public interface IPredictionTesterDataInputItem
    {
        /// <summary>
        /// Name for the prediction tester data input item
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the value as string
        /// </summary>
        /// <returns></returns>
        string GetValueAsString();

        /// <summary>
        /// Attempts to set the new value for the current data input item
        /// </summary>
        /// <param name="newValue">New value as string</param>
        /// <returns>True if a new value can be set, false otherwise.</returns>
        bool TrySetValue(string newValue);

        /// <summary>
        /// Sets the stored value to the object instance, which is part of the data input
        /// </summary>
        /// <param name="obj">Object instance</param>
        void SetValue(object obj);

    }
}
