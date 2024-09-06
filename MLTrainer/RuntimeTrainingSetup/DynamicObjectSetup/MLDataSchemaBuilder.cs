using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Globalization;
using Microsoft.ML;
using Microsoft.ML.Data;
using System.Data;
using System.Runtime.Remoting.Messaging;

namespace MLTrainer.RuntimeTrainingSetup.DynamicObjectSetup
{
    /// <summary>
    /// Ambitious machine-learning data schema builder, which will be used for data input and output
    /// </summary>
    internal class MLDataSchemaBuilder
    {
        private readonly List<PropertyItem> properties = new List<PropertyItem>();
        private readonly List<SingularDataItem> propertyValues = new List<SingularDataItem>();

        private string DataSchemaName = string.Empty;

        internal Type SchemaType { get; private set; } = null;

        internal MLDataSchemaBuilder(string dataSchemaName)
        {
            DataSchemaName = dataSchemaName;
        }

        internal bool SchemaProperlySetup(out string problemMessage)
        {
            problemMessage = string.Empty;
            // We require one, and only one, label for the attribute.
            if (properties.SingleOrDefault(p => p.ColumnNameAttribute != null && p.ColumnNameAttribute.IsLabel) == null)
            {
                problemMessage = "Exactly one of the properties is required to be a label";
                return false;
            }

            return true;
        }

        /// <summary>
        /// Property item for the schema
        /// </summary>
        private class PropertyItem
        {
            internal string Name { get; private set; }

            internal Type Type { get; private set; }

            internal ColumnNameStorageAttribute ColumnNameAttribute { get; private set; }

            internal static PropertyItem CreateInstance(string name, Type type, string columnName = null, bool isLabel = false)
            {
                return new PropertyItem
                {
                    Name = name,
                    Type = type,
                    ColumnNameAttribute = new ColumnNameStorageAttribute(columnName ?? name, type, isLabel)
                };
            }

            internal static PropertyItem CreateInstanceWithoutAttributes(string name, Type type) => new PropertyItem { Name = name, Type = type, ColumnNameAttribute = null };
        }

        /// <summary>
        /// Gets the schema properties, that allow modification of indications
        /// </summary>
        /// <returns></returns>
        internal IEnumerable<ColumnNameStorageAttribute> GetSchemaProperties() => properties.Select(p => p.ColumnNameAttribute).Where(c => c != null);

        /// <summary>
        /// Similar to column names in concrete object version, but with the lack of concrete objects, we make use of the make-shift PropertyItem.
        /// </summary>
        /// <param name="attributePredicate">Predicate on whether one wants the labels, non-labels, or both</param>
        /// <param name="columnNames">[Output] Column names</param>
        /// <returns>True if any items are found</returns>
        internal bool TryGetColumnNames(Predicate<ColumnNameStorageAttribute> attributePredicate, out List<string> columnNames)
        {
            columnNames = new List<string>();
            foreach (PropertyItem property in properties)
            {
                try
                {
                    ColumnNameStorageAttribute att = property.ColumnNameAttribute;
                    if (!string.IsNullOrEmpty(att.Name) && attributePredicate(att))
                    {
                        columnNames.Add(att.Name);
                    }
                }
                catch
                {
                    continue;
                }
            }

            return columnNames.Any();
        }

        private class SingularDataItem
        {
            internal List<PropertyItem> properties = new List<PropertyItem>();
            internal Type schema = null;
            internal Dictionary<string, object> propertyNameValuePair = new Dictionary<string, object>();

            internal bool SetPropertyValues(IEnumerable<(string, object)> propertyNameValuePairs)
            {
                foreach ((string name, object value) in propertyNameValuePairs)
                {
                    if (properties.SingleOrDefault(property => property.Name == name) is PropertyItem match &&
                        match.ColumnNameAttribute.ColumnType == value.GetType())
                    {
                        propertyNameValuePair[name] = value;
                    }
                }

                return true;
            }

            internal void SetSchemaType(Type schemaType) => schema = schemaType;

            internal bool TryGetObjectInstance(out object validInstance)
            {
                validInstance = null;
                if (schema == null || properties.Count != propertyNameValuePair.Count)
                {
                    return false;
                }

                validInstance = Activator.CreateInstance(schema);
                foreach (PropertyInfo propertyInfo in schema.GetProperties())
                {
                    if (propertyNameValuePair.TryGetValue(propertyInfo.Name, out object validValue))
                    {
                        propertyInfo.SetValue(validInstance, validValue);
                    }
                }

                // Even though there might be some values that are not populated, return true.
                return true;
            }

            internal bool TryGetDataAsPropertyNameValues(out List<(string, object)> nameValuePairs)
            {
                nameValuePairs = new List<(string, object)>();
                if (schema == null || properties.Count != propertyNameValuePair.Count)
                {
                    return false;
                }

                foreach (PropertyInfo propertyInfo in schema.GetProperties())
                {
                    nameValuePairs.Add((propertyInfo.Name,
                        propertyNameValuePair.TryGetValue(propertyInfo.Name, out object value) ? value.ToString() : string.Empty));
                }

                return true;
            }
        }

