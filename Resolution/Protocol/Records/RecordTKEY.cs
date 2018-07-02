using System;

/*
 * http://tools.ietf.org/rfc/rfc2930.txt
 * 
2. The TKEY Resource Record

   The TKEY resource record (RR) has the structure given below.  Its RR
   type code is 249.

      Field       Type         Comment
      -----       ----         -------
       Algorithm:   domain
       Inception:   u_int32_t
       Expiration:  u_int32_t
       Mode:        u_int16_t
       Error:       u_int16_t
       Key Size:    u_int16_t
       Key Data:    octet-stream
       Other Size:  u_int16_t
       Other Data:  octet-stream  undefined by this specification

 */

namespace Resolution.Protocol.Records
{
	public class RecordTkey : Record
	{
		public string Algorithm;
		public UInt32 Inception;
		public UInt32 Expiration;
		public UInt16 Mode;
		public UInt16 Error;
		public UInt16 Keysize;
		public byte[] Keydata;
		public UInt16 Othersize;
		public byte[] Otherdata;

		public RecordTkey(RecordReader rr)
		{
			Algorithm = rr.ReadDomainName();
			Inception = rr.ReadUInt32();
			Expiration = rr.ReadUInt32();
			Mode = rr.ReadUInt16();
			Error = rr.ReadUInt16();
			Keysize = rr.ReadUInt16();
			Keydata = rr.ReadBytes(Keysize);
			Othersize = rr.ReadUInt16();
			Otherdata = rr.ReadBytes(Othersize);
		}

		public override string ToString()
		{
			return $"{Algorithm} {Inception} {Expiration} {Mode} {Error}";
		}

	}
}
