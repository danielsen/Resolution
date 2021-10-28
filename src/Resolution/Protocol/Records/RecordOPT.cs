namespace Resolution.Protocol.Records
{
    [NotUsed]
	public class RecordOpt : Record
	{
		public byte[] Rdata;

		public RecordOpt(RecordReader rr)
		{
			// re-read length
			ushort rdlength = rr.ReadUInt16(-2);
			Rdata = rr.ReadBytes(rdlength);
		}
	}
}
