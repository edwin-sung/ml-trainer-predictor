namespace MLTrainer.DataSetup.DynamicObjectSetup
{
    /// <summary>
    /// Abstract class for functionality-specific machine learning set-up item, which does not have concrete objects defined
    /// and thus needs Data Schema Builder to help with the creation of objects for training and prediction
    /// </summary>
    public abstract class DynamicObjectMLSetupItem : FunctionalitySpecificMLSetupItem
    {

        protected DynamicObjectMLSetupItem(string functionalityName) : base(functionalityName)
        {
        }
    }
}
