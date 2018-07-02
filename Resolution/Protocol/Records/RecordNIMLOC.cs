namespace Resolution.Protocol.Records
{
    [NotUsed]
	public class RecordNimloc : Record
	{
		public byte[] Rdata;

		public RecordNimloc(RecordReader rr)
		{
			// re-read length
			ushort rdlength = rr.ReadUInt16(-2);
			Rdata = rr.ReadBytes(rdlength);
		}
	}
}
