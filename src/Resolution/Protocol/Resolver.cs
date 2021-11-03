using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using Resolution.Common;
using Resolution.Common.Caching;
using Resolution.Protocol.Records;

namespace Resolution.Protocol
{
    /// <summary>
	/// Resolver is the main class to do DNS query lookups
	/// </summary>
	public class Resolver
    {
        private readonly IResponseCache _cache = null;
        private ushort _unique;
        
        internal Resolver(ResolverContext context)
        {
            Retries = context.Retries;
            TimeOut = context.Timeout;
            TransportType = context.TransportType;
            Recursion = context.UseRecursion;
            DnsServers = context.DnsServers.Count == 0 ? GetDnsServers() : context.DnsServers;
            UseCache = context.UseCache;

            if (context.UseCache)
                _cache = new ResponseCache(context.CacheSize);
        }
        
        /// <summary>
        /// Version of this set of routines, when not in a library
        /// </summary>
        public string Version => Assembly.GetExecutingAssembly().GetName().Version.ToString();

        /// <summary>
        /// Default DNS port
        /// </summary>
        public const int DefaultPort = 53;
        
        public int Retries { get; }

        public IList<IPEndPoint> DnsServers { get; }
        
        public bool UseCache { get; }

        public class VerboseOutputEventArgs : EventArgs
        {
            public string Message;
            public VerboseOutputEventArgs(string message)
            {
                Message = message;
            }
        }

        private void Verbose(string format, params object[] args)
        {
            OnVerbose?.Invoke(this, new VerboseEventArgs(string.Format(format, args)));
        }

        /// <summary>
        /// Verbose messages from internal operations
        /// </summary>
        public event VerboseEventHandler OnVerbose;
        public delegate void VerboseEventHandler(object sender, VerboseEventArgs e);

        public class VerboseEventArgs : EventArgs
        {
            public string Message { get; }
            public VerboseEventArgs(string message)
            {
                Message = message;
            }
        }

        /// <summary>
        /// Gets or sets timeout in milliseconds
        /// </summary>
        public int TimeOut { get; set; }

        /// <summary>
        /// Gets or set recursion for doing queries
        /// </summary>
        public bool Recursion { get; }

        /// <summary>
        /// Gets or sets protocol to use
        /// </summary>
        public TransportType TransportType { get; }

        /// <summary>
        /// Gets first DNS server address or sets single DNS server to use
        /// </summary>
        public string DnsServer => DnsServers[0].Address.ToString();

        private Response UdpRequest(Request request)
        {
            // RFC1035 max. size of a UDP datagram is 512 bytes
            byte[] responseMessage = new byte[512];

            for (int intAttempts = 0; intAttempts < Retries; intAttempts++)
            {
                for (int intDnsServer = 0; intDnsServer < DnsServers.Count; intDnsServer++)
                {
                    Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                    socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveTimeout, TimeOut * 1000);

                    try
                    {
                        socket.SendTo(request.Data, DnsServers[intDnsServer]);
                        int intReceived = socket.Receive(responseMessage);
                        byte[] data = new byte[intReceived];
                        Array.Copy(responseMessage, data, intReceived);
                        Response response = new Response(DnsServers[intDnsServer], data);
                        AddToCache(response);
                        return response;
                    }
                    catch (SocketException)
                    {
                        Verbose($";; Connection to nameserver {(intDnsServer + 1)} failed");
                    }
                    finally
                    {
                        _unique++;
                        socket.Close();
                    }
                }
            }

            Response responseTimeout = new Response {Error = "Timeout Error"};
            return responseTimeout;
        }

