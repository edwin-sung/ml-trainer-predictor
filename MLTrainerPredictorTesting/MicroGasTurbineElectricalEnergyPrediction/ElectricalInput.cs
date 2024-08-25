using Microsoft.ML.Data;
using MLTrainer;

namespace MLTrainerTests.MicroGasTurbineElectricalEnergyPrediction
{
    public class ElectricalTestInput
    {

        [ColumnName("time")]
        [ColumnNameStorage("time", typeof(float))]
        public float Time { get; set; }

        [ColumnName("input_voltage")]
        [ColumnNameStorage("input_voltage", typeof(float))]
        public float InputVoltage { get; set; }

        [ColumnName("Label")]
        [ColumnNameStorage("Label", typeof(float), true)]
        public float ElectricalPower { get; set; }

    }
}
