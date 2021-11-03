using NUnit.Framework;
using Resolution.Protocol;

namespace Resolution.Tests.Unit
{
    [TestFixture]
    public class QuestionTests
    {
        [Test]
        public void should_correctly_parse_question()
        {
            var content = Common.ReadFixture("Question", "www-google-com_basic");
            var question = new Question(new RecordReader(content));

            Assert.AreEqual(question.DomainName, "www.google.com.");
            Assert.AreEqual(question.QuestionClass, QuestionClass.In);
            Assert.AreEqual(question.QuestionType, QuestionType.A);
        }
    }
}
