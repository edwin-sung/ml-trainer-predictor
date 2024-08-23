using MLTrainer.TrainingAlgorithms.CustomisableOption;

namespace MLTrainer.TrainingAlgorithms.LbfgsMaximumEntropyAlgorithm
{
    internal class L1RegularisationOption : TrainingAlgorithmOption<float>
    {
        public override string Name => "L1 Regularisation";

        internal L1RegularisationOption(float initialValue) => value = initialValue;

        public override bool TryGetValueAsString(out string valueAsString)
        {
            valueAsString = value.ToString("R");
            return true;
        }

        public override bool TrySetValue(string newValue)
        {
            if (float.TryParse(newValue, out float validResult))
            {
                value = validResult;
                return true;
            }

            return false;
        }
    }
}
