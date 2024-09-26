using Microsoft.ML.Data;

namespace MLTrainer.Demo.IrisPrediction
{
    public class IrisClassificationOutput
    {
        [ColumnName(@"PredictedLabel")]
        [ColumnNameStorage(@"PredictedLabel", typeof(string), true)]
        public string PredictedVariety { get; set; }
    }
}
