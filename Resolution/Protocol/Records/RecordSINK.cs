namespace Resolution.Protocol.Records
{
    [NotUsed]
	public class RecordSink : Record
	{
		public byte[] Rdata;

		public RecordSink(RecordReader rr)
		{
			// re-read length
			ushort rdlength = rr.ReadUInt16(-2);
			Rdata = rr.ReadBytes(rdlength);
		}
	}
}
