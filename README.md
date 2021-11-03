# Resolution

`Resolution` is a simple utility for resolving DNS records.

### Packages

Current version: `2.0.0`

Target Frameworks: `.NET standard 2`

### Dependencies

- None.

### Development Dependencies

- [Microsoft.NET.Test.SDK](https://www.nuget.org/packages/Microsoft.NET.Test.Sdk/)
- [NUnit](https://www.nuget.org/packages/NUnit/)
- [NUnit3TestAdapter](https://www.nuget.org/packages/NUnit3TestAdapter/)

### Usage

The `Resolver` class serves as the core of this library. It takes the given domain and question type and performs the required DNS lookups. By default, it will use the system default DNS servers but can be configured to use alternatives.

The simplest way to use `Resolver` is to use the static `Query(string domainName, QuestionType questionType, string dnsServer = null, QuestionClass questionClass = QuestionClass.In)` method.

        var response = Resolver.Query("status.github.com", QuestionType.Cname, "1.1.1.1");

The above example queries the Cloudflare DNS server at "1.1.1.1" for the `CNAME` record of `status.github.com`. If the `dnsServer` parameter is `null` the system DNS will be used.

The `Resolver` class can also be configured for re-use using `ResolverFactory.GetResolver(IEnumerable<string> dnsServers = null, int cacheSize = 1000, int retries = 3, TransportType transportType = TransportType.Udp, int timeout = 1, bool useCache = false, bool useRecursion = true)`. 

The `Resolver` configuration options are:

- `IEnumerable<string> dnsServers`, a collection of DNS server IP addresses.
- `int cacheSize`, size of the response cache.
- `int retries`, the number of times a query is retried.
- `TransportType transportType`, the query transport type, either `UDP` or `TCP`.
- `int timeout`, the query timeout in seconds.
- `bool useCache`, indicates if resolver should create and use an in-memory response cache.
- `bool useRecursion`, indicates if the resolver should use query recursion.

        # Get a caching resolver using Cloudflare DNS at 1.1.1.1
        var resolver = ResolverFactory.GetResolver(dnsServers: new string[]{"1.1.1.1"}, useCache: true);

### Defaults

The following are the defaults for the static `Resolver.Query()` method or `ResolverFactory.GetResolver()` with no parameters.

- DNS servers: The system DNS servers.
- Retries: 3.
- Timeout: 1 second.
- Recursion: True.
- Caching: None.
- Cache Size: 1,000

### Caching

The resolver caching is rudimentary and not thread-safe. By default 1,000 responses are stored in the cache. The eviction strategy uses the oldest last access time of a cache item. In other words, if the cache size limit is reached items will be evicted in order of last access to make room for new items. 

### References
- [IANA DNS page](http://www.iana.org/assignments/dns-parameters)
