using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis;
using Microsoft.ML;
using Microsoft.ML.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Globalization;

namespace MLTrainer
{
    /// <summary>
    /// Ambitious machine-learning data schema builder, which will be used for data input and output
    /// </summary>
    internal class MLDataSchemaBuilder
    {
        private List<PropertyItem> properties = new List<PropertyItem>();
        private List<object[]> propertyValues = new List<object[]>();


        internal MLDataSchemaBuilder(string dataSchemaName)
        {
            DataSchemaName = dataSchemaName;
        }

        private string DataSchemaName;

        /// <summary>
        /// Property item for the schema
        /// </summary>
        private class PropertyItem
        {
            internal string Name { get; private set; }

            internal ColumnNameStorageAttribute ColumnNameAttribute { get; private set; }

            internal static PropertyItem CreateInstance<T>(string name, string columnName = null, bool isLabel = false)
            {
                return new PropertyItem
                {
                    Name = name,
                    ColumnNameAttribute = new ColumnNameStorageAttribute(columnName ?? name, typeof(T), isLabel)
                };
            }
        }

        /// <summary>
        /// Adds a property for this data schema (dynamic class)
        /// </summary>
        /// <typeparam name="T">Generic type that is translatable to ML.NET</typeparam>
        /// <param name="name">Name of the property</param>
        /// <param name="columnName">[Optional] Column name attribute name, if left empty it will use the value of [name]</param>
        /// <param name="isLabel">True if this property is a label</param>
        internal void AddProperty<T>(string name, string columnName = null, bool isLabel = false)
        {
            properties.Add(PropertyItem.CreateInstance<T>(name, columnName, isLabel));
        }

        /// <summary>
        /// Adds a singular data item, as array of objects, matching the order of which the property items are introduced.
        /// If there are unmatching sizes between data and property items, or unmatching types, this will be skipped completely.
        /// </summary>
        /// <param name="matchingPropertyValues">Matching property data values as array of objects</param>
        /// <returns>True if the data is added to the collection successfully</returns>
        internal bool AddSingularData(object[] matchingPropertyDataValues)
        {
            // Check that the data has the same size as the property items.
            if (properties.Count != matchingPropertyDataValues.Length)
            {
                return false;
            }

            for(int i = 0; i < properties.Count; i++)
            {
                object matchingValue = matchingPropertyDataValues[i];
                if (matchingValue.GetType() != properties[i].ColumnNameAttribute.ColumnType)
                {
                    return false;
                }
            }

            propertyValues.Add(matchingPropertyDataValues);
            return true;
        }

        /// <summary>
        /// Creates a list of the specified type
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private IEnumerable<object> CreateDynamicList(Type type)
        {
            var listType = typeof(List<>);
            var dynamicListType = listType.MakeGenericType(type);
            return (IEnumerable<object>)Activator.CreateInstance(dynamicListType);
        }

        /// <summary>
        /// creates an action which can be used to add items to the list
        /// </summary>
        /// <param name="listType"></param>
        /// <returns></returns>
        private Action<object[]> GetAddAction(IEnumerable<object> list)
        {
            var listType = list.GetType();
            var addMethod = listType.GetMethod("Add");
            var itemType = listType.GenericTypeArguments[0];
            var itemProperties = itemType.GetProperties();

            var action = new Action<object[]>((values) =>
            {
                var item = Activator.CreateInstance(itemType);

                for (var i = 0; i < values.Length; i++)
                {
                    itemProperties[i].SetValue(item, values[i]);
                }

                addMethod.Invoke(list, new[] { item });
            });

            return action;
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
            classCode.AppendLine("using Microsoft.ML.Data;");
            classCode.AppendLine("using MLTrainer;");
            classCode.AppendLine("namespace MLTrainerPredictor.Dynamic {");
            classCode.AppendLine($"public class {dynamicClassName} {{");

            foreach (var property in properties)
            {
                classCode.AppendLine($"[ColumnNameStorage(\"{property.ColumnNameAttribute.Name}\",typeof({property.ColumnNameAttribute.ColumnType.Name}), {property.ColumnNameAttribute.IsLabel.ToString().ToLower()})]");
                classCode.AppendLine($"public {property.ColumnNameAttribute.ColumnType.Name} {property.Name} {{get; set; }}");
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

        internal void MakeSchema()
        {

            // Create the new type.
            Type dynamicType = CreateDynamicType();
            SchemaDefinition schema = SchemaDefinition.Create(dynamicType);

            // Create list with required data
            IEnumerable<object> dynamicList = CreateDynamicList(dynamicType);
            // Get an action that will add to the list
            Action<object[]> addAction = GetAddAction(dynamicList);

            // Call the action, with an object[] containing parameters
            addAction.Invoke(new object[] { 1.1f, "testString"});



            var mlContext = new MLContext();
            var dataType = mlContext.Data.GetType();
            var loadMethodGeneric = dataType.GetMethods().First(method => method.Name == "LoadFromEnumerable" && method.IsGenericMethod);
            var loadMethod = loadMethodGeneric.MakeGenericMethod(dynamicType);
            var trainData = (IDataView)loadMethod.Invoke(mlContext.Data, new[] { dynamicList, schema });
        }

    }
}
