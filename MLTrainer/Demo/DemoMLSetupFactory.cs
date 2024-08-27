using MLTrainer.DataSetup;
using MLTrainer.RuntimeTrainingSetup.DynamicObjectSetup;
using MLTrainerTests.MicroGasTurbineElectricalEnergyPrediction;
using System.Collections.Generic;

namespace MLTrainer.Demo
{
    internal class DemoMLSetupFactory : IFunctionalitySpecificMLSetupFactory
    {
        public IEnumerable<IFunctionalitySpecificMLSetupItem> GetAvailableSetupItems()
        {
            yield return new ElectricalEnergySetupItem();
            yield return new JsonObjectMLSetupItem();
        }
    }
}
