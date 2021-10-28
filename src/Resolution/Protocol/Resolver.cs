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

namespace Resolution.Protocol
{
    /// <summary>
	/// Resolver is the main class to do DNS query lookups
	/// </summary>
	public class Resolver
    {
        /// <summary>
        /// Version of this set of routines, when not in a library
        /// </summary>
        public string Version => Assembly.GetExecutingAssembly().GetName().Version.ToString();

        /// <summary>
        /// Default DNS port
        /// </summary>
        public const int DefaultPort = 53;

        /// <summary>
        /// Gets list of OPENDNS servers
        /// </summary>
        public static readonly IPEndPoint[] DefaultDnsServers =
        {
            new IPEndPoint(IPAddress.Parse("208.67.222.222"), DefaultPort),
            new IPEndPoint(IPAddress.Parse("208.67.220.220"), DefaultPort)
        };

        private ushort _mUnique;
        private int _mRetries;

        private readonly List<IPEndPoint> _mDnsServers;

        /// <summary>
        /// Constructor of Resolver using DNS servers specified.
        /// </summary>
        /// <param name="dnsServers">Set of DNS servers</param>
        public Resolver(IPEndPoint[] dnsServers)
        {
            _mDnsServers = new List<IPEndPoint>();
            _mDnsServers.AddRange(dnsServers);

            _mUnique = (ushort)(new Random()).Next();
            _mRetries = 3;
            TimeOut = 1;
            Recursion = true;
            TransportType = TransportType.Udp;
        }

        /// <summary>
        /// Constructor of Resolver using DNS server specified.
        /// </summary>
        /// <param name="dnsServer">DNS server to use</param>
        public Resolver(IPEndPoint dnsServer)
            : this(new[] { dnsServer })
        {
        }

        /// <summary>
        /// Constructor of Resolver using DNS server and port specified.
        /// </summary>
        /// <param name="serverIpAddress">DNS server to use</param>
        /// <param name="serverPortNumber">DNS port to use</param>
        public Resolver(IPAddress serverIpAddress, int serverPortNumber)
            : this(new IPEndPoint(serverIpAddress, serverPortNumber))
        {
        }

        /// <summary>
        /// Constructor of Resolver using DNS address and port specified.
        /// </summary>
        /// <param name="serverIpAddress">DNS server address to use</param>
        /// <param name="serverPortNumber">DNS port to use</param>
        public Resolver(string serverIpAddress, int serverPortNumber)
            : this(IPAddress.Parse(serverIpAddress), serverPortNumber)
        {
        }

        /// <summary>
        /// Constructor of Resolver using DNS address.
        /// </summary>
        /// <param name="serverIpAddress">DNS server address to use</param>
        public Resolver(string serverIpAddress)
            : this(IPAddress.Parse(serverIpAddress), DefaultPort)
        {
        }

        /// <summary>
        /// Resolver constructor, using DNS servers specified by Windows
        /// </summary>
        public Resolver()
            : this(GetDnsServers())
        {
        }

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
            public string Message;
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
        /// Gets or sets number of retries before giving up
        /// </summary>
        public int Retries
        {
            get => _mRetries;
            set
            {
                if (value >= 1)
                    _mRetries = value;
            }
        }

        /// <summary>
        /// Gets or set recursion for doing queries
        /// </summary>
        public bool Recursion { get; set; }

        /// <summary>
        /// Gets or sets protocol to use
        /// </summary>
        public TransportType TransportType { get; set; }

        /// <summary>
        /// Gets or sets list of DNS servers to use
        /// </summary>
        public IPEndPoint[] DnsServers
        {
            get => _mDnsServers.ToArray();
            set
            {
                _mDnsServers.Clear();
                _mDnsServers.AddRange(value);
            }
        }

        /// <summary>
        /// Gets first DNS server address or sets single DNS server to use
        /// </summary>
        public string DnsServer
        {
            get => _mDnsServers[0].Address.ToString();
            set
            {
                if (IPAddress.TryParse(value, out var ip))
                {
                    _mDnsServers.Clear();
                    _mDnsServers.Add(new IPEndPoint(ip, DefaultPort));
                    return;
                }
                Response response = Query(value, QType.A);
                if (response.RecordsA.Any())
                {
                    _mDnsServers.Clear();
                    _mDnsServers.Add(new IPEndPoint(response.RecordsA.First().Address, DefaultPort));
                }
            }
        }

