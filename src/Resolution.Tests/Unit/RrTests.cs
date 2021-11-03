using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Resolution.Protocol;

namespace Resolution.Tests.Unit
{
    [TestFixture]
    public class RrTests
    {
        private byte[] GetContentResource(string name)
        {
            return Common.ReadFixture("ResourceRecord", name);
        }

        [TestCase("empty-domain_basic", Protocol.Type.A, Class.In, 0)]
        [TestCase("empty-domain_cname", Protocol.Type.Cname, Class.In, 0)]
        [TestCase("empty-domain_ttl", Protocol.Type.A, Class.In, 1)]
        public void should_read_resource_record(string resource, Protocol.Type type, Class pClass, int ttl)
        {
            var content = GetContentResource(resource);
            var rr = new ResourceRecord(new RecordReader(content));

            Assert.AreEqual(rr.Class, pClass);
            Assert.AreEqual(rr.Type, type);
            Assert.AreEqual(rr.Ttl, ttl);
        }
    }
}
