namespace Resolution.Protocol.Records
{
    [NotUsed]
	public class RecordSshfp : Record
	{
		public byte[] Rdata;

		public RecordSshfp(RecordReader rr)
		{
			// re-read length
			ushort rdlength = rr.ReadUInt16(-2);
			Rdata = rr.ReadBytes(rdlength);
		}
	}
}
