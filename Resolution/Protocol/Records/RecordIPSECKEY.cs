namespace Resolution.Protocol.Records
{
    [NotUsed]
	public class RecordIpseckey : Record
	{
		public byte[] Rdata;

		public RecordIpseckey(RecordReader rr)
		{
			// re-read length
			ushort rdlength = rr.ReadUInt16(-2);
			Rdata = rr.ReadBytes(rdlength);
		}
	}
}
