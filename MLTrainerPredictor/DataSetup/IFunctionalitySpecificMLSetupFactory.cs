namespace MLTrainerPredictor.DataSetup
{
    /// <summary>
    /// Functionality-specific machine learning setup factory instance
    /// This allows training and prediction for different functionality
    /// </summary>
    public interface IFunctionalitySpecificMLSetupFactory
    {

        /// <summary>
        /// Gets all the available set-up items to be configured
        /// </summary>
        /// <returns>All available setup items</returns>
        IEnumerable<IFunctionalitySpecificMLSetupItem> GetAvailableSetupItems();

    }
}
