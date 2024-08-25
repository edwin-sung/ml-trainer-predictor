using Microsoft.ML;
using System;

namespace MLTrainer.Predictor.ConcreteObjectPredictor
{

    /// <summary>
    /// ML.NET predictor for concrete object instances
    /// </summary>
    /// <typeparam name="ModelInput">Model input class as generic type</typeparam>
    /// <typeparam name="ModelOutput">Model output class as generic type</typeparam>
    public class ConcreteObjectModelPredictor<ModelInput, ModelOutput> where ModelInput : class where ModelOutput : class, new()
    {
        private readonly string trainedModelFilePath = string.Empty;
        private Lazy<PredictionEngine<ModelInput, ModelOutput>> PredictionEngine;

        /// <summary>
        /// Model predictor constructor
        /// </summary>
        /// <param name="trainedModelFilePath">Trained model file path</param>
        public ConcreteObjectModelPredictor(string trainedModelFilePath)
        {
            PredictionEngine = new Lazy<PredictionEngine<ModelInput, ModelOutput>>(CreatePredictionEngine);
            this.trainedModelFilePath = trainedModelFilePath;
        }

        private PredictionEngine<ModelInput, ModelOutput> CreatePredictionEngine()
        {
            MLContext mlContextInstance = new MLContext();
            ITransformer mlModel = mlContextInstance.Model.Load(trainedModelFilePath, out DataViewSchema _);
            return mlContextInstance.Model.CreatePredictionEngine<ModelInput, ModelOutput>(mlModel);
        }

        /// <summary>
        /// Gets the predicted output instance given the input
        /// </summary>
        /// <param name="input">Model input data</param>
        /// <param name="output">Model output data</param>
        /// <returns></returns>
        public bool TryGetPredictedOutput(ModelInput input, out ModelOutput output)
        {
            output = PredictionEngine.Value.Predict(input);
            return output != null;
        }
    }
}
