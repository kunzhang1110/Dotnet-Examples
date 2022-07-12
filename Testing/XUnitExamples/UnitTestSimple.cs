namespace XUnitExamples
{
    public class UnitTestSimple
    {
        [Fact]
        public void IsOdd_1_True()
        {
            Assert.True(IsOdd(1));
        }

        [Theory]
        [InlineData(3)]
        [InlineData(5)]
        public void IsOdd_Multiple_True(int value)
        {
            Assert.True(IsOdd(value));
        }

        bool IsOdd(int value)
        {
            return value % 2 == 1;
        }
    }
}