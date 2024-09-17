using System.ComponentModel;

namespace MLTrainer.TrainingAlgorithms.AutoSelection
{
    /// <summary>
    /// Optimisable objective that will determine the way the results are calculated
    /// TODO: This will eventually be replaced with a proper factory method, which does raw calculations
    /// </summary>
    internal enum OptimisableObjective
    {

        [Description("R-Squared")]
        RSquared = 0,

        [Description("Root-Mean-Squared Error")]
        RootMeanSquared = 1,

        [Description("Mean-Squared Error")]
        MeanSquaredError = 2

    }
}
