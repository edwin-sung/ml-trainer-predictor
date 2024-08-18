using MLTrainerPredictor;
using MLTrainerPredictor.DataSetup;

namespace MLTrainerPredictorTests.MicroGasTurbineElectricalEnergyPrediction
{
    [TestClass]
    public class ElectricalEnergyTrainingAndPredicting
    {
        private IFunctionalitySpecificMLSetupItem setupItem;

        [TestInitialize]
        public void Initialise()
        {
            setupItem = new ElectricalEnergySetupItem();
            setupItem.AddDataInputsByCSVFilePath(".\\example.csv");
        }

        [TestCleanup]
        public void TestCleanup()
        {
            setupItem.CleanupTemporaryFiles();
        }

        [TestMethod]
        public void Test_Training_And_Predicting()
        {
            setupItem.ApplyTrainedModel();

            ElectricalEnergyPredictor predictor = new ElectricalEnergyPredictor();

            ElectricalInput input = new ElectricalInput { Time = 5213, InputVoltage = 3 };

            predictor.TryGetPredictedOutput(input, out ElectricalOutput output);

            float? predictedValue = output.Prediction;
            Assert.IsNotNull(predictedValue);

        }
    }
}