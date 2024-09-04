using MLTrainer.TrainingAlgorithms.CustomisableOption;

namespace MLTrainer.TrainingAlgorithms.OnlineGradientDescentAlgorithm
{
    internal class L2RegularisationOption : TrainingAlgorithmOption<float>
    {
        public override string Name => "L2 Regularisation";

        internal L2RegularisationOption(float initialValue) => value = initialValue;

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
