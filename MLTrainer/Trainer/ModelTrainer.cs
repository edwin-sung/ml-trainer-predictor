using Microsoft.ML;
using MLTrainer.TrainingAlgorithms;
using System.Collections.Generic;
using System.Data;

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
        /// <param name="estimatorChain">Estimator chain, which forms a build pipeline for training to take place</param>
        /// <param name="dataView">Data view instance</param>
        /// <returns>ITransformer instance, if the training was successful</returns>
        protected ITransformer GetTrainedModel(IEstimator<ITransformer> estimatorChain, IDataView dataView)
        {
            return estimatorChain?.Fit(dataView);
        }

        /// <summary>
        /// Saves the trained model to a given file path
        /// </summary>
        /// <param name="mlContext">Machine learning context instance</param>
        /// <param name="schema">Data view schema for the trained model</param>
        /// <param name="trainedModelFilePath">Trained model file path</param>
        protected void SaveTrainedModel(MLContext mlContext, ITransformer trainedModel, DataViewSchema schema, string trainedModelFilePath)
        {
            mlContext.Model.Save(trainedModel, schema, trainedModelFilePath);
        }

        /// <summary>
        /// Splits the input data (IDataView) with a given ratio to training and testing sets
        /// </summary>
        /// <param name="mlContext">Machine learning context instance</param>
        /// <param name="testFraction">Ratio in which are for testing</param>
        /// <param name="data">Data</param>
        /// <param name="seed">Seed value to be fed to the splitting</param>
        /// <param name="trainSet">[Output] Train set</param>
        /// <param name="testSet">[Output] Test set</param>
        protected void SplitTrainingTestingData(MLContext mlContext, double testFraction, IDataView data, int? seed, 
            out IDataView trainSet, out IDataView testSet)
        {
            var split = mlContext.Data.TrainTestSplit(data, testFraction, seed: seed);
            trainSet = split.TrainSet;
            testSet = split.TestSet;
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
