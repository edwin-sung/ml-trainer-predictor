using Microsoft.ML;
using Microsoft.ML.Data;
using MLTrainer.RuntimeTrainingSetup.DynamicObjectSetup;
using MLTrainer.Trainer;
using MLTrainer.TrainingAlgorithms;
using System;
using System.Linq;
using System.Reflection;

namespace MLTrainer.RuntimeTrainingSetup.DynamicObjectTrainer
{

    /// <summary>
    /// ML.NET trainer for dynamic objects
    /// </summary>
    internal class DynamicObjectModelTrainer : ModelTrainer
    {
        internal DynamicObjectModelTrainer(Type inputType, Type outputType)
        {
        }

        internal bool TryTrainModel(IMLTrainingAlgorithm trainingAlgorithm, MLDataSchemaBuilder inputDataBuilder, MLDataSchemaBuilder outputDataBuilder, string trainedModelFilePath, out TrainerAccuracyCalculator accuracyResult, double dataSplitTestPercentage = 0.2, int? seed = null)
        {
            accuracyResult = null;

            // Create schema definition from schema type
            SchemaDefinition schema = SchemaDefinition.Create(inputDataBuilder.SchemaType);

            // Use MLContext to create train data.
            MLContext mlContext = new MLContext();
            Type dataType = mlContext.Data.GetType();
            if (dataType.GetMethods().FirstOrDefault(m => m.Name == "LoadFromEnumerable" && m.IsGenericMethod)
                is MethodInfo loadMethodGeneric)
            {
                MethodInfo loadMethod = loadMethodGeneric.MakeGenericMethod(inputDataBuilder.SchemaType);
                if (!(loadMethod.Invoke(mlContext.Data, new[] { inputDataBuilder.GetInputData(), schema }) is IDataView dataView))
                {
                    return false;
                }


                SplitTrainingTestingData(mlContext, dataSplitTestPercentage, dataView, seed, out IDataView trainSet, out IDataView testSet);

                IEstimator<ITransformer> predictorPipeline = trainingAlgorithm.BuildTrainingAlgorithmPipeline(
                    mlContext, inputDataBuilder.GetSchemaProperties(), outputDataBuilder.GetSchemaProperties());

                if (!(GetTrainedModel(predictorPipeline, trainSet) is ITransformer trainedModel))
                {
                    return false;
                }

                SaveTrainedModel(mlContext, trainedModel, trainSet.Schema, trainedModelFilePath);

                // Training model was successful, make use of the test set to determine the accuracy of the trained model.
                if (!inputDataBuilder.TryCloneSchemaWithNewData(mlContext, testSet,
                    out MLDataSchemaBuilder newTestSchemaBuilder))
                {
                    return false;
                }

                accuracyResult = new DynamicObjectTrainingAccuracyResult(mlContext, newTestSchemaBuilder, outputDataBuilder, trainedModelFilePath);
                return accuracyResult != null;
            }

            return false;
        }
    }
}
