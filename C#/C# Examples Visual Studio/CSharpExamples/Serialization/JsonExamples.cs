using System.Text.Json;
using System.Text.Json.Serialization;

namespace CSharpExamples.SerializationExamples
{
    public static class JsonExamples
    {
        public class Employee
        {
            public string Name { get; set; } = "";
            public Employee? Manager { get; set; }

            [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
            public List<Employee>? Assisstants { get; set; }

            [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
            public string? Summary { get; set; }
        }

        public static void SimpleExample()
        {
            Employee tyler = new Employee()
            {
                Name = "Tyler",
                Summary = "Good"
            };
            string tylerJson = JsonSerializer.Serialize(tyler);
            Console.WriteLine(tylerJson);   // {"Name":"Tyler","Manager":null,"Summary":"Good"}


            Employee tyler2 = JsonSerializer.Deserialize<Employee>(tylerJson)!;
            Console.WriteLine(tyler.Name);  //Tyler
        }

        public static void ReferencesHandlerExample()
        {
            Employee tyler = new()
            {
                Name = "Tyler"
            };
            Employee adrian = new()
            {
                Name = "Adrian"
            };
            tyler.Assisstants = new List<Employee> { adrian };
            adrian.Manager = tyler;

            JsonSerializerOptions options = new()
            {

                ReferenceHandler = ReferenceHandler.Preserve, //preserve references and handle circular references
                //ReferenceHandler = ReferenceHandler.IgnoreCycles, //ignore circular references
                WriteIndented = true //pretty format
            };
            string tylerJson = JsonSerializer.Serialize(tyler, options);
            Console.WriteLine($"Tyler serialized:\n{tylerJson}");           

            Employee tylerDeserialized =
                JsonSerializer.Deserialize<Employee>(tylerJson, options)!;

            Console.WriteLine(
                tylerDeserialized.Assisstants?[0].Manager == tylerDeserialized); //true

        }

    }
}
