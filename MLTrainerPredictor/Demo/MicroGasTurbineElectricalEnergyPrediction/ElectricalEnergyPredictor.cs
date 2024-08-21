using MLTrainerPredictor;

namespace MLTrainerPredictorTests.MicroGasTurbineElectricalEnergyPrediction
{
    internal class ElectricalEnergyPredictor : ModelPredictor<ElectricalInput, ElectricalOutput>
    {
        protected override string TrainedModelFilePath => new ElectricalEnergySetupItem().GetTrainedModelFilePath();
    }
}
