using System.Reflection;

namespace MLTrainer.PredictionTesterUI.DataInputItemType
{
    /// <summary>
    /// Prediction tester data input string
    /// </summary>
    internal class PredictionTesterDataInputString : PredictionTesterDataInputItem<string>
    {
        internal PredictionTesterDataInputString(PropertyInfo propertyInfo, string originalValue) : base(propertyInfo, originalValue) 
        { 
        }

        /// <inheritdoc />
        public override string GetValueAsString() => value;

        /// <inheritdoc />
        public override bool TrySetValue(string newValue)
        {
            value = newValue;
            return true;
        }
    }
}
