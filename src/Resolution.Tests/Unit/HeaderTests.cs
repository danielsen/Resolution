using NUnit.Framework;
using Resolution.Protocol;

namespace Resolution.Tests.Unit
{
    [TestFixture]
    public class HeaderTests
    {
        private byte[] GetHeaderContent(string param)
        {
            return Common.ReadFixture("Header", param);
        }

        [TestCase("id")]
        public void should_read_id(string param)
        {
            var content = GetHeaderContent(param);
            var header = new Header(new RecordReader(content));

            Assert.AreEqual(header.Id, 1);
        }

        [TestCase("qdcount")]
        public void should_read_qdcount(string param)
        {
            var content = GetHeaderContent(param);
            var header = new Header(new RecordReader(content));

            Assert.AreEqual(header.Qdcount, 1);
        }

        [TestCase("arcount")]
        public void should_read_arcount(string param)
        {
            var content = GetHeaderContent(param);
            var header = new Header(new RecordReader(content));

            Assert.AreEqual(header.Arcount, 1);
        }

        [TestCase("ancount")]
        public void should_read_ancount(string param)
        {
            var content = GetHeaderContent(param);
            var header = new Header(new RecordReader(content));

            Assert.AreEqual(header.Ancount, 1);
        }

        [TestCase("nscount")]
        public void should_read_ns_count(string param)
        {
            var content = GetHeaderContent(param);
            var header = new Header(new RecordReader(content));

            Assert.AreEqual(header.Nscount, 1);
        }

        [TestCase("qr")]
        public void should_read_qr(string param)
        {
            var content = GetHeaderContent(param);
            var header = new Header(new RecordReader(content));

            Assert.AreEqual(header.Qr, true);
        }

        [TestCase("opcode")]
        public void should_read_opcode(string param)
        {
            var content = GetHeaderContent(param);
            var header = new Header(new RecordReader(content));

            Assert.AreEqual(header.Opcode, OperationCode.Status);
        }

        [TestCase("aa")]
        public void should_read_aa(string param)
        {
            var content = GetHeaderContent(param);
            var header = new Header(new RecordReader(content));

            Assert.AreEqual(header.Aa, true);
        }

        [TestCase("tc")]
        public void should_read_tc(string param)
        {
            var content = GetHeaderContent(param);
            var header = new Header(new RecordReader(content));

            Assert.AreEqual(header.Tc, true);
        }

        [TestCase("rd")]
        public void should_read_rd(string param)
        {
            var content = GetHeaderContent(param);
            var header = new Header(new RecordReader(content));

            Assert.AreEqual(header.Rd, true);
        }

        [TestCase("ra")]
        public void should_read_ra(string param)
        {
            var content = GetHeaderContent(param);
            var header = new Header(new RecordReader(content));

            Assert.AreEqual(header.Ra, true);
        }

        [TestCase("rcode")]
        public void should_read_rcode(string param)
        {
            var content = GetHeaderContent(param);
            var header = new Header(new RecordReader(content));

            Assert.AreEqual(header.Rcode, ResponseCode.ServFail);
        }
    }
}
