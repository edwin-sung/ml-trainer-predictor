using Microsoft.ML;
using Microsoft.ML.Data;
using MLTrainer.DynamicDataBuilder;
using System.Collections.Generic;
using System.Linq;

namespace MLTrainer
{
    /// <summary>
    /// Ambitious machine-learning data schema builder, which will be used for data input and output
    /// </summary>
    internal class MLDataSchemaBuilder
    {

        internal void MakeSchema()
        {
            List<DynamicTypeProperty> properties  = new List<DynamicTypeProperty>
            {
                new DynamicTypeProperty("SepalLength", typeof(float), "sepal_length"),
                new DynamicTypeProperty("TestString", typeof(string), "test_string", true)
            };

            // Create the new type.
            var dynamicType = DynamicType.CreateDynamicType(properties);
            var schema = SchemaDefinition.Create(dynamicType);

            // Create list with required data
            var dynamicList = DynamicType.CreateDynamicList(dynamicType);
            // Get an action that will add to the list
            var addAction = DynamicType.GetAddAction(dynamicList);

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
