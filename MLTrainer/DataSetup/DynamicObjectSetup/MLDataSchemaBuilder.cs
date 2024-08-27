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

namespace MLTrainer.DataSetup.DynamicObjectSetup
{
    /// <summary>
    /// Ambitious machine-learning data schema builder, which will be used for data input and output
    /// </summary>
    internal class MLDataSchemaBuilder
    {
        private List<PropertyItem> properties = new List<PropertyItem>();
        private List<SingularDataItem> propertyValues = new List<SingularDataItem>();

        private string DataSchemaName = string.Empty;

        internal Type SchemaType { get; private set; }

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
            private Dictionary<string, object> propertyNameValuePair = new Dictionary<string, object>();

            internal bool SetPropertyValues(IEnumerable<(string, object)> propertyNameValuePairs)
            {
                foreach((string name, object value) in propertyNameValuePairs)
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
                foreach(PropertyInfo propertyInfo in schema.GetProperties())
                {
                    if (propertyNameValuePair.TryGetValue(propertyInfo.Name, out object validValue))
                    {
                        propertyInfo.SetValue(validInstance, validValue);
                    }
                }

                // Even though there might be some values that are not populated, return true.
                return true;
            }

            internal bool TryGetDataAsStrings(out List<string> validDataAsStrings)
            {
                validDataAsStrings = new List<string>();
                if (schema == null || properties.Count != propertyNameValuePair.Count)
                {
                    return false;
                }

                foreach(PropertyInfo propertyInfo in schema.GetProperties())
                {
                    validDataAsStrings.Add(
                        propertyNameValuePair.TryGetValue(propertyInfo.Name, out object value) ? value.ToString() : string.Empty);
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
                addMethod.Invoke(inputData, new[] { newObject} );
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

        internal IEnumerable<List<string>> GetInputDataAsStrings()
        {
            foreach(SingularDataItem data in propertyValues)
            {
                if (data.TryGetDataAsStrings(out List<string> validDataAsStrings))
                {
                    yield return validDataAsStrings;
                }
            }
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
    }
}
