using Microsoft.ML;
using MLTrainer.RuntimeTrainingSetup.DynamicObjectPredictor;

namespace MLTrainer.RuntimeTrainingSetup.DynamicObjectSetup
{
    /// <summary>
    /// Dynamic object training accuracy result instance, which gets the test data and the trained model to 
    /// compare predicted results and actual results
    /// </summary>
    internal class DynamicObjectTrainingAccuracyResult
    {

        private MLContext mlContext;
        private MLDataSchemaBuilder inputDataSchemaBuilder;
        private MLDataSchemaBuilder outputDataSchemaBuilder;
        private DynamicObjectModelPredictor testSetPredictor;


        internal DynamicObjectTrainingAccuracyResult(MLContext context, MLDataSchemaBuilder inputDataSchemaBuilder,
            MLDataSchemaBuilder outputDataSchemaBuilder, string trainedModelFilePath)
        {
            mlContext = context;
            this.inputDataSchemaBuilder = inputDataSchemaBuilder;
            this.outputDataSchemaBuilder = outputDataSchemaBuilder;

            testSetPredictor = new DynamicObjectModelPredictor(trainedModelFilePath,
                inputDataSchemaBuilder.GetType(), outputDataSchemaBuilder.GetType());
        }

        internal double CalculateAccuracy()
        {
            if (testSetPredictor.TryGetMultiplePredictions(inputDataSchemaBuilder, outputDataSchemaBuilder,
                out MLDataSchemaBuilder predictedOutputs))
            {
                return inputDataSchemaBuilder.GetRSquared(predictedOutputs) ?? 0;
            }

            return 0;
        }

    }
}
