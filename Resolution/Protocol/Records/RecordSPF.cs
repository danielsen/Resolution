namespace Resolution.Protocol.Records
{
    [NotUsed]
	public class RecordSpf : Record
	{
		public byte[] Rdata;

		public RecordSpf(RecordReader rr)
		{
			// re-read length
			ushort rdlength = rr.ReadUInt16(-2);
			Rdata = rr.ReadBytes(rdlength);
		}
	}
}
