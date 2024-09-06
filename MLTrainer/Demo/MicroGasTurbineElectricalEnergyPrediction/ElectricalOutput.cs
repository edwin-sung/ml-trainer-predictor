﻿using Microsoft.ML.Data;
using MLTrainer;

namespace MLTrainerTests.MicroGasTurbineElectricalEnergyPrediction
{
    public class ElectricalOutput
    {

        /// <summary>
        /// Predicted label
        /// </summary>
        [ColumnName("PredictedLabel")]
        [ColumnNameStorage("PredictedLabel", typeof(float), true)]
        public float Prediction { get; set; }

        /*/// <summary>
        /// Score
        /// </summary>
        public float[] Score { get; set; }*/

    }
}
