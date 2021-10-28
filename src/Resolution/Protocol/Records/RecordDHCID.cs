namespace Resolution.Protocol.Records
{
    [NotUsed]
	public class RecordDhcid : Record
	{
		public byte[] Rdata;

		public RecordDhcid(RecordReader rr)
		{
			// re-read length
			ushort rdlength = rr.ReadUInt16(-2);
			Rdata = rr.ReadBytes(rdlength);
		}
	}
}