        /// <summary>
        /// Adds a property for this data schema (dynamic class)
        /// </summary>
        /// <param name="name">Name of the property</param>
        /// <param name="type">Type of the property</param>
        /// <param name="columnName">[Optional] Column name attribute name, if left empty it will use the value of [name]</param>
        /// <param name="isLabel">True if this property is a label</param>
        internal void AddProperty(string name, Type type, string columnName = null, bool isLabel = false)
        {
            properties.Add(PropertyItem.CreateInstance(name, type, columnName, isLabel));
        }

        internal void AddOutputScoreProperty()
        {
            properties.Add(PropertyItem.CreateInstance("Score", typeof(float[])));
        }

        internal bool AddSingularData(IEnumerable<(string, object)> dataValues)
        {
            SingularDataItem data = new SingularDataItem { properties = properties, schema = SchemaType };
            propertyValues.Add(data);
            return data.SetPropertyValues(dataValues);
        }

        /// <summary>
        /// Attempts to add a singular data by converting a given object instance, given that the schema of the 
        /// object is the same as the schema defined in this builder.
        /// </summary>
        /// <param name="objectInstance">Object instance to be converted and added as singular data</param>
        /// <returns>True if the object instance can be added as a singular data instance</returns>
        internal bool AddSingularData(object objectInstance)
        {
            if (SchemaType == null || objectInstance.GetType() != SchemaType)
            {
                return false;
            }

            List<(string, object)> dataValues = new List<(string, object)>();
            foreach(PropertyInfo objectProperty in SchemaType.GetProperties())
            {
                dataValues.Add((objectProperty.Name, objectProperty.GetValue(objectInstance)));
            }

            return AddSingularData(dataValues);
        }

