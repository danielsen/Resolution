using System;
using System.Net;
using Resolution.Protocol;

namespace Resolution.Common.Caching
{
    internal class ResponseCacheItem
    {
        public ResponseCacheItem(IPAddress dnsServer, Question question, Response response)
        {
            DnsServer = dnsServer;
            Question = question;
            Response = response;
            SetLastUse();
        }

        public IPAddress DnsServer { get; }
        public DateTime LastUsedUtc { get; private set; }
        public Question Question { get; }
        public Response Response { get; }

        public void SetLastUse()
        {
            LastUsedUtc = DateTime.UtcNow;
        }
    }
}