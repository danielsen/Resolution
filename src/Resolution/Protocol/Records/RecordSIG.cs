using System;

#region Rfc info
/*
 * http://www.ietf.org/rfc/rfc2535.txt
 * 4.1 SIG RDATA Format

   The RDATA portion of a SIG RR is as shown below.  The integrity of
   the RDATA information is protected by the signature field.

                           1 1 1 1 1 1 1 1 1 1 2 2 2 2 2 2 2 2 2 2 3 3
       0 1 2 3 4 5 6 7 8 9 0 1 2 3 4 5 6 7 8 9 0 1 2 3 4 5 6 7 8 9 0 1
      +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+
      |        type covered           |  algorithm    |     labels    |
      +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+
      |                         original TTL                          |
      +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+
      |                      signature expiration                     |
      +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+
      |                      signature inception                      |
      +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+
      |            key  tag           |                               |
      +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+         signer's name         +
      |                                                               /
      +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-/
      /                                                               /
      /                            signature                          /
      /                                                               /
      +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+


*/
#endregion

namespace Resolution.Protocol.Records
{
	public class RecordSig : Record
	{
		public UInt16 Typecovered;
		public byte Algorithm;
		public byte Labels;
		public UInt32 Originalttl;
		public UInt32 Signatureexpiration;
		public UInt32 Signatureinception;
		public UInt16 Keytag;
		public string Signersname;
		public string Signature;

		public RecordSig(RecordReader rr)
		{
			Typecovered = rr.ReadUInt16();
			Algorithm = rr.ReadByte();
			Labels = rr.ReadByte();
			Originalttl = rr.ReadUInt32();
			Signatureexpiration = rr.ReadUInt32();
			Signatureinception = rr.ReadUInt32();
			Keytag = rr.ReadUInt16();
			Signersname = rr.ReadDomainName();
			Signature = rr.ReadString();
		}

		public override string ToString()
		{
			return
			    $"{Typecovered} {Algorithm} {Labels} {Originalttl} {Signatureexpiration} {Signatureinception} {Keytag} {Signersname} \"{Signature}\"";
		}

	}
}
