namespace Resolution.Protocol.Records
{
    [NotUsed]
	public class RecordNsec : Record
	{
		public byte[] Rdata;

		public RecordNsec(RecordReader rr)
		{
			// re-read length
			ushort rdlength = rr.ReadUInt16(-2);
			Rdata = rr.ReadBytes(rdlength);
		}
	}
}
