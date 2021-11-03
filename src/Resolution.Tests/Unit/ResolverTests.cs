using NUnit.Framework;
using Resolution.Protocol;

namespace Resolution.Tests.Unit
{
    public class ResolverTests
    {
        [Test]
        public void should_get_dns()
        {
            var resolver = new Resolver("1.1.1.1");
            var response = resolver.Query("reachmail.com", QuestionType.Txt);
            Assert.Greater(response.Answers.Count, 0);
        }
    }
}