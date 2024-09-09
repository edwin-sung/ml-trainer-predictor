using System.ComponentModel;

namespace MLTrainer.TrainingAlgorithms
{
    public enum MLTrainingAlgorithmType
    {
        [Description("One Versus All")]
        ONE_VERSUS_ALL = 0,

        [Description("LBFGS Maximum Entropy")]
        LBFGS_MAX_ENTROPY = 1,

        [Description("Online Gradient Descent Regression")]
        ONLINE_GRADIENT_DESCENT = 2,

        [Description("LBFGS Poisson Regression")]
        LBFGS_POISSON_REGRESSION = 3
    }
}
