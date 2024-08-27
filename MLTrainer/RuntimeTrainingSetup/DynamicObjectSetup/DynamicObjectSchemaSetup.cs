using System;
using System.Collections.Generic;
using System.Linq;

namespace MLTrainer.RuntimeTrainingSetup.DynamicObjectSetup
{
    /// <summary>
    /// Dynamic object schema setup instance, that allows any extensions to be read and configured properly
    /// </summary>
    internal abstract class DynamicObjectSchemaSetup
    {
        private readonly List<DynamicObjectSchemaInfo> schemaInfos = new List<DynamicObjectSchemaInfo>();

        protected List<Type> acceptedObjectTypes = new List<Type> { typeof(float), typeof(int), typeof(string) };

        private class DynamicObjectSchemaInfo
        {
            internal string Name { get; set; }

            internal bool IsLabel { get; set; } = false;

            internal Type ObjectType { get; set; }

        }

        internal bool IsValidImport { get; }

        internal DynamicObjectSchemaSetup(string inputFile)
        {
            IsValidImport = TryParse(inputFile);
        }

        /// <summary>
        /// Parses the input string to schemas
        /// </summary>
        /// <param name="inputFile"></param>
        /// <returns></returns>
        protected abstract bool TryParse(string inputFile);

        /// <summary>
        /// Adds a new property of a generic type
        /// </summary>
        /// <typeparam name="T">Generic type that can be readable by ML.NET</typeparam>
        /// <param name="name">Name</param>
        /// <param name="isLabel">True if this selected as a label</param>
        protected void AddPropertyOfType<T>(string name, bool isLabel = false)
        {
            schemaInfos.Add(new DynamicObjectSchemaInfo { Name = name, IsLabel = isLabel, ObjectType = typeof(T) });
        }

        /// <summary>
        /// Changes the name of a given index to the new name 
        /// </summary>
        /// <param name="index">Index</param>
        /// <param name="name">Name to be assigned to that index</param>
        internal void ChangeNameOfIndex(int index, string name)
        {
            // Only change the name if it is not empty, and that there are no existing schemas with the same name 
            if (!string.IsNullOrEmpty(name) && !schemaInfos.Any(s => s.Name == name))
            {
                schemaInfos[index].Name = name;
            }
        }

        /// <summary>
        /// Sets the label of the a property collection to true, with a given index
        /// </summary>
        /// <param name="index">Index</param>
        internal void SetIndexAsLabel(int index)
        {
            schemaInfos.ForEach(s => s.IsLabel = false);
            schemaInfos[index].IsLabel = true;
        }

        /// <summary>
        /// Changes the object type for a given index
        /// </summary>
        /// <typeparam name="T">Generic type</typeparam>
        /// <param name="index">Index</param>
        internal void ChangeObjectTypeForIndex<T>(int index)
        {
            if (acceptedObjectTypes.Contains(typeof(T)))
            {
                schemaInfos[index].ObjectType = typeof(T);
            }
        }

    }
}
