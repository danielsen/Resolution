/*
 * http://www.iana.org/assignments/dns-parameters
 */

namespace Resolution.Protocol
{
    /*
	 * 3.2.2. TYPE values
	 *
	 * TYPE fields are used in resource records.
	 * Note that these types are a subset of QTYPEs.
	 *
	 *		TYPE		value			meaning
	 */
    public enum Type : ushort
    {
        A = 1,              // a IPV4 host address
        Ns = 2,             // an authoritative name server
        Md = 3,             // a mail destination (Obsolete - use MX)
        Mf = 4,             // a mail forwarder (Obsolete - use MX)
        Cname = 5,          // the canonical name for an alias
        Soa = 6,            // marks the start of a zone of authority
        Mb = 7,             // a mailbox domain name (EXPERIMENTAL)
        Mg = 8,             // a mail group member (EXPERIMENTAL)
        Mr = 9,             // a mail rename domain name (EXPERIMENTAL)
        Null = 10,          // a null RR (EXPERIMENTAL)
        Wks = 11,           // a well known service description
        Ptr = 12,           // a domain name pointer
        Hinfo = 13,         // host information
        Minfo = 14,         // mailbox or mail list information
        Mx = 15,            // mail exchange
        Txt = 16,           // text strings

        Rp = 17,            // The Responsible Person rfc1183
        Afsdb = 18,         // AFS Data Base location
        X25 = 19,           // X.25 address rfc1183
        Isdn = 20,          // ISDN address rfc1183 
        Rt = 21,            // The Route Through rfc1183

        Nsap = 22,          // Network service access point address rfc1706
        Nsapptr = 23,       // Obsolete, rfc1348

        Sig = 24,           // Cryptographic public key signature rfc2931 / rfc2535
        Key = 25,           // Public key as used in DNSSEC rfc2535

        Px = 26,            // Pointer to X.400/RFC822 mail mapping information rfc2163

        Gpos = 27,          // Geographical position rfc1712 (obsolete)

        Aaaa = 28,          // a IPV6 host address, rfc3596

        Loc = 29,           // Location information rfc1876

        Nxt = 30,           // Next Domain, Obsolete rfc2065 / rfc2535

        Eid = 31,           // *** Endpoint Identifier (Patton)
        Nimloc = 32,        // *** Nimrod Locator (Patton)

        Srv = 33,           // Location of services rfc2782

        Atma = 34,          // *** ATM Address (Dobrowski)

        Naptr = 35,         // The Naming Authority Pointer rfc3403

        Kx = 36,            // Key Exchange Delegation Record rfc2230

        Cert = 37,          // *** CERT RFC2538

        A6 = 38,            // IPv6 address rfc3363 (rfc2874 rfc3226)
        Dname = 39,         // A way to provide aliases for a whole domain, not just a single domain name as with CNAME. rfc2672

        Sink = 40,          // *** SINK Eastlake
        Opt = 41,           // *** OPT RFC2671

        Apl = 42,           // *** APL [RFC3123]

        Ds = 43,            // Delegation Signer rfc3658

        Sshfp = 44,         // SSH Key Fingerprint rfc4255
        Ipseckey = 45,      // IPSECKEY rfc4025
        Rrsig = 46,         // RRSIG rfc3755
        Nsec = 47,          // NSEC rfc3755
        Dnskey = 48,        // DNSKEY 3755
        Dhcid = 49,         // DHCID rfc4701

        Nsec3 = 50,         // NSEC3 rfc5155
        Nsec3Param = 51,    // NSEC3PARAM rfc5155

        Hip = 55,           // Host Identity Protocol  [RFC-ietf-hip-dns-09.txt]

        Spf = 99,           // SPF rfc4408

        Uinfo = 100,        // *** IANA-Reserved
        Uid = 101,          // *** IANA-Reserved
        Gid = 102,          // *** IANA-Reserved
        Unspec = 103,       // *** IANA-Reserved

        Tkey = 249,         // Transaction key rfc2930
        Tsig = 250,         // Transaction signature rfc2845

        Ta = 32768,         // DNSSEC Trust Authorities          [Weiler]  13 December 2005
        Dlv = 32769         // DNSSEC Lookaside Validation       [RFC4431]
    }

