using System.Text.Json;
using Xunit.Abstractions;

namespace ApiExamples.UnitTests
{
    public static class TestHelper
    {

        public static string ObjectToString(object? obj)
        {
            return JsonSerializer.Serialize(obj);
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
