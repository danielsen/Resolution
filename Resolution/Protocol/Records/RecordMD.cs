/*
3.3.4. MD RDATA format (Obsolete)

    +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
    /                   MADNAME                     /
    /                                               /
    +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+

where:

MADNAME         A <domain-name> which specifies a host which has a mail
                agent for the domain which should be able to deliver
                mail for the domain.

MD records cause additional section processing which looks up an A type
record corresponding to MADNAME.

MD is obsolete.  See the definition of MX and [RFC-974] for details of
the new scheme.  The recommended policy for dealing with MD RRs found in
a master file is to reject them, or to convert them to MX RRs with a
preference of 0.
 * */
namespace Resolution.Protocol.Records
{
    [Obsolete]
	public class RecordMd : Record
	{
		public string Madname;

		public RecordMd(RecordReader rr)
		{
			Madname = rr.ReadDomainName();
		}

		public override string ToString()
		{
			return Madname;
		}

	}
}
