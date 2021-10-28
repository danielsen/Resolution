using System;

namespace Resolution.Protocol.Records
{
	/*
	3.3.9. MX RDATA format

		+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
		|                  PREFERENCE                   |
		+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
		/                   EXCHANGE                    /
		/                                               /
		+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+

	where:

	PREFERENCE      A 16 bit integer which specifies the preference given to
					this RR among others at the same owner.  Lower values
					are preferred.

	EXCHANGE        A <domain-name> which specifies a host willing to act as
					a mail exchange for the owner name.

	MX records cause type A additional section processing for the host
	specified by EXCHANGE.  The use of MX RRs is explained in detail in
	[RFC-974].
	*/

	public class RecordMx : Record, IComparable
	{
		public ushort Preference;
		public string Exchange;

		public RecordMx(RecordReader rr)
		{
			Preference = rr.ReadUInt16();
			Exchange = rr.ReadDomainName();
		}

		public override string ToString()
		{
			return $"{Preference} {Exchange}";
		}

		public int CompareTo(object objA)
		{
			RecordMx recordMx = objA as RecordMx;
			if (recordMx == null)
				return -1;
		    if (Preference > recordMx.Preference)
		        return 1;
		    if (Preference < recordMx.Preference)
		        return -1;
		    return String.Compare(Exchange, recordMx.Exchange, StringComparison.OrdinalIgnoreCase);
		}

	}
}
