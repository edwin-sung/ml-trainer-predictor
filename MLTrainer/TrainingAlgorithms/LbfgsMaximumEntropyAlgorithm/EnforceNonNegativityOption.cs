using MLTrainer.TrainingAlgorithms.CustomisableOption;

namespace MLTrainer.TrainingAlgorithms.LbfgsMaximumEntropyAlgorithm
{
    internal class EnforceNonNegativityOption : TrainingAlgorithmOption<bool>
    {
        public override string Name => "Optimisation tolerance";

        internal EnforceNonNegativityOption(bool initialValue) => value = initialValue;

        public override bool TryGetValueAsString(out string valueAsString)
        {
            valueAsString = value.ToString();
            return true;
        }

        public override bool TrySetValue(string newValue)
        {
            if (bool.TryParse(newValue, out bool validResult))
            {
                value = validResult;
                return true;
            }

            return false;
        }
    }
}
