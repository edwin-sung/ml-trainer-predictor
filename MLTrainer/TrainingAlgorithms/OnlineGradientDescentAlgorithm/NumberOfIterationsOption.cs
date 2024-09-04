using MLTrainer.TrainingAlgorithms.CustomisableOption;

namespace MLTrainer.TrainingAlgorithms.OnlineGradientDescentAlgorithm
{
    internal class NumberOfIterationsOption : TrainingAlgorithmOption<int>
    {
        public override string Name => "Number of Iterations";

        internal NumberOfIterationsOption(int initialValue) => value = initialValue;

        public override bool TryGetValueAsString(out string valueAsString)
        {
            valueAsString = value.ToString();
            return true;
        }

        public override bool TrySetValue(string newValue)
        {
            if (int.TryParse(newValue, out int validResult))
            {
                value = validResult;
                return true;
            }

            return false;
        }
    }
}
