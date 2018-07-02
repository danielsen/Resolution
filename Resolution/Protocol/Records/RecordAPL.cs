namespace Resolution.Protocol.Records
{
    [NotUsed]
	public class RecordApl : Record
	{
		public byte[] Rdata;

		public RecordApl(RecordReader rr)
		{
			// re-read length
			ushort rdlength = rr.ReadUInt16(-2);
			Rdata = rr.ReadBytes(rdlength);
		}
	}
}
