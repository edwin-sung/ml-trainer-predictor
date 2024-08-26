using System;

namespace MLTrainer
{
    /// <summary>
    /// Column name storage attribute, which allows string value to be returned
    /// </summary>
    public class ColumnNameStorageAttribute : Attribute
    {

        /// <summary>
        /// Name of the column
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets the field type of the current column
        /// </summary>
        public Type ColumnType { get; set; }

        /// <summary>
        /// Whether or not this particular column is a label (used for prediction)
        /// </summary>
        public bool IsLabel { get; set; }

        /// <inheritdoc />
        public ColumnNameStorageAttribute(string columnName, Type columnType, bool label = false)
        {
            Name = columnName;
            ColumnType = columnType;
            IsLabel = label;
        }

    }
}
