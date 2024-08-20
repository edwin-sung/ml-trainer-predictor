namespace MLTrainerPredictor.TrainingAlgorithms.CustomisableOption
{
    public interface ITrainingAlgorithmOption
    {

        string Name { get; }

        bool TryGetValueAsString(out string valueAsString);

        bool TrySetValue(string newValue);

    }
}
