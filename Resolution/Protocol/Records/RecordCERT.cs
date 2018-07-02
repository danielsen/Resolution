using System;

/*

 CERT RR
 *                     1 1 1 1 1 1 1 1 1 1 2 2 2 2 2 2 2 2 2 2 3 3
   0 1 2 3 4 5 6 7 8 9 0 1 2 3 4 5 6 7 8 9 0 1 2 3 4 5 6 7 8 9 0 1
   +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+
   |             type              |             key tag           |
   +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+
   |   algorithm   |                                               /
   +---------------+            certificate or CRL                 /
   /                                                               /
   +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-|
 */

namespace Resolution.Protocol.Records
{
	public class RecordCert : Record
	{
		public byte[] Rdata;
        public ushort Type;
        public ushort Keytag;  //Format
        public byte Algorithm;
        public string Publickey;
        public byte[] Rawkey;

		public RecordCert(RecordReader rr)
		{
			// re-read length
			ushort rdlength = rr.ReadUInt16(-2);

            Type = rr.ReadUInt16();
            Keytag = rr.ReadUInt16();
            Algorithm = rr.ReadByte();
            var length = rdlength - 5;
            Rawkey = rr.ReadBytes(length);
            Publickey = Convert.ToBase64String(Rawkey);
		}

		public override string ToString()
		{
            return Publickey;
		}

	}
}
