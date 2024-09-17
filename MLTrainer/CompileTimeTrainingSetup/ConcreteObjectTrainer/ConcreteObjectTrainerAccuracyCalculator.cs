using Microsoft.ML;
using Microsoft.ML.Data;
using MLTrainer.CompileTimeTrainingSetup.ConcreteObjectPredictor;
using MLTrainer.Trainer;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace MLTrainer.CompileTimeTrainingSetup.ConcreteObjectTrainer
{
    /// <summary>
    /// Concrete object training accuracy result instance, which gets the test data and the trained model to
    /// compare predicted results and actual results
    /// <typeparamref name="ModelInput">Model input generic class</typeparamref>
    /// <typeparamref name="ModelOutput">Model output generic class</typeparamref>
    /// </summary>
    internal class ConcreteObjectTrainerAccuracyCalculator<ModelInput, ModelOutput> : TrainerAccuracyCalculator
        where ModelInput : class, new()
        where ModelOutput : class, new()
    {
        private readonly MLContext mlContext;
        private readonly IEnumerable<ModelInput> testSet;
        private readonly IDataView transformedTestSet;
        private readonly ConcreteObjectModelPredictor<ModelInput, ModelOutput> testSetPredictor;
        

        internal ConcreteObjectTrainerAccuracyCalculator(MLContext mlContext, IEnumerable<ModelInput> testSet, IDataView transformedTestSet, string trainedModelFilePath) : base(mlContext)
        {
            this.mlContext = mlContext;
            this.testSet = testSet;
            this.transformedTestSet = transformedTestSet;
            testSetPredictor = new ConcreteObjectModelPredictor<ModelInput, ModelOutput>(trainedModelFilePath);

            if (!TryGetLabelInfo<ModelInput>(out PropertyInfo _, out ColumnNameStorageAttribute inputAtt) ||
                !TryGetLabelInfo<ModelOutput>(out PropertyInfo _, out ColumnNameStorageAttribute outputAtt))
            {
                return;
            }


            // Extract model metrics and get RSquared
            ITransformer trainedModelTransformer = mlContext.Model.Load(trainedModelFilePath, out DataViewSchema _);
            /*RegressionMetrics trainedModelMetrics = mlContext.Regression.Evaluate(trainedModelTransformer.Transform(transformedTestSet), inputAtt.Name, outputAtt.Name);*/
            //double rSquared = trainedModelMetrics.RSquared;

            double rSquaredComparison = GetAccuracy() ?? 0;

            //double difference = Math.Abs(rSquared - rSquaredComparison);
        }


        internal override double? GetAccuracy()
        {
            // R-squared is calculated as 1 - sumSquaredRegression/sumOfSquares, where
            // sumSquaredRegression = sum (actual - predicted)^2
            // sumOfSquares = sum(actual - mean)^2
            if (testSetPredictor.TryGetMultiplePredictions(testSet, out IEnumerable<ModelOutput> outputs) &&
                testSet.Count() == outputs.Count())
            {
                // Go through each set and check their accuracies
                List<ModelInput> modelInputs = testSet.ToList();
                List<ModelOutput> modelOutputs = outputs.ToList();
                List<double> actualValues = new List<double>();
                List<double> residualSquared = new List<double>();

                for(int i = 0; i < modelInputs.Count; i++)
                {
                    GetActualAndPredictedValues(modelInputs[i], modelOutputs[i], out double? actual, out double? predicted);
                    if (!actual.HasValue || !predicted.HasValue)
                    {
                        continue;
                    }

                    actualValues.Add(actual.Value);
                    residualSquared.Add(Math.Pow(actual.Value - predicted.Value, 2));
                }

                double sumSquaredRegression = residualSquared.Sum();
                double mean = actualValues.Sum() / actualValues.Count;
                double sumOfSquares = actualValues.Select(value => Math.Pow(value - mean, 2)).Sum();

                return 1 - sumSquaredRegression / sumOfSquares;
                
            }
            return 0;
        }

        private void GetActualAndPredictedValues(ModelInput input, ModelOutput output, out double? actual, out double? predicted)
        {
            actual = null;
            predicted = null;
            if (!TryGetLabelInfo<ModelInput>(out PropertyInfo inputInfo, out ColumnNameStorageAttribute _) ||
                !TryGetLabelInfo<ModelOutput>(out PropertyInfo outputInfo, out ColumnNameStorageAttribute _) ||
                inputInfo.PropertyType != outputInfo.PropertyType)
            {
                return;
            }

            if (double.TryParse(inputInfo.GetValue(input).ToString(), out double validInput))
            {
                actual = validInput;
            }
            if (double.TryParse(outputInfo.GetValue(output).ToString(), out double validOutput))
            {
                predicted = validOutput;
            }
        }

        private bool TryGetLabelInfo<T>(out PropertyInfo labelPropertyInfo, out ColumnNameStorageAttribute labelAttribute)
        {
            labelPropertyInfo = null;
            labelAttribute = null;
            foreach (PropertyInfo property in typeof(T).GetProperties())
            {
                try
                {
                    labelAttribute = property.GetCustomAttribute<ColumnNameStorageAttribute>();
                    if (!string.IsNullOrEmpty(labelAttribute.Name) && labelAttribute.IsLabel)
                    {
                        labelPropertyInfo = property;
                        return true;
                    }
                }
                catch
                {
                    continue;
                }
            }

            return false;
        }

    }
}
