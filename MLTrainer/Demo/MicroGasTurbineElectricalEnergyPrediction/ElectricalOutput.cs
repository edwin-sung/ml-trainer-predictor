using Microsoft.ML.Data;
using MLTrainer;

namespace MLTrainerTests.MicroGasTurbineElectricalEnergyPrediction
{
    public class ElectricalOutput
    {
        [ColumnName(@"Score")]
        [ColumnNameStorage(@"Score", typeof(float), true)]
        public float Score { get; set; }
    }
}
