

/*
3.3.6. MG RDATA format (EXPERIMENTAL)

    +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
    /                   MGMNAME                     /
    /                                               /
    +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+

where:

MGMNAME         A <domain-name> which specifies a mailbox which is a
                member of the mail group specified by the domain name.

MG records cause no additional section processing.
*/
namespace Resolution.Protocol.Records
{
	public class RecordMg : Record
	{
		public string Mgmname;

		public RecordMg(RecordReader rr)
		{
			Mgmname = rr.ReadDomainName();
		}

		public override string ToString()
		{
			return Mgmname;
		}

	}
}
