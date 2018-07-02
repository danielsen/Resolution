

/*
3.3.3. MB RDATA format (EXPERIMENTAL)

    +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
    /                   MADNAME                     /
    /                                               /
    +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+

where:

MADNAME         A <domain-name> which specifies a host which has the
                specified mailbox.

MB records cause additional section processing which looks up an A type
RRs corresponding to MADNAME.
*/
namespace Resolution.Protocol.Records
{
	public class RecordMb : Record
	{
		public string Madname;

		public RecordMb(RecordReader rr)
		{
			Madname = rr.ReadDomainName();
		}

		public override string ToString()
		{
			return Madname;
		}

	}
}