        private Response UdpRequest(Request request)
        {
            // RFC1035 max. size of a UDP datagram is 512 bytes
            byte[] responseMessage = new byte[512];

            for (int intAttempts = 0; intAttempts < _mRetries; intAttempts++)
            {
                for (int intDnsServer = 0; intDnsServer < _mDnsServers.Count; intDnsServer++)
                {
                    Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                    socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveTimeout, TimeOut * 1000);

                    try
                    {
                        socket.SendTo(request.Data, _mDnsServers[intDnsServer]);
                        int intReceived = socket.Receive(responseMessage);
                        byte[] data = new byte[intReceived];
                        Array.Copy(responseMessage, data, intReceived);
                        Response response = new Response(_mDnsServers[intDnsServer], data);
                        return response;
                    }
                    catch (SocketException)
                    {
                        Verbose($";; Connection to nameserver {(intDnsServer + 1)} failed");
                    }
                    finally
                    {
                        _mUnique++;
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

            for (int intAttempts = 0; intAttempts < _mRetries; intAttempts++)
            {
                for (int intDnsServer = 0; intDnsServer < _mDnsServers.Count; intDnsServer++)
                {
                    TcpClient tcpClient = new TcpClient {ReceiveTimeout = TimeOut * 1000};

                    try
                    {
                        IAsyncResult result = tcpClient.BeginConnect(_mDnsServers[intDnsServer].Address, _mDnsServers[intDnsServer].Port, null, null);

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
                            Response response = new Response(_mDnsServers[intDnsServer], data);

                            if (response.Header.Rcode != RCode.NoError)
                                return response;

                            if (response.Questions[0].QType != QType.Axfr)
                            {
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
                        _mUnique++;

                        // close the socket
                        tcpClient.Close();
                    }
                }
            }

            Response responseTimeout = new Response {Error = "Timeout Error"};
            return responseTimeout;
        }

        /// <summary>
        /// Do Query on specified DNS servers
        /// </summary>
        /// <param name="name">Name to query</param>
        /// <param name="qtype">Question type</param>
        /// <param name="qclass">Class type</param>
        /// <returns>Response of the query</returns>
        public Response Query(string name, QType qtype, QClass qclass)
        {
            Question question = new Question(name, qtype, qclass);

            Request request = new Request();
            request.AddQuestion(question);
            return GetResponse(request);
        }

        /// <summary>
        /// Do an QClass=IN Query on specified DNS servers
        /// </summary>
        /// <param name="name">Name to query</param>
        /// <param name="qtype">Question type</param>
        /// <returns>Response of the query</returns>
        public Response Query(string name, QType qtype)
        {
            Question question = new Question(name, qtype, QClass.In);

            Request request = new Request();
            request.AddQuestion(question);
            return GetResponse(request);
        }

        private Response GetResponse(Request request)
        {
            request.Header.Id = _mUnique;
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
        public static IPEndPoint[] GetDnsServers()
        {
            List<IPEndPoint> list = new List<IPEndPoint>();

            NetworkInterface[] adapters = NetworkInterface.GetAllNetworkInterfaces();
            foreach (NetworkInterface n in adapters)
            {
                if (n.OperationalStatus == OperationalStatus.Up)
                {
                    IPInterfaceProperties ipProps = n.GetIPProperties();
                    foreach (IPAddress ipAddr in ipProps.DnsAddresses)
                    {
                        IPEndPoint entry = new IPEndPoint(ipAddr, DefaultPort);
                        if (!list.Contains(entry))
                            list.Add(entry);
                    }

                }
            }
            return list.ToArray();
        }

        private IPHostEntry MakeEntry(string hostName)
        {
            IPHostEntry entry = new IPHostEntry {HostName = hostName};


            Response response = Query(hostName, QType.A, QClass.In);

            // fill AddressList and aliases
            List<IPAddress> addressList = new List<IPAddress>();
            List<string> aliases = new List<string>();
            foreach (AnswerRr answerRr in response.Answers)
            {
                if (answerRr.Type == Type.A)
                {
                    addressList.Add(IPAddress.Parse((answerRr.Record.ToString())));
                    entry.HostName = answerRr.Name;
                }
                else
                {
                    if (answerRr.Type == Type.Cname)
                        aliases.Add(answerRr.Name);
                }
            }
            entry.AddressList = addressList.ToArray();
            entry.Aliases = aliases.ToArray();

            return entry;
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
