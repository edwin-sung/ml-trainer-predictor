using MLTrainer.TrainingAlgorithms.CustomisableOption;

namespace MLTrainer.TrainingAlgorithms.LbfgsMaximumEntropyAlgorithm
{
    internal class OptimisationTolOption : TrainingAlgorithmOption<float>
    {
        public override string Name => "Optimisation tolerance";

        internal OptimisationTolOption(float initialValue) => value = initialValue;

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
