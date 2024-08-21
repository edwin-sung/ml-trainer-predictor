using MLTrainer;
using MLTrainer.DataSetup;

namespace MLTrainerTests.MicroGasTurbineElectricalEnergyPrediction
{
    [TestFixture]
    public class ElectricalEnergyTrainingAndPredicting
    {
        private IFunctionalitySpecificMLSetupItem setupItem;

        [SetUp]
        public void Initialise()
        {
            setupItem = new ElectricalEnergySetupItem();
            setupItem.AddDataInputsByCSVFilePath(Environment.CurrentDirectory + "\\MicroGasTurbineElectricalEnergyPrediction\\example.csv");
        }

        [TearDown]
        public void TestCleanup()
        {
            setupItem.CleanupTemporaryFiles();
        }

        [Test]
        public void Test_Training_And_Predicting()
        {
            bool tryCreateTrainedModel = setupItem.TryCreateTrainedModelForTesting(out string filePath);

            ModelPredictor<ElectricalInput, ElectricalOutput> predictor = new ModelPredictor<ElectricalInput, ElectricalOutput>(filePath);

            ElectricalInput input = new ElectricalInput { Time = 5213, InputVoltage = 3 };

            predictor.TryGetPredictedOutput(input, out ElectricalOutput output);

            float? predictedValue = output.Prediction;

            
            Assert.IsTrue(tryCreateTrainedModel);
            Assert.IsNotNull(predictedValue);

        }
    }
}