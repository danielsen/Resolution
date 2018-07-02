# Resolution

`Resolution` is a simple DNS utility for resolving DNS records.

### Packages

Current version: `1.0.0`

Target Frameworks: `.NET 4.5`, `.NET 4.6`, `.NET CORE 2`, `.NET standard 2`

### Dependencies

- None.

### Development Dependencies

- [Microsoft.NET.Test.SDK](https://www.nuget.org/packages/Microsoft.NET.Test.Sdk/)
- [NUnit](https://www.nuget.org/packages/NUnit/)
- [NUnit3TestAdapter](https://www.nuget.org/packages/NUnit3TestAdapter/)

### Usage

The `Resolver` class serves as the core of this library. It takes the given
domain and question type and performs the required DNS lookups. By default, it
will use the system default DNS servers but can be configured to use 
alternatives.

E.x.

        public Resolver resolver = new Resolver 
        {
            DnsServer = "172.98.193.42" // Use BackplaneDNS
        };

	var response = resolver.Query("status.github.com", QType.Cname, QClass.In);

	// Do something with the response.

### References
- [IANA DNS page](http://www.iana.org/assignments/dns-parameters)
