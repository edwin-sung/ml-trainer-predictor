using Microsoft.ML;
using Microsoft.ML.Data;
using MLTrainer.DataSetup.DynamicObjectSetup;
using MLTrainer.TrainingAlgorithms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace MLTrainer.Trainer.DynamicObjectTrainer
{

    /// <summary>
    /// ML.NET trainer for dynamic objects
    /// </summary>
    internal class DynamicObjectModelTrainer : ModelTrainer
    {
        internal DynamicObjectModelTrainer(Type inputType, Type outputType, IMLTrainingAlgorithm trainingAlgorithm) : base(trainingAlgorithm)
        {
        }

        internal bool TryTrainModel(MLDataSchemaBuilder inputDataBuilder, MLDataSchemaBuilder outputDataBuilder, string trainedModelFilePath)
        {
            // Create schema definition from schema type
            SchemaDefinition schema = SchemaDefinition.Create(inputDataBuilder.SchemaType);

            // Use MLContext to create train data.
            MLContext mlContext = new MLContext();
            Type dataType = mlContext.Data.GetType();
            if (dataType.GetMethods().FirstOrDefault(m => m.Name == "LoadFromEnumerable" && m.IsGenericMethod)
                is MethodInfo loadMethodGeneric)
            {
                MethodInfo loadMethod = loadMethodGeneric.MakeGenericMethod(inputDataBuilder.SchemaType);
                return TryTrainModel(mlContext, BuildPipeline(mlContext, inputDataBuilder, outputDataBuilder), loadMethod.Invoke(mlContext.Data, new [] { inputDataBuilder.GetInputData(), schema }) as IDataView, trainedModelFilePath);
            }

            return false;

        }

        private IEstimator<ITransformer> BuildPipeline(MLContext mlContext, MLDataSchemaBuilder inputDataSchemaBuilder, 
            MLDataSchemaBuilder outputDataSchemaBuilder)
        {
            // Make sure we have one or more non-label inputs, only one label input, and only one label output
            if (!inputDataSchemaBuilder.TryGetColumnNames(att => !att.IsLabel, out List<string> nonLabelInputs) ||
                !inputDataSchemaBuilder.TryGetColumnNames(att => att.IsLabel, out List<string> labelledInputs) ||
                !(labelledInputs.SingleOrDefault() is string labelledInput) ||
                !outputDataSchemaBuilder.TryGetColumnNames(att => att.IsLabel, out List<string> labelledOutputs) ||
                !(labelledOutputs.SingleOrDefault() is string labelledOutput))
            {
                return null;
            }

            string[] features = nonLabelInputs.Select(c => @c).ToArray();
            List<IEstimator<ITransformer>> estimatorChainActions = new List<IEstimator<ITransformer>>();

            List<InputOutputColumnPair> hotEncodingColumnPairs = new List<InputOutputColumnPair>();
            if (inputDataSchemaBuilder.TryGetColumnNames(att => att.ColumnType == typeof(string) && !att.IsLabel, out List<string> stringColumns))
            {
                stringColumns.ForEach(c => hotEncodingColumnPairs.Add(new InputOutputColumnPair(@c, @c)));
                estimatorChainActions.Add(mlContext.Transforms.Categorical.OneHotEncoding(hotEncodingColumnPairs.ToArray()));
            }

            List<InputOutputColumnPair> missingValuesColumnPairs = new List<InputOutputColumnPair>();
            if (inputDataSchemaBuilder.TryGetColumnNames(att => att.ColumnType == typeof(float) && !att.IsLabel, out List<string> floatColumns))
            {
                floatColumns.ForEach(c => hotEncodingColumnPairs.Add(new InputOutputColumnPair(@c, @c)));

                estimatorChainActions.Add(mlContext.Transforms.ReplaceMissingValues(missingValuesColumnPairs.ToArray()));
            }

            estimatorChainActions.Add(mlContext.Transforms.Concatenate(@"Features", features));
            estimatorChainActions.Add(mlContext.Transforms.Conversion.MapValueToKey(@labelledInput, @labelledInput));
            estimatorChainActions.Add(mlContext.Transforms.NormalizeMinMax(@"Features", @"Features"));
            estimatorChainActions.Add(trainingAlgorithm.GetTrainingAlgorithm(mlContext, labelledInput, @"Features"));
            estimatorChainActions.Add(mlContext.Transforms.Conversion.MapKeyToValue(@labelledOutput, @labelledOutput));
            return CreateEstimatorChain(estimatorChainActions);
        }
    }
}
