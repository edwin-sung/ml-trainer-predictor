
using MLTrainer.CompileTimeTrainingSetup.DataSetup;
using System;

namespace MLTrainer.Demo.IrisPrediction
{
    internal class IrisClassificationSetupItem : ConcreteObjectMLSetupItem<IrisInput, IrisClassificationOutput>
    {
        public IrisClassificationSetupItem() : base("IrisClassificationTrainingModel")
        {
        }


        public override string Name => "Iris Classification Setup";

        public override string TrainingModelDirectory { get; set; } = "C:\\Temp";
        public override string TrainingModelName { get; set; } = string.Empty;

        protected override bool TryConvertToCSVString(IrisInput input, out string csvRow)
        {
            csvRow = input.SepalLength.ToString() + SEPARATOR +
                   input.SepalWidth.ToString() + SEPARATOR +
                   input.PetalLength.ToString() + SEPARATOR + 
                   input.PetalWidth.ToString() + SEPARATOR + 
                   input.Variety + SEPARATOR;
            return !string.IsNullOrEmpty(csvRow);
        }

        protected override bool TryParse(string csvRow, out IrisInput validModelInput)
        {
            validModelInput = new IrisInput();
            string[] items = csvRow.Split(new[] { SEPARATOR }, StringSplitOptions.None);
            if (items.Length != 5)
            {
                return false;
            }

            try
            {

                if (!float.TryParse(items[0], out float validSepalLength))
                {
                    return false;
                }
                validModelInput.SepalLength = validSepalLength;

                if (!float.TryParse(items[1], out float validSepalWidth))
                {
                    return false;
                }
                validModelInput.SepalWidth = validSepalWidth;

                if (!float.TryParse(items[2], out float validPetalLength))
                {
                    return false;
                }
                validModelInput.PetalLength = validPetalLength;

                if (!float.TryParse(items[3], out float validPetalWidth))
                {
                    return false;
                }
                validModelInput.PetalWidth = validPetalWidth;

                if (string.IsNullOrEmpty(items[4]))
                {
                    return false;
                }
                validModelInput.Variety = items[4];

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