    /*
	 * 3.2.3. QTYPE values
	 *
	 * QTYPE fields appear in the question part of a query.  QTYPES are a
	 * superset of TYPEs, hence all TYPEs are valid QTYPEs.  In addition, the
	 * following QTYPEs are defined:
	 *
	 *		QTYPE		value			meaning
	 */
    public enum QType : ushort
    {
        A = Type.A,         // a IPV4 host address
        Ns = Type.Ns,       // an authoritative name server
        Md = Type.Md,       // a mail destination (Obsolete - use MX)
        Mf = Type.Mf,       // a mail forwarder (Obsolete - use MX)
        Cname = Type.Cname, // the canonical name for an alias
        Soa = Type.Soa,     // marks the start of a zone of authority
        Mb = Type.Mb,       // a mailbox domain name (EXPERIMENTAL)
        Mg = Type.Mg,       // a mail group member (EXPERIMENTAL)
        Mr = Type.Mr,       // a mail rename domain name (EXPERIMENTAL)
        Null = Type.Null,   // a null RR (EXPERIMENTAL)
        Wks = Type.Wks,     // a well known service description
        Ptr = Type.Ptr,     // a domain name pointer
        Hinfo = Type.Hinfo, // host information
        Minfo = Type.Minfo, // mailbox or mail list information
        Mx = Type.Mx,       // mail exchange
        Txt = Type.Txt,     // text strings

        Rp = Type.Rp,       // The Responsible Person rfc1183
        Afsdb = Type.Afsdb, // AFS Data Base location
        X25 = Type.X25,     // X.25 address rfc1183
        Isdn = Type.Isdn,   // ISDN address rfc1183
        Rt = Type.Rt,       // The Route Through rfc1183

        Nsap = Type.Nsap,   // Network service access point address rfc1706
        NsapPtr = Type.Nsapptr, // Obsolete, rfc1348

        Sig = Type.Sig,     // Cryptographic public key signature rfc2931 / rfc2535
        Key = Type.Key,     // Public key as used in DNSSEC rfc2535

        Px = Type.Px,       // Pointer to X.400/RFC822 mail mapping information rfc2163

        Gpos = Type.Gpos,   // Geographical position rfc1712 (obsolete)

        Aaaa = Type.Aaaa,   // a IPV6 host address

        Loc = Type.Loc,     // Location information rfc1876

        Nxt = Type.Nxt,     // Obsolete rfc2065 / rfc2535

        Eid = Type.Eid,     // *** Endpoint Identifier (Patton)
        Nimloc = Type.Nimloc,// *** Nimrod Locator (Patton)

        Srv = Type.Srv,     // Location of services rfc2782

        Atma = Type.Atma,   // *** ATM Address (Dobrowski)

        Naptr = Type.Naptr, // The Naming Authority Pointer rfc3403

        Kx = Type.Kx,       // Key Exchange Delegation Record rfc2230

        Cert = Type.Cert,   // *** CERT RFC2538

        A6 = Type.A6,       // IPv6 address rfc3363
        Dname = Type.Dname, // A way to provide aliases for a whole domain, not just a single domain name as with CNAME. rfc2672

        Sink = Type.Sink,   // *** SINK Eastlake
        Opt = Type.Opt,     // *** OPT RFC2671

        Apl = Type.Apl,     // *** APL [RFC3123]

        Ds = Type.Ds,       // Delegation Signer rfc3658

        Sshfp = Type.Sshfp, // *** SSH Key Fingerprint RFC-ietf-secsh-dns
        Ipseckey = Type.Ipseckey, // rfc4025
        Rrsig = Type.Rrsig, // *** RRSIG RFC-ietf-dnsext-dnssec-2535
        Nsec = Type.Nsec,   // *** NSEC RFC-ietf-dnsext-dnssec-2535
        Dnskey = Type.Dnskey,// *** DNSKEY RFC-ietf-dnsext-dnssec-2535
        Dhcid = Type.Dhcid, // rfc4701

        Nsec3 = Type.Nsec3, // RFC5155
        Nsec3Param = Type.Nsec3Param, // RFC5155

        Hip = Type.Hip,     // RFC-ietf-hip-dns-09.txt

        Spf = Type.Spf,     // RFC4408
        Uinfo = Type.Uinfo, // *** IANA-Reserved
        Uid = Type.Uid,     // *** IANA-Reserved
        Gid = Type.Gid,     // *** IANA-Reserved
        Unspec = Type.Unspec,// *** IANA-Reserved

        Tkey = Type.Tkey,   // Transaction key rfc2930
        Tsig = Type.Tsig,   // Transaction signature rfc2845

        Ixfr = 251,         // incremental transfer                  [RFC1995]
        Axfr = 252,         // transfer of an entire zone            [RFC1035]
        Mailb = 253,        // mailbox-related RRs (MB, MG or MR)    [RFC1035]
        Maila = 254,        // mail agent RRs (Obsolete - see MX)    [RFC1035]
        Any = 255,          // A request for all records             [RFC1035]

