using System;

namespace Resolution.Protocol.Records
{
    public class NotUsed : Attribute
    {
        public override string ToString()
        {
            return "not-used";
        }
    }
}
