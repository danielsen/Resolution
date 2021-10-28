namespace Resolution.Protocol.Records
{
    [NotUsed]
	public class RecordUnknown : Record
	{
		public byte[] Rdata;
		public RecordUnknown(RecordReader rr)
		{
			// re-read length
			ushort rdlength = rr.ReadUInt16(-2);
			Rdata = rr.ReadBytes(rdlength);
		}
	}
}
