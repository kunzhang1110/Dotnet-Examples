using CSharpExamples.SerializationExamples;

namespace CSharpExamples.Test
{
    [TestClass]
    public class JsonExamplesTests
    {

        [TestMethod]
        public void SimpleExampleTest()
        {
            JsonExamples.SimpleExample();
        }

        [TestMethod]
        public void ReferencesHandlerExampleTest()
        {
            JsonExamples.ReferencesHandlerExample();
        }
    }
}