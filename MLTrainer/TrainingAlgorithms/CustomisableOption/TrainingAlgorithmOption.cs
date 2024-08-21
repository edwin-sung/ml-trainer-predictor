namespace MLTrainer.TrainingAlgorithms.CustomisableOption
{
    public abstract class TrainingAlgorithmOption<T> : ITrainingAlgorithmOption
    {
        protected T value;

        public abstract string Name { get; }

        public abstract bool TryGetValueAsString(out string valueAsString);

        public abstract bool TrySetValue(string newValue);

        internal T Value => value;
    }
}
