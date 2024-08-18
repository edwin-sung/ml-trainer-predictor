using MLTrainerPredictor;
using MLTrainerPredictor.DataSetup;

namespace MLTrainerPredictorTests.MicroGasTurbineElectricalEnergyPrediction
{
    [TestFixture]
    public class ElectricalEnergyTrainingAndPredicting
    {
        private IFunctionalitySpecificMLSetupItem setupItem;

        [SetUp]
        public void Initialise()
        {
            setupItem = new ElectricalEnergySetupItem();
            setupItem.AddDataInputsByCSVFilePath(".\\example.csv");
        }

        [TearDown]
        public void TestCleanup()
        {
            setupItem.CleanupTemporaryFiles();
        }

        [Test]
        public void Test_Training_And_Predicting()
        {
            bool trainedModelApplied = setupItem.ApplyTrainedModel();

            ElectricalEnergyPredictor predictor = new ElectricalEnergyPredictor();

            ElectricalInput input = new ElectricalInput { Time = 5213, InputVoltage = 3 };

            predictor.TryGetPredictedOutput(input, out ElectricalOutput output);

            float? predictedValue = output.Prediction;
            Assert.IsTrue(trainedModelApplied);
            Assert.IsNotNull(predictedValue);

        }
    }
}