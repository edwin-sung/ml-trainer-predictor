using MLTrainer.DataSetup;
using MLTrainer.Predictor;
using MLTrainer.Predictor.ConcreteObjectPredictor;

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

            ConcreteObjectModelPredictor<ElectricalTestInput, ElectricalTestOutput> predictor = new ConcreteObjectModelPredictor<ElectricalTestInput, ElectricalTestOutput>(filePath);

            ElectricalTestInput input = new ElectricalTestInput { Time = 5213, InputVoltage = 3 };

            predictor.TryGetPredictedOutput(input, out ElectricalTestOutput output);

            float? predictedValue = output.Prediction;

            
            Assert.IsTrue(tryCreateTrainedModel);
            Assert.IsNotNull(predictedValue);

        }
    }
}