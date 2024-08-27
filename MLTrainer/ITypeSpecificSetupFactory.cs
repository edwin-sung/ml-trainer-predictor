using MLTrainer.DataSetup;
using MLTrainer.Trainer;
using MLTrainer.TrainingAlgorithms;

namespace MLTrainer
{
    /// <summary>
    /// Abstract factory for tyoe-specific (dynamic/runtime or object/compile-time) set-up for machine learning module
    /// </summary>
    internal interface ITypeSpecificSetupFactory
    {

        /// <summary>
        /// Get functionality-specific ML set-up factory instance
        /// </summary>
        /// <returns>Functionality-specific ML set-up factory instance</returns>
        IFunctionalitySpecificMLSetupFactory GetFunctionalitySpecificMLSetupFactory();

        /// <summary>
        /// Creates a model trainer instance, with a given training algorithm
        /// </summary>
        /// <param name="trainingAlgorithm">Training algorithm implementation</param>
        /// <returns>Model trainer instance</returns>
        ModelTrainer CreateModelTrainerInstance(IMLTrainingAlgorithm trainingAlgorithm);

    }
}
