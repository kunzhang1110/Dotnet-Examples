using Moq;

namespace XUnitExamples
{
    public class UnitTestMoq
    {


        [Fact]
        public void GetTitle_Return_Title()
        {
            var mockArticle = new Mock<IArticle>();
            mockArticle.Setup(x => x.GetTitle()).Returns("SQL");
            Assert.Equal("SQL", mockArticle.Object.GetTitle());
        }

        [Fact]
        public void GetNumber_Return_Same()
        {
            var mockArticle = new Mock<Article>();
            mockArticle.Setup(x => x.GetNumber(It.IsInRange<int>(0, 10, Moq.Range.Inclusive))).Returns(3);
            Assert.Equal(3, mockArticle.Object.GetNumber(3));
        }

        public class Article : IArticle
        {
            private string _title;

            public string GetTitle() { return _title; }
            public virtual int GetNumber(int num) { return num; } //virtual is needed for Moq to override
        }

        public interface IArticle
        {
            string GetTitle();
        }

    }


}