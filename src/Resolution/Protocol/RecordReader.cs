using System;
using System.Collections.Generic;
using System.Text;
using Resolution.Protocol.Records;

namespace Resolution.Protocol
{
    public class RecordReader
    {
        private readonly byte[] _mData;

        public RecordReader(byte[] data)
        {
            _mData = data;
            Position = 0;
        }

        public int Position { get; set; }

        public RecordReader(byte[] data, int position)
        {
            _mData = data;
            Position = position;
        }


        public byte ReadByte()
        {
            return Position >= _mData.Length ? (byte) 0 : _mData[Position++];
        }

        public char ReadChar()
        {
            return (char)ReadByte();
        }

        public UInt16 ReadUInt16()
        {
            return (UInt16)(ReadByte() << 8 | ReadByte());
        }

        public UInt16 ReadUInt16(int offset)
        {
            Position += offset;
            return ReadUInt16();
        }

        public UInt32 ReadUInt32()
        {
            return (UInt32)(ReadUInt16() << 16 | ReadUInt16());
        }

        public string ReadDomainName()
        {
            StringBuilder name = new StringBuilder();
            int length = 0;

            // get  the length of the first label
            while ((length = ReadByte()) != 0)
            {
                // top 2 bits set denotes domain name compression and to reference elsewhere
                if ((length & 0xc0) == 0xc0)
                {
                    // work out the existing domain name, copy this pointer
                    RecordReader newRecordReader = new RecordReader(_mData, (length & 0x3f) << 8 | ReadByte());

                    name.Append(newRecordReader.ReadDomainName());
                    return name.ToString();
                }

                // if not using compression, copy a char at a time to the domain name
                while (length > 0)
                {
                    name.Append(ReadChar());
                    length--;
                }
                name.Append('.');
            }
            if (name.Length == 0)
                return ".";
            return name.ToString();
        }

        public string ReadString()
        {
            short length = ReadByte();

            StringBuilder name = new StringBuilder();
            for (int intI = 0; intI < length; intI++)
                name.Append(ReadChar());
            return name.ToString();
        }

        public byte[] ReadBytes(int intLength)
        {
            List<byte> list = new List<byte>();
            for (int intI = 0; intI < intLength; intI++)
                list.Add(ReadByte());
            return list.ToArray();
        }

        public Record ReadRecord(Type type, int length)
        {
            switch (type)
            {
                case Type.A:
                    return new RecordA(this);
                case Type.Ns:
                    return new RecordNs(this);
                case Type.Md:
                    return new RecordMd(this);
                case Type.Mf:
                    return new RecordMf(this);
                case Type.Cname:
                    return new RecordCname(this);
                case Type.Soa:
                    return new RecordSoa(this);
                case Type.Mb:
                    return new RecordMb(this);
                case Type.Mg:
                    return new RecordMg(this);
                case Type.Mr:
                    return new RecordMr(this);
                case Type.Null:
                    return new RecordNull(this);
                case Type.Wks:
                    return new RecordWks(this);
                case Type.Ptr:
                    return new RecordPtr(this);
                case Type.Hinfo:
                    return new RecordHinfo(this);
                case Type.Minfo:
                    return new RecordMinfo(this);
                case Type.Mx:
                    return new RecordMx(this);
                case Type.Txt:
                    return new RecordTxt(this, length);
                case Type.Rp:
                    return new RecordRp(this);
                case Type.Afsdb:
                    return new RecordAfsdb(this);
                case Type.X25:
                    return new RecordX25(this);
                case Type.Isdn:
                    return new RecordIsdn(this);
                case Type.Rt:
                    return new RecordRt(this);
                case Type.Nsap:
                    return new RecordNsap(this);
                case Type.Nsapptr:
                    return new RecordNsapptr(this);
                case Type.Sig:
                    return new RecordSig(this);
                case Type.Key:
                    return new RecordKey(this);
                case Type.Px:
                    return new RecordPx(this);
                case Type.Gpos:
                    return new RecordGpos(this);
                case Type.Aaaa:
                    return new RecordAaaa(this);
                case Type.Loc:
                    return new RecordLoc(this);
                case Type.Nxt:
                    return new RecordNxt(this);
                case Type.Eid:
                    return new RecordEid(this);
                case Type.Nimloc:
                    return new RecordNimloc(this);
                case Type.Srv:
                    return new RecordSrv(this);
                case Type.Atma:
                    return new RecordAtma(this);
                case Type.Naptr:
                    return new RecordNaptr(this);
                case Type.Kx:
                    return new RecordKx(this);
                case Type.Cert:
                    return new RecordCert(this);
                case Type.A6:
                    return new RecordA6(this);
                case Type.Dname:
                    return new RecordDname(this);
                case Type.Sink:
                    return new RecordSink(this);
                case Type.Opt:
                    return new RecordOpt(this);
                case Type.Apl:
                    return new RecordApl(this);
                case Type.Ds:
                    return new RecordDs(this);
                case Type.Sshfp:
                    return new RecordSshfp(this);
                case Type.Ipseckey:
                    return new RecordIpseckey(this);
                case Type.Rrsig:
                    return new RecordRrsig(this);
                case Type.Nsec:
                    return new RecordNsec(this);
                case Type.Dnskey:
                    return new RecordDnskey(this);
                case Type.Dhcid:
                    return new RecordDhcid(this);
                case Type.Nsec3:
                    return new RecordNsec3(this);
                case Type.Nsec3Param:
                    return new RecordNsec3Param(this);
                case Type.Hip:
                    return new RecordHip(this);
                case Type.Spf:
                    return new RecordSpf(this);
                case Type.Uinfo:
                    return new RecordUinfo(this);
                case Type.Uid:
                    return new RecordUid(this);
                case Type.Gid:
                    return new RecordGid(this);
                case Type.Unspec:
                    return new RecordUnspec(this);
                case Type.Tkey:
                    return new RecordTkey(this);
                case Type.Tsig:
                    return new RecordTsig(this);
                default:
                    return new RecordUnknown(this);
            }
        }

    }
}
