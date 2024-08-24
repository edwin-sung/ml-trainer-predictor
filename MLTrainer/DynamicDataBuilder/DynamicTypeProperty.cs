using System;

namespace MLTrainer.DynamicDataBuilder
{
    /// <summary>
    /// A property name, and type used to generate a property in the dynamic class.
    /// </summary>
    public class DynamicTypeProperty
    {
        public DynamicTypeProperty(string name, Type type, string columnName = null, bool isLabel = false)
        {
            Name = name;
            Type = type;
            ColumnName = columnName ?? name;
            IsLabel = isLabel;
        }
        public string Name { get; set; }
        public Type Type { get; set; }

        public string ColumnName { get; set; }

        public bool IsLabel { get; set; }
    }
}
