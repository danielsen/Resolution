

/*

 */

namespace Resolution.Protocol.Records
{
	public class RecordDnskey : Record
	{
		public byte[] Rdata;

		public RecordDnskey(RecordReader rr)
		{
			// re-read length
			ushort rdlength = rr.ReadUInt16(-2);
			Rdata = rr.ReadBytes(rdlength);
		}

		public override string ToString()
		{
			return "not-used";
		}

	}
}
