using MLTrainer.TrainingAlgorithms.CustomisableOption;

namespace MLTrainer.TrainingAlgorithms.OnlineGradientDescentAlgorithm
{
    internal class LearningRateOption : TrainingAlgorithmOption<float>
    {
        public override string Name => "Learning Rate";

        internal LearningRateOption(float initialValue) => value = initialValue;

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
