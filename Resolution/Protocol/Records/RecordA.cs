

/*
 3.4.1. A RDATA format

    +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
    |                    ADDRESS                    |
    +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+

where:

ADDRESS         A 32 bit Internet address.

Hosts that have multiple Internet addresses will have multiple A
records.
 * 
 */

using System.Net;

namespace Resolution.Protocol.Records
{
	public class RecordA : Record
	{
		public IPAddress Address;

		public RecordA(RecordReader rr)
		{
			IPAddress.TryParse($"{rr.ReadByte()}.{rr.ReadByte()}.{rr.ReadByte()}.{rr.ReadByte()}", out Address);
		}

		public override string ToString()
		{
			return Address.ToString();
		}

	}
}
