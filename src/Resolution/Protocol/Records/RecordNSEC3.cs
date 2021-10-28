namespace Resolution.Protocol.Records
{
    [NotUsed]
	public class RecordNsec3 : Record
	{
		public byte[] Rdata;

		public RecordNsec3(RecordReader rr)
		{
			// re-read length
			ushort rdlength = rr.ReadUInt16(-2);
			Rdata = rr.ReadBytes(rdlength);
		}
	}
}