        Ta = Type.Ta,       // DNSSEC Trust Authorities    [Weiler]  13 December 2005
        Dlv = Type.Dlv      // DNSSEC Lookaside Validation [RFC4431]
    }

    /*
	 * 3.2.4. CLASS values
	 *
	 * CLASS fields appear in resource records.  The following CLASS mnemonics
	 *and values are defined:
	 *
	 *		CLASS		value			meaning
	 */
    public enum Class : ushort
    {
        In = 1,             // the Internet
        Cs = 2,             // the CSNET class (Obsolete - used only for examples in some obsolete RFCs)
        Ch = 3,             // the CHAOS class
        Hs = 4              // Hesiod [Dyer 87]
    }

    /*
	 * 3.2.5. QCLASS values
	 *
	 * QCLASS fields appear in the question section of a query.  QCLASS values
	 * are a superset of CLASS values; every CLASS is a valid QCLASS.  In
	 * addition to CLASS values, the following QCLASSes are defined:
	 *
	 *		QCLASS		value			meaning
	 */
    public enum QClass : ushort
    {
        In = Class.In,      // the Internet
        Cs = Class.Cs,      // the CSNET class (Obsolete - used only for examples in some obsolete RFCs)
        Ch = Class.Ch,      // the CHAOS class
        Hs = Class.Hs,      // Hesiod [Dyer 87]

        Any = 255           // any class
    }

    /*
     * 2.3 RCODE values           
     * Response code - this 4 bit field is set as part of
     * responses.  The values have the following interpretation:
	 *
	 *		QCLASS		value			meaning
	 */
    public enum RCode
    {
        NoError = 0,        // No Error                           [RFC1035]
        FormErr = 1,        // Format Error                       [RFC1035]
        ServFail = 2,       // Server Failure                     [RFC1035]
        NxDomain = 3,       // Non-Existent Domain                [RFC1035]
        NotImp = 4,         // Not Implemented                    [RFC1035]
        Refused = 5,        // Query Refused                      [RFC1035]
        YxDomain = 6,       // Name Exists when it should not     [RFC2136]
        YxrrSet = 7,        // RR Set Exists when it should not   [RFC2136]
        NxrrSet = 8,        // RR Set that should exist does not  [RFC2136]
        NotAuth = 9,        // Server Not Authoritative for zone  [RFC2136]
        NotZone = 10,       // Name not contained in zone         [RFC2136]

        Reserved11 = 11,    // Reserved
        Reserved12 = 12,    // Reserved
        Reserved13 = 13,    // Reserved
        Reserved14 = 14,    // Reserved
        Reserved15 = 15,    // Reserved

        Badverssig = 16,    // Bad OPT Version                    [RFC2671]
                            // TSIG Signature Failure             [RFC2845]
        Badkey = 17,        // Key not recognized                 [RFC2845]
        Badtime = 18,       // Signature out of time window       [RFC2845]
        Badmode = 19,       // Bad TKEY Mode                      [RFC2930]
        Badname = 20,       // Duplicate key name                 [RFC2930]
        Badalg = 21,        // Algorithm not supported            [RFC2930]
        Badtrunc = 22       // Bad Truncation                     [RFC4635]
                            /*
                                23-3840              available for assignment
                                    0x0016-0x0F00
                                3841-4095            Private Use
                                    0x0F01-0x0FFF
                                4096-65535           available for assignment
                                    0x1000-0xFFFF
                            */

    }

    /*
     * 2.2 OPCODE          
     * A four bit field that specifies kind of query in this
     * message.  This value is set by the originator of a query
     * and copied into the response.  The values are:
     *     0               a standard query (QUERY)
     *     1               an inverse query (IQUERY)
     *     2               a server status request (STATUS)
     *     3-15            reserved for future use
	 */
    public enum OpCode
    {
        Query = 0,              // a standard query (QUERY)
        Iquery = 1,             // OpCode Retired (previously IQUERY - No further [RFC3425]
                                // assignment of this code available)
        Status = 2,             // a server status request (STATUS) RFC1035
        Reserved3 = 3,          // IANA

        Notify = 4,             // RFC1996
        Update = 5,             // RFC2136

        Reserved6 = 6,
        Reserved7 = 7,
        Reserved8 = 8,
        Reserved9 = 9,
        Reserved10 = 10,
        Reserved11 = 11,
        Reserved12 = 12,
        Reserved13 = 13,
        Reserved14 = 14,
        Reserved15 = 15
    }

    public enum TransportType
    {
        Udp,
        Tcp
    }
}
