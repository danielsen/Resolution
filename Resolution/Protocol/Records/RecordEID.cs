namespace Resolution.Protocol.Records
{
    [NotUsed]
	public class RecordEid : Record
	{
		public byte[] Rdata;

		public RecordEid(RecordReader rr)
		{
			// re-read length
			ushort rdlength = rr.ReadUInt16(-2);
			Rdata = rr.ReadBytes(rdlength);
		}
	}
}
