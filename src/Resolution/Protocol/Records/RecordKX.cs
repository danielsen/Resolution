using System;

/*
 * http://tools.ietf.org/rfc/rfc2230.txt
 * 
 * 3.1 KX RDATA format

   The KX DNS record has the following RDATA format:

    +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
    |                  PREFERENCE                   |
    +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
    /                   EXCHANGER                   /
    /                                               /
    +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+

   where:

   PREFERENCE      A 16 bit non-negative integer which specifies the
                   preference given to this RR among other KX records
                   at the same owner.  Lower values are preferred.

   EXCHANGER       A <domain-name> which specifies a host willing to
                   act as a mail exchange for the owner name.

   KX records MUST cause type A additional section processing for the
   host specified by EXCHANGER.  In the event that the host processing
   the DNS transaction supports IPv6, KX records MUST also cause type
   AAAA additional section processing.

   The KX RDATA field MUST NOT be compressed.

 */
namespace Resolution.Protocol.Records
{
	public class RecordKx : Record, IComparable
	{
		public ushort Preference;
		public string Exchanger;

		public RecordKx(RecordReader rr)
		{
			Preference = rr.ReadUInt16();
			Exchanger = rr.ReadDomainName();
		}

		public override string ToString()
		{
			return $"{Preference} {Exchanger}";
		}

		public int CompareTo(object objA)
		{
			RecordKx recordKx = objA as RecordKx;
			if (recordKx == null)
				return -1;
		    if (Preference > recordKx.Preference)
		        return 1;
		    if (Preference < recordKx.Preference)
		        return -1;
		    return string.Compare(Exchanger, recordKx.Exchanger, true);
		}

	}
}
