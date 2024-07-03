
using System.Text.Json;
using System.Text.Json.Serialization;
using Xunit.Abstractions;

namespace ApiExamples.IntegrationTests
{
    public static class TestHelper
    {

        public static string ObjectToString(object? obj)
        {
            return JsonSerializer.Serialize(obj, new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve,
            });
        }

        public static void Print(ITestOutputHelper output, object obj)
        {
            output.WriteLine(JsonSerializer.Serialize(obj,
                 new JsonSerializerOptions { WriteIndented = true }));
            output.WriteLine(obj.GetType().ToString());
        }

        public static void AssertObjectEqual(object expected, object? actual)
        {
            Assert.Equal(ObjectToString(expected), ObjectToString(actual));
        }

    }
}
