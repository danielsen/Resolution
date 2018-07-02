namespace Resolution.Protocol.Records
{
    [NotUsed]
	public class RecordAtma : Record
	{
		public byte[] Rdata;

		public RecordAtma(RecordReader rr)
		{
			// re-read length
			ushort rdlength = rr.ReadUInt16(-2);
			Rdata = rr.ReadBytes(rdlength);
		}
	}
}
