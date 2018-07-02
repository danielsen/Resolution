namespace Resolution.Protocol.Records
{
    [NotUsed]
	public class RecordUid : Record
	{
		public byte[] Rdata;

		public RecordUid(RecordReader rr)
		{
			// re-read length
			ushort rdlength = rr.ReadUInt16(-2);
			Rdata = rr.ReadBytes(rdlength);
		}
	}
}
