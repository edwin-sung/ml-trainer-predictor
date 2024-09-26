using Microsoft.ML.Data;

namespace MLTrainer.Demo.IrisPrediction
{
    public class IrisInput
    {

        [ColumnName("sepal.length")]
        [ColumnNameStorage("sepal.length", typeof(float))]
        public float SepalLength { get; set; }

        [ColumnName("sepal.width")]
        [ColumnNameStorage("sepal.width", typeof(float))]
        public float SepalWidth { get; set; }

        [ColumnName("petal.length")]
        [ColumnNameStorage("petal.length", typeof(float))]
        public float PetalLength { get; set; }

        [ColumnName("petal.width")]
        [ColumnNameStorage("petal.width", typeof(float))]
        public float PetalWidth { get; set; }

        [ColumnName("variety")]
        [ColumnNameStorage("variety", typeof(string), true)]
        public string Variety { get; set; }


    }
}
