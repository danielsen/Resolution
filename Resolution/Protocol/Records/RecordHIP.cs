namespace Resolution.Protocol.Records
{
    [NotUsed]
	public class RecordHip : Record
	{
		public byte[] Rdata;

		public RecordHip(RecordReader rr)
		{
			// re-read length
			ushort rdlength = rr.ReadUInt16(-2);
			Rdata = rr.ReadBytes(rdlength);
		}
	}
}
