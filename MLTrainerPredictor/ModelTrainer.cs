using Microsoft.ML;
using MLTrainerPredictor.TrainingAlgorithms;
using System.Reflection;

namespace MLTrainerPredictor
{
    /// <summary>
    /// ML.NET trainer, with model input and model output
    /// </summary>
    /// <typeparam name="ModelInput">Model input type</typeparam>
    /// <typeparam name="ModelOutput">Model output type</typeparam>
    internal class ModelTrainer<ModelInput, ModelOutput> where ModelInput : class where ModelOutput : class, new()
    {
        private IMLTrainingAlgorithm trainingAlgorithm;

        internal ModelTrainer(IMLTrainingAlgorithm trainingAlgorithm)
        {
            this.trainingAlgorithm = trainingAlgorithm;
        }

        #region Model training
        
        /// <summary>
        /// Gets all column names for a given generic type, based on the predicate for label
        /// </summary>
        /// <typeparam name="T">Generic type</typeparam>
        /// <param name="attributePredicate">Predicate on whether one wants the labels, non-labels, or both</param>
        /// <param name="columnNames">[Output] Column names</param>
        /// <returns>True if any items are found</returns>
        private bool TryGetColumnNamesFor<T>(Predicate<ColumnNameStorageAttribute> attributePredicate, out List<string> columnNames)
        {
            columnNames = new List<string>();
            foreach (var property in typeof(T).GetProperties())
            {
                try
                {
                    ColumnNameStorageAttribute att = property.GetCustomAttribute<ColumnNameStorageAttribute>();
                    if (!string.IsNullOrEmpty(att.Name) && attributePredicate(att))
                    {
                        columnNames.Add(att.Name);
                    }
                }
                catch
                {
                    continue;
                }
            }

            // If we are looking for label, make sure there is only one column, otherwise simply check whether there are any.
            return columnNames.Any();
        }
        /// <summary>
        /// Train the model
        /// </summary>
        internal bool TryTrainModel(IEnumerable<ModelInput> inputs, string trainedModelFilePath)
        {
            MLContext mlContextInstance = new MLContext();
            IDataView trainData = mlContextInstance.Data.LoadFromEnumerable(inputs);

            ITransformer trainedModel = RetrainPipeline(mlContextInstance, trainData);
            if (trainedModel == null)
            {
                return false;
            }

            mlContextInstance.Model.Save(trainedModel, trainData.Schema, trainedModelFilePath);

            return true;

        }

        private ITransformer RetrainPipeline(MLContext context, IDataView trainData)
        {
            var pipeline = BuildPipeline(context);
            var model = pipeline?.Fit(trainData);

            return model;
        }

        /// <summary>
        /// build the pipeline that is used from model builder. Use this function to retrain model.
        /// </summary>
        /// <param name="mlContext"></param>
        /// <returns></returns>
        private IEstimator<ITransformer> BuildPipeline(MLContext mlContext)
        {
            // Make sure we have one or more non-label inputs, only one label input, and only one label output
            if (!TryGetColumnNamesFor<ModelInput>(att => !att.IsLabel, out List<string> nonLabelInputs) ||
                !TryGetColumnNamesFor<ModelInput>(att => att.IsLabel, out List<string> labelledInputs) ||
                !(labelledInputs.SingleOrDefault() is string labelledInput) ||
                !TryGetColumnNamesFor<ModelOutput>(att => att.IsLabel, out List<string> labelledOutputs) ||
                !(labelledOutputs.SingleOrDefault() is string labelledOutput))
            {
                return null;
            }

            string[] features = nonLabelInputs.Select(c => @c).ToArray();

            IEstimator<ITransformer>? pipeline = null;

            void AppendAction(IEstimator<ITransformer> estimator)
            {
                if (pipeline == null)
                {
                    pipeline = estimator;
                }
                else
                {
                    pipeline = pipeline.Append(estimator);
                }
            }
            
            List<InputOutputColumnPair> hotEncodingColumnPairs = new List<InputOutputColumnPair>();
            if (TryGetColumnNamesFor<ModelInput>(att => att.ColumnType == typeof(string) && !att.IsLabel, out List<string> stringColumns))
            {
                stringColumns.ForEach(c => hotEncodingColumnPairs.Add(new InputOutputColumnPair(@c, @c)));
                AppendAction(mlContext.Transforms.Categorical.OneHotEncoding(hotEncodingColumnPairs.ToArray()));
            }

            List<InputOutputColumnPair> missingValuesColumnPairs = new List<InputOutputColumnPair>();
            if (TryGetColumnNamesFor<ModelInput>(att => att.ColumnType == typeof(float) && !att.IsLabel, out List<string> floatColumns))
            {
                floatColumns.ForEach(c => hotEncodingColumnPairs.Add(new InputOutputColumnPair(@c, @c)));

                AppendAction(mlContext.Transforms.ReplaceMissingValues(missingValuesColumnPairs.ToArray()));
            }

            AppendAction(mlContext.Transforms.Concatenate(@"Features", features));
            AppendAction(mlContext.Transforms.Conversion.MapValueToKey(@labelledInput, @labelledInput));
            AppendAction(mlContext.Transforms.NormalizeMinMax(@"Features", @"Features"));
            AppendAction(trainingAlgorithm.GetTrainingAlgorithm(mlContext, labelledInput, @"Features"));
            AppendAction(mlContext.Transforms.Conversion.MapKeyToValue(@labelledOutput, @labelledOutput));
            return pipeline;
        }

        #endregion

    }
}
