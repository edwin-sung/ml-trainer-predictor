using Microsoft.ML;
using System;

namespace MLTrainer
{
    /// <summary>
    /// ML.NET predictor, with model input and model output
    /// </summary>
    /// <typeparam name="ModelInput">Model input type</typeparam>
    /// <typeparam name="ModelOutput">Model output type</typeparam>
    public class ModelPredictor<ModelInput, ModelOutput> where ModelInput : class where ModelOutput : class, new()
    {
        private readonly string trainedModelFilePath = string.Empty;

        private Lazy<PredictionEngine<ModelInput, ModelOutput>> PredictionEngine;

        /// <summary>
        /// Model predictor constructor
        /// </summary>
        /// <param name="trainedModelFilePath">Trained model file path</param>
        public ModelPredictor(string trainedModelFilePath)
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
            PredictionEngine.Value.Dispose();
            return output != null;
        }
    }
}
