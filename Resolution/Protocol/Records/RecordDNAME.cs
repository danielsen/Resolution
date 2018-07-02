

/*
 * http://tools.ietf.org/rfc/rfc2672.txt
 * 
3. The DNAME Resource Record

   The DNAME RR has mnemonic DNAME and type code 39 (decimal).
   DNAME has the following format:

      <owner> <ttl> <class> DNAME <target>

   The format is not class-sensitive.  All fields are required.  The
   RDATA field <target> is a <domain-name> [DNSIS].

 * 
 */
namespace Resolution.Protocol.Records
{
	public class RecordDname : Record
	{
		public string Target;

		public RecordDname(RecordReader rr)
		{
			Target = rr.ReadDomainName();
		}

		public override string ToString()
		{
			return Target;
		}

	}
}
