using Microsoft.ML;
using MLTrainer.TrainingAlgorithms;
using System.Collections.Generic;

namespace MLTrainer.Trainer
{
    /// <summary>
    /// ML.NET trainer, with model input and model output
    /// </summary>
    /// <typeparam name="ModelInput">Model input type</typeparam>
    /// <typeparam name="ModelOutput">Model output type</typeparam>
    internal abstract class ModelTrainer
    {
        protected IMLTrainingAlgorithm trainingAlgorithm;

        protected ModelTrainer(IMLTrainingAlgorithm trainingAlgorithm)
        {
            this.trainingAlgorithm = trainingAlgorithm;
        }

        #region Model training

        /// <summary>
        /// Trains a given model as a IDataView instance, and save it to the given trained model file path
        /// </summary>
        /// <param name="mlContextInstance">Machine learning context instance</param>
        /// <param name="estimatorChain">Estimator chain, which forms a build pipeline for training to take place</param>
        /// <param name="dataView">Data view instance</param>
        /// <param name="trainedModelFilePath">Trained model file path</param>
        /// <returns>True if the model training was successful</returns>
        protected bool TryTrainModel(MLContext mlContextInstance, IEstimator<ITransformer> estimatorChain, IDataView dataView, string trainedModelFilePath)
        {
            ITransformer trainedModel = estimatorChain?.Fit(dataView);

            if (trainedModel == null)
            {
                return false;
            }

            mlContextInstance.Model.Save(trainedModel, dataView.Schema, trainedModelFilePath);

            return true;
        }

        /// <summary>
        /// Chain of estimator chair actions to be called to end up with an ML-build pipeline instance used for training
        /// </summary>
        /// <param name="estimatorChainActions">Estimator chain actions</param>
        /// <returns>Build pipeline instance</returns>
        protected IEstimator<ITransformer> CreateEstimatorChain(IEnumerable<IEstimator<ITransformer>> estimatorChainActions)
        {
            IEstimator<ITransformer> pipeline = null;
            foreach(IEstimator<ITransformer> action in estimatorChainActions)
            {
                if (pipeline == null)
                {
                    pipeline = action;
                }
                else
                {
                    pipeline = pipeline.Append(action);
                }
            }
            return pipeline;
        }

        #endregion

    }
}
