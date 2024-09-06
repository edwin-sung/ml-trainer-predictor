using MLTrainer.TrainingAlgorithms;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.ML;
using System.Linq;
using MLTrainer.Trainer;

namespace MLTrainer.CompileTimeTrainingSetup.ConcreteObjectTrainer
{
    /// <summary>
    /// ML.NET trainer for concrete objects, with model input and model output
    /// </summary>
    /// <typeparam name="ModelInput">Model input type</typeparam>
    /// <typeparam name="ModelOutput">Model output type</typeparam>
    internal class ConcreteObjectModelTrainer<ModelInput, ModelOutput> : ModelTrainer
        where ModelInput : class, new()
        where ModelOutput : class, new()
    {


        internal ConcreteObjectModelTrainer(IMLTrainingAlgorithm trainingAlgorithm) : base(trainingAlgorithm)
        {
        }

        private IEnumerable<ColumnNameStorageAttribute> GetColumnNameAttributesFor<T>()
        {
            return typeof(T).GetProperties().Select(
                prop => prop.GetCustomAttribute<ColumnNameStorageAttribute>()).Where(att => att != null);
        }

        /// <summary>
        /// Train the model
        /// </summary>
        internal bool TryTrainModel(IEnumerable<ModelInput> inputs, string trainedModelFilePath, out double? rSquared, double dataSplitTestPercentage = 0.2, int? seed = null)
        {
            rSquared = null;
            MLContext mlContextInstance = new MLContext();
            IDataView trainData = mlContextInstance.Data.LoadFromEnumerable(inputs);

            SplitTrainingTestingData(mlContextInstance, dataSplitTestPercentage, trainData, 
                seed, out IDataView trainSet, out IDataView testSet);

            IEstimator<ITransformer> predictionModelPipeline =
                trainingAlgorithm.BuildTrainingAlgorithmPipeline(mlContextInstance, 
                GetColumnNameAttributesFor<ModelInput>(), GetColumnNameAttributesFor<ModelOutput>());

            if (!(GetTrainedModel(predictionModelPipeline, trainSet) is ITransformer trainedModel))
            {
                return false;
            }

            SaveTrainedModel(mlContextInstance, trainedModel, trainSet.Schema, trainedModelFilePath);

            // Training model was successful, make use of the test set to determine the accuracy of the trained model
            ConcreteObjectTrainingAccuracyResult<ModelInput, ModelOutput> accuracyResult =
                new ConcreteObjectTrainingAccuracyResult<ModelInput, ModelOutput>(mlContextInstance,
                    mlContextInstance.Data.CreateEnumerable<ModelInput>(testSet, false), testSet, trainedModelFilePath);

            rSquared = accuracyResult.CalculateAccuracy();

            return true;
        }

    }
}