        private Response TcpRequest(Request request)
        {
            byte[] responseMessage = new byte[512];

            for (int intAttempts = 0; intAttempts < Retries; intAttempts++)
            {
                for (int intDnsServer = 0; intDnsServer < DnsServers.Count; intDnsServer++)
                {
                    TcpClient tcpClient = new TcpClient {ReceiveTimeout = TimeOut * 1000};

                    try
                    {
                        IAsyncResult result = tcpClient.BeginConnect(DnsServers[intDnsServer].Address, DnsServers[intDnsServer].Port, null, null);

                        bool success = result.AsyncWaitHandle.WaitOne(TimeOut * 1000, true);

                        if (!success || !tcpClient.Connected)
                        {
                            tcpClient.Close();
                            Verbose($";; Connection to nameserver {(intDnsServer + 1)} failed");
                            continue;
                        }

                        BufferedStream bs = new BufferedStream(tcpClient.GetStream());

                        byte[] data = request.Data;
                        bs.WriteByte((byte)((data.Length >> 8) & 0xff));
                        bs.WriteByte((byte)(data.Length & 0xff));
                        bs.Write(data, 0, data.Length);
                        bs.Flush();

                        Response transferResponse = new Response();
                        int intSoa = 0;
                        int intMessageSize = 0;

                        while (true)
                        {
                            int intLength = bs.ReadByte() << 8 | bs.ReadByte();
                            if (intLength <= 0)
                            {
                                tcpClient.Close();
                                Verbose($";; Connection to nameserver {(intDnsServer + 1)} failed");
                                throw new SocketException(); // next try
                            }

                            intMessageSize += intLength;

                            data = new byte[intLength];
                            bs.Read(data, 0, intLength);
                            Response response = new Response(DnsServers[intDnsServer], data);

                            if (response.Header.Rcode != ResponseCode.NoError)
                                return response;

                            if (response.Questions[0].QuestionType != QuestionType.Axfr)
                            {
                                AddToCache(response);
                                return response;
                            }

                            // Zone transfer!!

                            if (transferResponse.Questions.Count == 0)
                                transferResponse.Questions.AddRange(response.Questions);
                            transferResponse.Answers.AddRange(response.Answers);
                            transferResponse.Authorities.AddRange(response.Authorities);
                            transferResponse.Additionals.AddRange(response.Additionals);

                            if (response.Answers[0].Type == Type.Soa)
                                intSoa++;

                            if (intSoa != 2)
                                continue;
                            transferResponse.Header.Qdcount = (ushort)transferResponse.Questions.Count;
                            transferResponse.Header.Ancount = (ushort)transferResponse.Answers.Count;
                            transferResponse.Header.Nscount = (ushort)transferResponse.Authorities.Count;
                            transferResponse.Header.Arcount = (ushort)transferResponse.Additionals.Count;
                            transferResponse.MessageSize = intMessageSize;
                            return transferResponse;
                        }
                    } // try
                    catch (SocketException)
                    {
                    }
                    finally
                    {
                        _unique++;

                        // close the socket
                        tcpClient.Close();
                    }
                }
            }

            Response responseTimeout = new Response {Error = "Timeout Error"};
            return responseTimeout;
        }

        private Response CacheLookup(Question question)
        {
            if (!UseCache)
                return null;

            Response response = _cache.Get(question);

            if (response == null)
                return null;

            return response.Answers.Any(x => x.IsExpired(response.TimeStamp)) ? null : response;
        }

        private void AddToCache(Response response)
        {
            if (!UseCache)
                return;

            if (response.Questions.Count == 0)
                return;

            var question = response.Questions[0];
            _cache.Set(question, response);
        }

        public void ClearCache()
        {
            if (!UseCache)
                return;

            _cache.Clear();
        }

        public Response GetCachedResponse(string domainName, QuestionType questionType,
            QuestionClass questionClass = QuestionClass.In)
        {   
            var question = new Question(domainName, questionType, questionClass);
            return CacheLookup(question);
        }

        public void DeleteCache(string domainName, QuestionType questionType,
            QuestionClass questionClass = QuestionClass.In)
        {
            var question = new Question(domainName, questionType, questionClass);
            _cache.Delete(question);
        }

        /// <summary>
        /// Do Query on specified DNS servers
        /// </summary>
        /// <param name="name">Name to query</param>
        /// <param name="qtype">Question type</param>
        /// <param name="qclass">Class type</param>
        /// <returns>Response of the query</returns>
        public Response Query(string name, QuestionType qtype, QuestionClass qclass = QuestionClass.In)
        {
            Question question = new Question(name, qtype, qclass);
            var cacheResponse = CacheLookup(question);
            if (cacheResponse != null)
                return cacheResponse;

            Request request = new Request();
            request.AddQuestion(question);
            return GetResponse(request);
        }

        public static Response Query(string name, QuestionType questionType, string dnsServer = null,
            QuestionClass @class = QuestionClass.In)
        {
            var builder = new ResolverContextBuilder();
            if (!string.IsNullOrEmpty(dnsServer))
                builder.AddDnsServer(dnsServer);

            var resolver = new Resolver(builder.Build());
            return resolver.Query(name, questionType, @class);
        }

