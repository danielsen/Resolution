using System.Net;
using NUnit.Framework;
using Resolution.Protocol;

namespace Resolution.Tests.Unit
{
    [TestFixture]
    public class ResponseTests
    {
        [Test]
        public void should_parse_response_with_empty_header()
        {
            var content = Common.ReadFixture("Response", "empty-header_basic");
            var response = new Response(new IPEndPoint(0, 0), content);

            Assert.AreEqual(response.Header.Id, 0);
            Assert.AreEqual(response.Header.Ra, false);
            Assert.AreEqual(response.Questions.Count, 1);
            Assert.AreEqual(response.Answers.Count, 1);
            Assert.AreEqual(response.Authorities.Count, 1);
            Assert.AreEqual(response.Additionals.Count, 1);
        }
    }
}
