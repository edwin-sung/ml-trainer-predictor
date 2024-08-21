using MLTrainerPredictor.DataSetup;
using MLTrainerPredictorTests.MicroGasTurbineElectricalEnergyPrediction;

namespace MLTrainerPredictor.Demo
{
    internal class DemoMLSetupFactory : IFunctionalitySpecificMLSetupFactory
    {
        public IEnumerable<IFunctionalitySpecificMLSetupItem> GetAvailableSetupItems()
        {
            yield return new ElectricalEnergySetupItem();
        }
    }
}
