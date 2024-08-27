using MLTrainer.CompileTimeTrainingSetup.ConcreteObjectPredictor;
using MLTrainer.PredictionTesterUI;
using MLTrainer.PredictionTesterUI.DataInputItemType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace MLTrainer.CompileTimeTrainingSetup.ConcreteObjectPredictionTest
{
    internal class ConcreteObjectPredictionTester<ModelInput, ModelOutput> where ModelInput : class where ModelOutput : class, new()
    {

        internal List<IPredictionTesterDataInputItem> DataInputItems { get; } = new List<IPredictionTesterDataInputItem>();

        internal ConcreteObjectPredictionTester()
        {
            SetupInputInterface();
        }

        private void SetupInputInterface()
        {
            void AddInputInterfaces(PropertyInfo propertyInfo, ColumnNameStorageAttribute columnNameAtt)
            {
                if (columnNameAtt.IsLabel)
                {
                    return;
                }

                if (ConstructTesterDataInputItem(propertyInfo) is IPredictionTesterDataInputItem validItem)
                {
                    DataInputItems.Add(validItem);
                }

            }

            ForEachPropertyInfoWithColumnName<ModelInput>(AddInputInterfaces);
        }

        private void ForEachPropertyInfoWithColumnName<T>(
            Action<PropertyInfo, ColumnNameStorageAttribute> action)
        {
            foreach (var property in typeof(T).GetProperties())
            {
                try
                {
                    ColumnNameStorageAttribute att = property.GetCustomAttribute<ColumnNameStorageAttribute>();
                    if (att != null)
                    {
                        action?.Invoke(property, att);
                    }
                }
                catch
                {
                }
            }
        }

        private IPredictionTesterDataInputItem ConstructTesterDataInputItem(PropertyInfo propertyInfo, object data = null)
        {

            if (propertyInfo.PropertyType == typeof(string))
            {
                string defaultString = data != null ? propertyInfo.GetValue(data) as string : string.Empty;
                return new PredictionTesterDataInputString(propertyInfo, defaultString);
            }
            else if (propertyInfo.PropertyType == typeof(float))
            {
                float defaultFloat = data != null ? (float)propertyInfo.GetValue(data) : 0f;
                return new PredictionTesterDataInputFloat(propertyInfo, defaultFloat);
            }

            return null;
        }

        /// <summary>
        /// Construct a ModelInput instance based on the input, and run the prediction engine for the result
        /// </summary>
        internal bool RunPrediction(ConcreteObjectModelPredictor<ModelInput, ModelOutput> predictorEngine, out string predictedValueAsString)
        {
            ModelInput input = Activator.CreateInstance<ModelInput>();
            foreach (var property in typeof(ModelInput).GetProperties())
            {
                if (DataInputItems.SingleOrDefault(d => d.Name == property.Name) is IPredictionTesterDataInputItem validInputItem)
                {
                    validInputItem.SetValue(input);
                }
            }

            predictedValueAsString = string.Empty;

            if (!predictorEngine.TryGetPredictedOutput(input, out ModelOutput output))
            {
                return false;
            }

            string predictedString = string.Empty;
            ForEachPropertyInfoWithColumnName<ModelOutput>((p, att) =>
            {
                if (!att.IsLabel)
                {
                    return;
                }

                IPredictionTesterDataInputItem item = ConstructTesterDataInputItem(p, output);
                predictedString = item.GetValueAsString();
            });

            predictedValueAsString = predictedString;

            return true;

        }
    }
}
