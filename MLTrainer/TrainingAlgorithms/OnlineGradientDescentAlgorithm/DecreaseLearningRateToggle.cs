using MLTrainer.TrainingAlgorithms.CustomisableOption;

namespace MLTrainer.TrainingAlgorithms.OnlineGradientDescentAlgorithm
{
    internal class DecreaseLearningRateToggle : TrainingAlgorithmOption<bool>
    {
        public override string Name => "Decrease learning rate";

        internal DecreaseLearningRateToggle(bool initialValue) => value = initialValue;

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
