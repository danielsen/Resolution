using System;
using System.Collections.Generic;
using System.Net;
using Resolution.Protocol.Records;

namespace Resolution.Protocol
{
    public class Response
    {
        /// <summary>
        /// List of Question records
        /// </summary>
        public List<Question> Questions;

        /// <summary>
        /// List of AnswerRr records
        /// </summary>
        public List<AnswerRr> Answers;

        /// <summary>
        /// List of AuthorityRr records
        /// </summary>
        public List<AuthorityRr> Authorities;

        /// <summary>
        /// List of AdditionalRr records
        /// </summary>
        public List<AdditionalRr> Additionals;

        public Header Header;

        /// <summary>
        /// Error message, empty when no error
        /// </summary>
        public string Error;

        /// <summary>
        /// The Size of the message
        /// </summary>
        public int MessageSize;

        /// <summary>
        /// TimeStamp when cached
        /// </summary>
        public DateTime TimeStamp;

        /// <summary>
        /// Server which delivered this response
        /// </summary>
        public IPEndPoint Server;

        public Response()
        {
            Questions = new List<Question>();
            Answers = new List<AnswerRr>();
            Authorities = new List<AuthorityRr>();
            Additionals = new List<AdditionalRr>();

            Server = new IPEndPoint(0, 0);
            Error = "";
            MessageSize = 0;
            TimeStamp = DateTime.Now;
            Header = new Header();
        }

        public Response(IPEndPoint iPEndPoint, byte[] data)
        {
            Error = "";
            Server = iPEndPoint;
            TimeStamp = DateTime.Now;
            MessageSize = data.Length;
            var rr = new RecordReader(data);

            Questions = new List<Question>();
            Answers = new List<AnswerRr>();
            Authorities = new List<AuthorityRr>();
            Additionals = new List<AdditionalRr>();

            Header = new Header(rr);

            for (var intI = 0; intI < Header.Qdcount; intI++)
            {
                Questions.Add(new Question(rr));
            }

            for (var intI = 0; intI < Header.Ancount; intI++)
            {
                Answers.Add(new AnswerRr(rr));
            }

            for (var intI = 0; intI < Header.Nscount; intI++)
            {
                Authorities.Add(new AuthorityRr(rr));
            }

            for (var intI = 0; intI < Header.Arcount; intI++)
            {
                Additionals.Add(new AdditionalRr(rr));
            }
        }

        private IEnumerable<T> GetAnswers<T>()
        {
            var list = new List<T>();
            foreach (var answerRr in Answers)
            {
                if (answerRr.Record.GetType() == typeof(T))
                {
                    list.Add((T)Convert.ChangeType(answerRr.Record, typeof(T)));
                }
            }

            return list;
        }
        /// <summary>
        /// List of RecordMx in Response.Answers
        /// </summary>
        public IEnumerable<RecordMx> RecordsMx
        {
            get
            {
                var list = new List<RecordMx>();
                foreach (var answerRr in Answers)
                {
                    if (answerRr.Record is RecordMx record)
                        list.Add(record);
                }

                list.Sort();
                return list;
            }
        }

        /// <summary>
        /// List of RecordTxt in Response.Answers
        /// </summary>
        public IEnumerable<RecordTxt> RecordsTxt => GetAnswers<RecordTxt>();

        /// <summary>
        /// List of RecordA in Response.Answers
        /// </summary>
        public IEnumerable<RecordA> RecordsA
        {
            get
            {
                var list = new List<RecordA>();
                foreach (var answerRr in Answers)
                {
                    if (answerRr.Record is RecordA record)
                        list.Add(record);
                }

                return list;
            }
        }

        /// <summary>
        /// List of RecordPtr in Response.Answers
        /// </summary>
        public IEnumerable<RecordPtr> RecordsPtr
        {
            get
            {
                var list = new List<RecordPtr>();
                foreach (var answerRr in Answers)
                {
                    if (answerRr.Record is RecordPtr record)
                        list.Add(record);
                }

                return list;
            }
        }

        /// <summary>
        /// List of RecordCname in Response.Answers
        /// </summary>
        public IEnumerable<RecordCname> RecordsCname
        {
            get
            {
                var list = new List<RecordCname>();
                foreach (var answerRr in Answers)
                {
                    if (answerRr.Record is RecordCname record)
                        list.Add(record);
                }

                return list;
            }
        }

        /// <summary>
        /// List of RecordAaaa in Response.Answers
        /// </summary>
        public IEnumerable<RecordAaaa> RecordsAaaa
        {
            get
            {
                var list = new List<RecordAaaa>();
                foreach (var answerRr in Answers)
                {
                    if (answerRr.Record is RecordAaaa record)
                        list.Add(record);
                }

                return list;
            }
        }

        /// <summary>
        /// List of RecordNs in Response.Answers
        /// </summary>
        public IEnumerable<RecordNs> RecordsNs
        {
            get
            {
                var list = new List<RecordNs>();
                foreach (var answerRr in Answers)
                {
                    if (answerRr.Record is RecordNs record)
                        list.Add(record);
                }

                return list;
            }
        }

        /// <summary>
        /// List of RecordSoa in Response.Answers
        /// </summary>
        public IEnumerable<RecordSoa> RecordsSoa
        {
            get
            {
                var list = new List<RecordSoa>();
                foreach (var answerRr in Answers)
                {
                    if (answerRr.Record is RecordSoa record)
                        list.Add(record);
                }

                return list;
            }
        }

        /// <summary>
        /// List of RecordCert in Response.Answers
        /// </summary>
        public IEnumerable<RecordCert> RecordsCert
        {
            get
            {
                var list = new List<RecordCert>();
                foreach (var answerRr in Answers)
                {
                    if (answerRr.Record is RecordCert record)
                        list.Add(record);
                }

                return list;
            }
        }

        public IEnumerable<RecordSrv> RecordsSrv
        {
            get
            {
                var list = new List<RecordSrv>();
                foreach (var answerRr in Answers)
                {
                    if (answerRr.Record is RecordSrv record)
                        list.Add(record);
                }

                return list;
            }
        }

        public IEnumerable<Rr> RecordsRr
        {
            get
            {
                var list = new List<Rr>();
                foreach (AnswerRr rr in Answers)
                {
                    list.Add(rr);
                }

                foreach (AnswerRr rr in Answers)
                {
                    list.Add(rr);
                }

                foreach (AuthorityRr rr in Authorities)
                {
                    list.Add(rr);
                }

                foreach (AdditionalRr rr in Additionals)
                {
                    list.Add(rr);
                }

                return list;
            }
        }
    }

}
