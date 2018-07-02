using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using NUnit.Framework;
using Resolution.Protocol;
using Resolution.Protocol.Records;

namespace Resolution.Tests.Unit.Records
{
    [TestFixture]
    public class RecordTests
    {
        [Test]
        public void should_correctly_handle_a_record()
        {
            var ip = IPAddress.Parse("192.168.1.1");
            var bytes = ip.GetAddressBytes();

            var reader = new RecordReader(bytes);
            var recordA = new RecordA(reader);

            Assert.AreEqual("192.168.1.1", recordA.ToString());
        }

        [Test]
        public void should_correctly_handle_cname_record()
        {
            var bytes = Common.ReadFixture("ResourceRecord", "www-google-com_basic");
            var reader = new RecordReader(bytes);
            var recordCname = new RecordCname(reader);

            Assert.AreEqual("www.google.com.", recordCname.ToString());
        }
    }
}
