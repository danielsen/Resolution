using System.Collections.Generic;
using System.Net;
using TransportType = Resolution.Protocol.TransportType;

namespace Resolution.Common
{
    internal class ResolverContext
    {
        public int CacheSize { get; private set; } = 1000;
        public IList<IPEndPoint> DnsServers { get; } = new List<IPEndPoint>();
        public int Retries { get; private set; } = 3;
        public int Timeout { get; private set; } = 1;
        public TransportType TransportType { get; private set; } = TransportType.Udp;
        public bool UseCache { get; private set; } = false;
        public bool UseRecursion { get; private set; } = true;

        public ResolverContext AddDnsServer(string ipAddress, int port = 53)
        {
            DnsServers.Add(new IPEndPoint(IPAddress.Parse(ipAddress), port));
            return this;
        }

        public ResolverContext SetCacheSize(int cacheSize)
        {
            CacheSize = cacheSize;
            return this;
        }

        public ResolverContext SetRetries(int value)
        {
            Retries = value;
            return this;
        }

        public ResolverContext SetTimeout(int value)
        {
            Timeout = value;
            return this;
        }

        public ResolverContext SetTransportType(TransportType value)
        {
            TransportType = value;
            return this;
        }

        public ResolverContext SetUseCache(bool value)
        {
            UseCache = value;
            return this;
        }

        public ResolverContext SetUseRecursion(bool value)
        {
            UseRecursion = value;
            return this;
        }
    }
}