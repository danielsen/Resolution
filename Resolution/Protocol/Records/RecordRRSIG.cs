namespace Resolution.Protocol.Records
{
    [NotUsed]
	public class RecordRrsig : Record
	{
		public byte[] Rdata;

		public RecordRrsig(RecordReader rr)
		{
			// re-read length
			ushort rdlength = rr.ReadUInt16(-2);
			Rdata = rr.ReadBytes(rdlength);
		}
	}
}
