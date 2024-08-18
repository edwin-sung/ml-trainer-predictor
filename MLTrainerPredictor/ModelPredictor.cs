using Microsoft.ML;

namespace MLTrainerPredictor
{
    /// <summary>
    /// ML.NET predictor, with model input and model output
    /// </summary>
    /// <typeparam name="ModelInput">Model input type</typeparam>
    /// <typeparam name="ModelOutput">Model output type</typeparam>
    public abstract class ModelPredictor<ModelInput, ModelOutput> where ModelInput : class where ModelOutput : class, new()
    {
        /// <summary>
        /// Trained model file path
        /// </summary>
        protected abstract string TrainedModelFilePath { get; }

        private Lazy<PredictionEngine<ModelInput, ModelOutput>> PredictionEngine;

        /// <summary>
        /// Model predictor constructor
        /// </summary>
        protected ModelPredictor()
        {
            PredictionEngine = new Lazy<PredictionEngine<ModelInput, ModelOutput>>(CreatePredictionEngine);
        }

        private PredictionEngine<ModelInput, ModelOutput> CreatePredictionEngine()
        {
            MLContext mlContextInstance = new MLContext();
            ITransformer mlModel = mlContextInstance.Model.Load(TrainedModelFilePath, out DataViewSchema _);
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
