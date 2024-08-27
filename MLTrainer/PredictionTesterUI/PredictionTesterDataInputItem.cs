using System.Reflection;

namespace MLTrainer.PredictionTesterUI
{
    /// <summary>
    /// Singular prediction tester data input item
    /// </summary>
    internal abstract class PredictionTesterDataInputItem<T> : IPredictionTesterDataInputItem
    {
        protected T value;
        private PropertyInfo propertyInfo;

        protected PredictionTesterDataInputItem(PropertyInfo propertyInfo, T originalValue)
        {
            this.propertyInfo = propertyInfo;
            Name = propertyInfo.Name;
            value = originalValue;
        }

        /// <inheritdoc />
        public string Name { get; }

        /// <inheritdoc />
        public abstract string GetValueAsString();

        /// <inheritdoc />
        public abstract bool TrySetValue(string newValue);

        /// <inheritdoc />
        public void SetValue(object obj)
        {
            propertyInfo.SetValue(obj, value);
        }
    }
}
