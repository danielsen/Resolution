using System.Collections.Generic;
using System.Linq;
using Resolution.Protocol;

namespace Resolution.Common
{
    public static class ResolverFactory
    {
        public static Resolver GetResolver(IEnumerable<string> dnsServers = null, int cacheSize = 1000, int retries = 3,
            TransportType transportType = TransportType.Udp, int timeout = 1, bool useCache = false,
            bool useRecursion = true)
        {
            var contextBuilder = new ResolverContextBuilder();
            contextBuilder.CacheSize(cacheSize).Retries(retries).TransportType(transportType).Timeout(timeout)
                .UseCache(useCache).UseRecursion(useRecursion);

            if (dnsServers == null)
                return new Resolver(contextBuilder.Build());

            var enumerable = dnsServers.ToList();
            if (!enumerable.Any())
                return new Resolver(contextBuilder.Build());

            foreach (var host in enumerable)
            {
                contextBuilder.AddDnsServer(host);
            }

            return new Resolver(contextBuilder.Build());
        }
 
    }
}