using Microsoft.ML;
using System;
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
