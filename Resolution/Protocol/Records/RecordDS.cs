using System;
using System.Text;

/*
 * http://tools.ietf.org/rfc/rfc3658.txt
 * 
2.4.  Wire Format of the DS record

   The DS (type=43) record contains these fields: key tag, algorithm,
   digest type, and the digest of a public key KEY record that is
   allowed and/or used to sign the child's apex KEY RRset.  Other keys
   MAY sign the child's apex KEY RRset.

                        1 1 1 1 1 1 1 1 1 1 2 2 2 2 2 2 2 2 2 2 3 3
    0 1 2 3 4 5 6 7 8 9 0 1 2 3 4 5 6 7 8 9 0 1 2 3 4 5 6 7 8 9 0 1
   +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+
   |           key tag             |  algorithm    |  Digest type  |
   +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+
   |                digest  (length depends on type)               |
   +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+
   |                (SHA-1 digest is 20 bytes)                     |
   +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+
   |                                                               |
   +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-|
   |                                                               |
   +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-|
   |                                                               |
   +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+

 */

namespace Resolution.Protocol.Records
{
	public class RecordDs : Record
	{
		public UInt16 Keytag;
		public byte Algorithm;
		public byte Digesttype;
		public byte[] Digest;

		public RecordDs(RecordReader rr)
		{
			ushort length = rr.ReadUInt16(-2);
			Keytag = rr.ReadUInt16();
			Algorithm = rr.ReadByte();
			Digesttype = rr.ReadByte();
			length -= 4;
			Digest = new byte[length];
			Digest = rr.ReadBytes(length);
		}

		public override string ToString()
		{
			StringBuilder sb = new StringBuilder();
			for (int intI = 0; intI < Digest.Length; intI++)
				sb.AppendFormat("{0:x2}", Digest[intI]);
			return $"{Keytag} {Algorithm} {Digesttype} {sb}";
		}

	}
}
