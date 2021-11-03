using System;
using System.Collections.Generic;
using System.Net;

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
        public List<AnswerResourceRecord> Answers;

        /// <summary>
        /// List of AuthorityRr records
        /// </summary>
        public List<AuthorityResourceRecord> Authorities;

        /// <summary>
        /// List of AdditionalRr records
        /// </summary>
        public List<AdditionalResourceRecord> Additionals;

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
            Answers = new List<AnswerResourceRecord>();
            Authorities = new List<AuthorityResourceRecord>();
            Additionals = new List<AdditionalResourceRecord>();

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
            Answers = new List<AnswerResourceRecord>();
            Authorities = new List<AuthorityResourceRecord>();
            Additionals = new List<AdditionalResourceRecord>();

            Header = new Header(rr);

            for (var intI = 0; intI < Header.Qdcount; intI++)
            {
                Questions.Add(new Question(rr));
            }

            for (var intI = 0; intI < Header.Ancount; intI++)
            {
                Answers.Add(new AnswerResourceRecord(rr));
            }

            for (var intI = 0; intI < Header.Nscount; intI++)
            {
                Authorities.Add(new AuthorityResourceRecord(rr));
            }

            for (var intI = 0; intI < Header.Arcount; intI++)
            {
                Additionals.Add(new AdditionalResourceRecord(rr));
            }
        }

        public IEnumerable<T> GetAnswers<T>()
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
    }
}
