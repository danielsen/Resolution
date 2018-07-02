using System;

/*
 * http://www.ietf.org/rfc/rfc2845.txt
 * 
 * Field Name       Data Type      Notes
      --------------------------------------------------------------
      Algorithm Name   domain-name    Name of the algorithm
                                      in domain name syntax.
      Time Signed      u_int48_t      seconds since 1-Jan-70 UTC.
      Fudge            u_int16_t      seconds of error permitted
                                      in Time Signed.
      MAC Size         u_int16_t      number of octets in MAC.
      MAC              octet stream   defined by Algorithm Name.
      Original ID      u_int16_t      original message ID
      Error            u_int16_t      expanded RCODE covering
                                      TSIG processing.
      Other Len        u_int16_t      length, in octets, of
                                      Other Data.
      Other Data       octet stream   empty unless Error == BADTIME

 */

namespace Resolution.Protocol.Records
{
	public class RecordTsig : Record
	{
		public string Algorithmname;
		public long Timesigned;
		public UInt16 Fudge;
		public UInt16 Macsize;
		public byte[] Mac;
		public UInt16 Originalid;
		public UInt16 Error;
		public UInt16 Otherlen;
		public byte[] Otherdata;

		public RecordTsig(RecordReader rr)
		{
			Algorithmname = rr.ReadDomainName();
			Timesigned = rr.ReadUInt32() << 32 | rr.ReadUInt32();
			Fudge = rr.ReadUInt16();
			Macsize = rr.ReadUInt16();
			Mac = rr.ReadBytes(Macsize);
			Originalid = rr.ReadUInt16();
			Error = rr.ReadUInt16();
			Otherlen = rr.ReadUInt16();
			Otherdata = rr.ReadBytes(Otherlen);
		}

		public override string ToString()
		{
			DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0);
			dateTime = dateTime.AddSeconds(Timesigned);
			string printDate = dateTime.ToShortDateString() + " " + dateTime.ToShortTimeString();
			return $"{Algorithmname} {printDate} {Fudge} {Originalid} {Error}";
		}

	}
}
