using System.Reflection;

namespace MLTrainer.PredictionTesterUI.DataInputItemType
{
    /// <summary>
    /// Prediction tester data input string
    /// </summary>
    internal class PredictionTesterDataInputFloat : PredictionTesterDataInputItem<float>
    {
        internal PredictionTesterDataInputFloat(PropertyInfo propertyInfo, float originalValue) : base(propertyInfo, originalValue) 
        { 
        }

        /// <inheritdoc />
        public override string GetValueAsString() => value.ToString("R");

        /// <inheritdoc />
        public override bool TrySetValue(string newValue) => float.TryParse(newValue, out value);
    }
}