        /// <summary>
        /// Creates a type based on the property/type values specified in the properties
        /// </summary>
        /// <param name="properties">Property items as part of the class schema</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        private Type CreateDynamicType()
        {
            StringBuilder classCode = new StringBuilder();

            // Create a class using title case of the data schema name
            string dynamicClassName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(DataSchemaName);
            dynamicClassName = dynamicClassName.Replace(" ", "");

            // Generate the class code
            classCode.AppendLine("using System;");
            classCode.AppendLine("using MLTrainer;");
            classCode.AppendLine("namespace MLTrainerPredictor.Dynamic {");
            classCode.AppendLine($"public class {dynamicClassName} {{");

            foreach (var property in properties)
            {
                if (property.ColumnNameAttribute != null)
                {
                    classCode.AppendLine($"[ColumnNameStorage(\"{property.ColumnNameAttribute.Name}\",typeof({property.ColumnNameAttribute.ColumnType.Name}), {property.ColumnNameAttribute.IsLabel.ToString().ToLower()})]");
                }
                classCode.AppendLine($"public {property.Type.Name} {property.Name} {{get; set; }}");
            }
            classCode.AppendLine("}");
            classCode.AppendLine("}");

            var syntaxTree = CSharpSyntaxTree.ParseText(classCode.ToString());

            var references = new MetadataReference[]
            {
                MetadataReference.CreateFromFile(typeof(ColumnNameStorageAttribute).GetTypeInfo().Assembly.Location),
                MetadataReference.CreateFromFile(typeof(object).GetTypeInfo().Assembly.Location),
                MetadataReference.CreateFromFile(typeof(DictionaryBase).GetTypeInfo().Assembly.Location)
            };

            var compilation = CSharpCompilation.Create(dynamicClassName + Guid.NewGuid() + ".dll",
                syntaxTrees: new[] { syntaxTree },
                references: references,
                options: new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

            using (var ms = new MemoryStream())
            {
                var result = compilation.Emit(ms);

                if (!result.Success)
                {
                    var failures = result.Diagnostics.Where(diagnostic =>
                        diagnostic.IsWarningAsError ||
                        diagnostic.Severity == DiagnosticSeverity.Error);

                    var message = new StringBuilder();

                    foreach (var diagnostic in failures)
                    {
                        message.AppendFormat("{0}: {1}", diagnostic.Id, diagnostic.GetMessage());
                    }

                    throw new Exception($"Invalid property definition: {message}.");
                }
                else
                {

                    ms.Seek(0, SeekOrigin.Begin);
                    var assembly = Assembly.Load(ms.GetBuffer());
                    var dynamicType = assembly.GetType($"MLTrainerPredictor.Dynamic.{dynamicClassName}");
                    return dynamicType;
                }
            }
        }

        internal void InitialiseSchemaType()
        {
            // Ensure that schema type is not yet set.
            if (SchemaType != null)
            {
                return;
            }
            SchemaType = CreateDynamicType();
            // Go through all the data and set the schemas for data type conversions later
            propertyValues.ForEach(p => p.SetSchemaType(SchemaType));
        }

        internal object GetInputData()
        {
            Type listType = typeof(List<>);
            Type dynamicListType = listType.MakeGenericType(SchemaType);
            object inputData = Activator.CreateInstance(dynamicListType);

            void AddAction(object newObject)
            {
                MethodInfo addMethod = dynamicListType.GetMethod("Add");
                addMethod.Invoke(inputData, new[] { newObject });
            }
            foreach (SingularDataItem data in propertyValues)
            {
                if (data.TryGetObjectInstance(out object objectInstance))
                {
                    AddAction(objectInstance);
                }
            }
            return inputData;
        }

        internal IEnumerable<List<(string,object)>> GetInputDataAsNameValuePairs()
        {
            List<(string, object)> validDataAsNameValuePairs = new List<(string, object)>();
            return from SingularDataItem data in propertyValues
                   where data.TryGetDataAsPropertyNameValues(out validDataAsNameValuePairs)
                   select validDataAsNameValuePairs;
        }

        /// <summary>
        /// Attempts to create an output data schema builder instance based on the current instance
        /// This should only involve the property which is a label, and a Score property.
        /// </summary>
        /// <param name="outputDataSchemaBuilder">[Output] Data schema builder as output</param>
        /// <returns></returns>
        internal bool TryCreateOutputDataSchemaBuilder(out MLDataSchemaBuilder outputDataSchemaBuilder)
        {
            outputDataSchemaBuilder = new MLDataSchemaBuilder(DataSchemaName + "Output");
            if (!(properties.SingleOrDefault(p => p.ColumnNameAttribute.IsLabel) is PropertyItem labelProperty))
            {
                return false;
            }
            outputDataSchemaBuilder.AddProperty("PredictedLabel", labelProperty.ColumnNameAttribute.ColumnType, isLabel: true);
            outputDataSchemaBuilder.AddOutputScoreProperty();

            outputDataSchemaBuilder.InitialiseSchemaType();

            return true;
        }

        /// <summary>
        /// Creates a new data schema builder instance from a given IDataView instance
        /// A reverse-engineering attempt to convert IDataView instance back to singular data and schema
        /// </summary>
        /// <param name="mlContext">Machine learning context instance, that is used to break down IDataView instance</param>
        /// <param name="dataView">IDataView instance to be converted back to the schema builder instance</param>
        /// <returns>True if a valid new data schema builder instance could be built</returns>
        internal bool TryCloneSchemaWithNewData(MLContext mlContext, IDataView dataView, out MLDataSchemaBuilder newBuilderInstance)
        {
            newBuilderInstance = new MLDataSchemaBuilder(DataSchemaName);

            if (SchemaType == null) 
            {
                return false;
            }

            // Copying the schema properties and type from this instance.
            newBuilderInstance.properties.AddRange(properties);
            newBuilderInstance.SchemaType = SchemaType;

            // Use MLContext instance to break down IDataView instance.
            Type dataType = mlContext.Data.GetType();
            if (!(dataType.GetMethods().FirstOrDefault(m => m.Name == "CreateEnumerable" && m.IsGenericMethod) is MethodInfo loadMethodGeneric))
            {
                return false;
            }

            // Ensure that the conversion ends up with an object, of type generic type
            MethodInfo createEnumerableMethod = loadMethodGeneric.MakeGenericMethod(SchemaType);

            SchemaDefinition schema = SchemaDefinition.Create(SchemaType);

            if (!(createEnumerableMethod.Invoke(mlContext.Data, new object[] {dataView, false, false, null}) is IEnumerable<object> validEnumerable))
            {
                return false;
            }

            foreach(object value in validEnumerable)
            {
                newBuilderInstance.AddSingularData(value);
            }

            return newBuilderInstance.propertyValues.Any();
        }

        /// <summary>
        /// Gets the R-squared value between this instance and the other instance
        /// </summary>
        /// <param name="other">Other data schema builder instance</param>
        /// <returns>R-squared value</returns>
        internal double? GetRSquared(MLDataSchemaBuilder other)
        {
            if (propertyValues.Count != other.propertyValues.Count)
            {
                return null;
            }

            List<double> actualValues = new List<double>();
            List<double> residualSquared = new List<double>();

            for(int i = 0; i < propertyValues.Count; i++)
            {
                SingularDataItem item = propertyValues[i];
                SingularDataItem otherItem = other.propertyValues[i];

                // Get their labels and their corresponding float values
                string label = item.properties.SingleOrDefault(prop => prop.ColumnNameAttribute.IsLabel)?.Name;
                string otherLabel = otherItem.properties.SingleOrDefault(prop => prop.ColumnNameAttribute.IsLabel)?.Name;
                if (!item.propertyNameValuePair.TryGetValue(label, out object labelValue) ||
                    !otherItem.propertyNameValuePair.TryGetValue(otherLabel, out object otherLabelValue) ||
                    !double.TryParse(labelValue.ToString(), out double labelDouble) ||
                    !double.TryParse(otherLabelValue.ToString(), out double otherLabelDouble))
                {
                    continue;
                }

                actualValues.Add(labelDouble);
                residualSquared.Add(Math.Pow(labelDouble - otherLabelDouble, 2));
            }

            double sumSquaredRegression = residualSquared.Sum();
            double mean = actualValues.Sum() / actualValues.Count;
            double sumOfSquares = actualValues.Select(value => Math.Pow(value - mean, 2)).Sum();

            return 1 - sumSquaredRegression / sumOfSquares;
        }
    }
}
