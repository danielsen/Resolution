namespace Resolution.Protocol.Records
{
    [NotUsed]
	public class RecordUnspec : Record
	{
		public byte[] Rdata;

		public RecordUnspec(RecordReader rr)
		{
			// re-read length
			ushort rdlength = rr.ReadUInt16(-2);
			Rdata = rr.ReadBytes(rdlength);
		}
	}
}
