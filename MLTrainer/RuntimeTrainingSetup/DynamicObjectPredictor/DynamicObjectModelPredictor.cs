using Microsoft.ML;
using Microsoft.ML.Data;
using MLTrainer.RuntimeTrainingSetup.DynamicObjectSetup;
using System;
using System.Linq;
using System.Reflection;

namespace MLTrainer.RuntimeTrainingSetup.DynamicObjectPredictor
{
    /// <summary>
    /// ML.NET predictor
    /// </summary>
    internal class DynamicObjectModelPredictor
    {
        private readonly string trainedModelFilePath = string.Empty;
        private readonly Type inputType, outputType;

        /// <summary>
        /// Model predictor constructor
        /// </summary>
        /// <param name="trainedModelFilePath">Trained model file path</param>
        /// <param name="inputType">Model input data</param>
        /// <param name="outputType">Model output data</param>
        internal DynamicObjectModelPredictor(string trainedModelFilePath, Type inputType, Type outputType)
        {
            this.trainedModelFilePath = trainedModelFilePath;
            this.inputType = inputType;
            this.outputType = outputType;
        }

        /// <summary>
        /// Gets the predicted output instances
        /// </summary>
        /// <param name="inputObjectInstances">Collection of data input, as object instance</param>
        /// <param name="outputSchema">Output schema to referenced when constructing</param>
        /// <param name="outputObjectInstances">[Output] Collection of data output, as object instance</param>
        /// <returns>True if multiple predictions could be played</returns>
        internal bool TryGetMultiplePredictions(MLDataSchemaBuilder inputObjectInstances, 
            MLDataSchemaBuilder outputSchema, out MLDataSchemaBuilder outputInstances)
        {
            outputInstances = null;

            // Create schema definition from schema type
            SchemaDefinition schema = SchemaDefinition.Create(inputObjectInstances.SchemaType);

            // Use MLContext to create train data.
            MLContext mlContext = new MLContext();
            Type dataType = mlContext.Data.GetType();
            if (!(dataType.GetMethods().FirstOrDefault(m => m.Name == "LoadFromEnumerable" && m.IsGenericMethod) 
                is MethodInfo loadMethodGeneric))
            {
                return false;
            }

            MethodInfo loadMethod = loadMethodGeneric.MakeGenericMethod(inputObjectInstances.SchemaType);
            if (!(loadMethod.Invoke(mlContext.Data, new[] { inputObjectInstances.GetInputData(), schema })
                is IDataView transformedInput))
            {
                return false;
            }

            // Load trained model
            ITransformer predictionPipeline = mlContext.Model.Load(trainedModelFilePath, out DataViewSchema otherSchema);

            IDataView predictions = predictionPipeline.Transform(transformedInput);

            return outputSchema.TryCloneSchemaWithNewData(mlContext, predictions, out outputInstances);
        }

        /// <summary>
        /// Gets the predicted output instance given the input
        /// </summary>
        /// <returns></returns>
        public bool TryGetPredictedOutput(object inputObjectInstance, out object predictedResult)
        {
            predictedResult = null;
            // We wish to ensure that the input schema data input matches the object instance. Otherwise return false

            if (inputObjectInstance.GetType() != inputType)
            {
                return false;
            }

            MLContext mlContext = new MLContext();
            ITransformer mlModel = mlContext.Model.Load(trainedModelFilePath, out DataViewSchema dataViewSchema);

            // Create prediction engine dynamically for prediction to work
            MethodInfo genericPredictionMethod =
                mlContext.Model.GetType().GetMethod("CreatePredictionEngine", new[] { typeof(ITransformer), typeof(DataViewSchema) });
            MethodInfo predictionMethod = genericPredictionMethod.MakeGenericMethod(inputType, outputType);
            dynamic dynamicPredictionEngine = predictionMethod.Invoke(mlContext.Model, new object[] { mlModel, dataViewSchema });

            // Now use the dynamic prediction to predict the result.
            MethodInfo predictMethod = dynamicPredictionEngine.GetType().GetMethod("Predict", new[] { inputType });
            predictedResult = predictMethod.Invoke(dynamicPredictionEngine, new[] { inputObjectInstance });

            // Ensure that the output matches the type of the output schema
            return predictedResult != null && predictedResult.GetType() == outputType;
        }
    }
}
