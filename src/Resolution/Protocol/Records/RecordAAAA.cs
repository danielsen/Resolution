#region Rfc info
/*
2.2 AAAA data format

   A 128 bit IPv6 address is encoded in the data portion of an AAAA
   resource record in network byte order (high-order byte first).
 */
#endregion

using System.Net;

namespace Resolution.Protocol.Records
{
	public class RecordAaaa : Record
	{
		public IPAddress Address;

		public RecordAaaa(RecordReader rr)
		{
			IPAddress.TryParse(
			    $"{rr.ReadUInt16():x}:{rr.ReadUInt16():x}:{rr.ReadUInt16():x}:{rr.ReadUInt16():x}:{rr.ReadUInt16():x}:{rr.ReadUInt16():x}:{rr.ReadUInt16():x}:{rr.ReadUInt16():x}", out Address);
		}

		public override string ToString()
		{
			return Address.ToString();
		}

	}
}
