using System.Collections.Generic;
using System.Text;

#region Rfc info
/*
3.3.14. TXT RDATA format

    +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
    /                   TXT-DATA                    /
    +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+

where:

TXT-DATA        One or more <character-string>s.

TXT RRs are used to hold descriptive text.  The semantics of the text
depends on the domain where it is found.
 * 
*/
#endregion

namespace Resolution.Protocol.Records
{
	public class RecordTxt : Record
	{
		public List<string> Txt;

		public RecordTxt(RecordReader rr, int length)
		{
			int pos = rr.Position;
			Txt = new List<string>();
			while ((rr.Position - pos) < length)
				Txt.Add(rr.ReadString());
		}

		public override string ToString()
		{
			StringBuilder sb = new StringBuilder();
            		foreach (string txt in Txt)
                		sb.Append(txt);
			return sb.ToString().TrimEnd();
		}
	}
}