        private Response GetResponse(Request request)
        {
            request.Header.Id = _unique;
            request.Header.Rd = Recursion;

            switch (TransportType)
            {
                case TransportType.Udp:
                    return UdpRequest(request);
                case TransportType.Tcp:
                    return TcpRequest(request);
            }

            Response response = new Response {Error = "Unknown TransportType"};
            return response;
        }

        /// <summary>
        /// Gets a list of default DNS servers used on the Windows machine.
        /// </summary>
        /// <returns></returns>
        public static IList<IPEndPoint> GetDnsServers()
        {
            List<IPEndPoint> list = new List<IPEndPoint>();

            NetworkInterface[] adapters = NetworkInterface.GetAllNetworkInterfaces();
            foreach (NetworkInterface n in adapters)
            {
                if (n.OperationalStatus != OperationalStatus.Up) continue;
                IPInterfaceProperties ipProps = n.GetIPProperties();
                foreach (IPAddress ipAddr in ipProps.DnsAddresses)
                {
                    IPEndPoint entry = new IPEndPoint(ipAddr, DefaultPort);
                    if (!list.Contains(entry))
                        list.Add(entry);
                }
            }
            return list;
        }

        /// <summary>
        /// Translates the IPV4 or IPV6 address into an arpa address
        /// </summary>
        /// <param name="ip">IP address to get the arpa address form</param>
        /// <returns>The 'mirrored' IPV4 or IPV6 arpa address</returns>
        public static string GetArpaFromIp(IPAddress ip)
        {
            if (ip.AddressFamily == AddressFamily.InterNetwork)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("in-addr.arpa.");
                foreach (byte b in ip.GetAddressBytes())
                {
                    sb.Insert(0, $"{b}.");
                }
                return sb.ToString();
            }
            if (ip.AddressFamily == AddressFamily.InterNetworkV6)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("ip6.arpa.");
                foreach (byte b in ip.GetAddressBytes())
                {
                    sb.Insert(0, $"{(b >> 4) & 0xf:x}.");
                    sb.Insert(0, $"{(b >> 0) & 0xf:x}.");
                }
                return sb.ToString();
            }
            return "?";
        }

        public static string GetArpaFromEnum(string strEnum)
        {
            StringBuilder sb = new StringBuilder();
            string number = Regex.Replace(strEnum, "[^0-9]", "");
            sb.Append("e164.arpa.");
            foreach (char c in number)
            {
                sb.Insert(0, $"{c}.");
            }
            return sb.ToString();
        }

        private enum RrRecordStatus
        {
            Unknown,
            Name,
            Ttl,
            Class,
            Type,
            Value
        }

        public void LoadRootFile(string strPath)
        {
            StreamReader sr = new StreamReader(strPath);
            while (!sr.EndOfStream)
            {
                string strLine = sr.ReadLine();
                if (strLine == null)
                    break;
                int intI = strLine.IndexOf(';');
                if (intI >= 0)
                    strLine = strLine.Substring(0, intI);
                strLine = strLine.Trim();
                if (strLine.Length == 0)
                    continue;
                RrRecordStatus status = RrRecordStatus.Name;
                string name = "";
                string ttl = "";
                string Class = "";
                string type = "";
                string value = "";
                string strW = "";
                for (intI = 0; intI < strLine.Length; intI++)
                {
                    char c = strLine[intI];

                    if (c <= ' ' && strW != "")
                    {
                        switch (status)
                        {
                            case RrRecordStatus.Name:
                                name = strW;
                                status = RrRecordStatus.Ttl;
                                break;
                            case RrRecordStatus.Ttl:
                                ttl = strW;
                                status = RrRecordStatus.Class;
                                break;
                            case RrRecordStatus.Class:
                                Class = strW;
                                status = RrRecordStatus.Type;
                                break;
                            case RrRecordStatus.Type:
                                type = strW;
                                status = RrRecordStatus.Value;
                                break;
                            case RrRecordStatus.Value:
                                value = strW;
                                status = RrRecordStatus.Unknown;
                                break;
                            default:
                                break;
                        }
                        strW = "";
                    }
                    if (c > ' ')
                        strW += c;
                }
            }
            sr.Close();
        }
    }
}
