using MLTrainer.DataSetup.DynamicObjectSetup;
using MLTrainer.PredictionTesterUI.DataInputItemType;
using MLTrainer.Predictor.ConcreteObjectPredictor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace MLTrainer.PredictionTesterUI.DynamicObjectPredictionTest
{
    internal class DynamicObjectPredictionTester
    {
        private Type dataInputSchemaType = null;
        internal List<IPredictionTesterDataInputItem> DataInputItems { get; } = new List<IPredictionTesterDataInputItem>();

        internal DynamicObjectPredictionTester(MLDataSchemaBuilder inputDataSchemaBuilder)
        {
            dataInputSchemaType = inputDataSchemaBuilder.SchemaType;
            SetupInputInterface(inputDataSchemaBuilder);
        }

        private void SetupInputInterface(MLDataSchemaBuilder inputDataSchemaBuilder)
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

            ForEachPropertyInfoWithColumnName(inputDataSchemaBuilder.SchemaType, AddInputInterfaces);
        }

        private void ForEachPropertyInfoWithColumnName(Type dynamicObjectType,
            Action<PropertyInfo, ColumnNameStorageAttribute> action)
        {
            foreach (var property in dynamicObjectType.GetProperties())
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
        internal bool RunPrediction(DynamicObjectModelPredictor predictorEngine, out string predictedValueAsString)
        {
            object input = Activator.CreateInstance(dataInputSchemaType);
            foreach (var property in dataInputSchemaType.GetProperties())
            {
                if (DataInputItems.SingleOrDefault(d => d.Name == property.Name) is IPredictionTesterDataInputItem validInputItem)
                {
                    validInputItem.SetValue(input);
                }
            }

            predictedValueAsString = string.Empty;

            if (!predictorEngine.TryGetPredictedOutput(input, out object output))
            {
                return false;
            }

            string predictedString = string.Empty;
            ForEachPropertyInfoWithColumnName(output.GetType(), (p, att) =>
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
