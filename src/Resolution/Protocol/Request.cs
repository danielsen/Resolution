using System.Collections.Generic;

namespace Resolution.Protocol
{
    public class Request
    {
        public Header Header;

        private readonly List<Question> _questions;

        public Request()
        {
            Header = new Header();
            Header.Opcode = OperationCode.Query;
            Header.Qdcount = 0;

            _questions = new List<Question>();
        }

        public void AddQuestion(Question question)
        {
            _questions.Add(question);
        }

        public byte[] Data
        {
            get
            {
                List<byte> data = new List<byte>();
                Header.Qdcount = (ushort) _questions.Count;
                data.AddRange(Header.Data);
                foreach (Question q in _questions)
                    data.AddRange(q.Data);
                return data.ToArray();
            }

        }
    }
}
