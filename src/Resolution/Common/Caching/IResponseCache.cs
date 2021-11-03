using Resolution.Protocol;

namespace Resolution.Common.Caching
{
    public interface IResponseCache
    {
        bool Clear();
        bool Delete(Question question);
        Response Get(Question question);
        bool Set(Question question, Response response);
        int Size { get; }
    }
}