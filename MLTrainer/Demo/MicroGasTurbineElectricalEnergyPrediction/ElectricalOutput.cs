using Microsoft.ML.Data;
using MLTrainer;

namespace MLTrainerTests.MicroGasTurbineElectricalEnergyPrediction
{
    public class ElectricalOutputForClassification
    {
        [ColumnName(@"PredictedLabel")]
        [ColumnNameStorage(@"PredictedLabel", typeof(float), true)]
        public float PredictedLabel { get; set; }
    }

    public class ElectricalOutputForRegression
    {
        [ColumnName(@"Score")]
        [ColumnNameStorage(@"Score", typeof(float), true)]
        public float Score { get; set; }
    }
}
