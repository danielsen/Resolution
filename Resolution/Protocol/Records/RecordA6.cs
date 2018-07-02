namespace Resolution.Protocol.Records
{
    [NotUsed]
	public class RecordA6 : Record
	{
		public byte[] Rdata;

		public RecordA6(RecordReader rr)
		{
			// re-read length
			ushort rdlength = rr.ReadUInt16(-2);
			Rdata = rr.ReadBytes(rdlength);
		}
	}
}
