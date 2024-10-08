﻿using MLTrainer.CompileTimeTrainingSetup.DataSetup;
using MLTrainer.TrainingAlgorithms;

namespace MLTrainerTests.MicroGasTurbineElectricalEnergyPrediction
{
    internal class ElectricalEnergySetupItem : ConcreteObjectMLSetupItem<ElectricalTestInput, ElectricalTestOutput>
    {
        public ElectricalEnergySetupItem() : base("ElectricalEnergyTrainingModel")
        {

        }

        public override string Name => "Electrical Energy Setup";

        public override string TrainingModelDirectory { get; set; } = "C:\\Temp";

        public override string TrainingModelName { get; set; } = string.Empty;

        protected override bool TryConvertToCSVString(ElectricalTestInput input, out string csvRow)
        {
            csvRow = input.Time.ToString() + SEPARATOR +
                   input.InputVoltage.ToString() + SEPARATOR +
                   input.ElectricalPower.ToString() + SEPARATOR;
            return !string.IsNullOrEmpty(csvRow);
        }

        protected override bool TryParse(string csvRow, out ElectricalTestInput validModelInput)
        {
            validModelInput = new ElectricalTestInput();
            string[] items = csvRow.Split(new[] { SEPARATOR }, StringSplitOptions.None);
            if (items.Length != 3)
            {
                return false;
            }

            try
            {

                if (!float.TryParse(items[0], out float validTime))
                {
                    return false;
                }
                validModelInput.Time = validTime;

                if (!float.TryParse(items[1], out float validInputVoltage))
                {
                    return false;
                }
                validModelInput.InputVoltage = validInputVoltage;

                if (!float.TryParse(items[2], out float validElectricalPower))
                {
                    return false;
                }
                validModelInput.ElectricalPower = validElectricalPower;

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
