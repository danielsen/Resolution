using System.Collections.Generic;
using System.Linq;
using Resolution.Protocol;

namespace Resolution.Common.Caching
{
    internal class ResponseCache : IResponseCache
    {
        private readonly IDictionary<Question, ResponseCacheItem> _cache;

        public int Size { get; }

        public ResponseCache(int size = 1000)
        {
            _cache = new Dictionary<Question, ResponseCacheItem>(size);
            Size = size;
        }

        private void PushOldest()
        {
            var item = _cache.Values.OrderBy(x => x.LastUsedUtc)
                .FirstOrDefault();
            if (item != null)
                _cache.Remove(item.Question);
        }

        public bool Clear()
        {
            lock (_cache)
            {
                _cache.Clear();
            }

            return true;
        }

        public bool Delete(Question question)
        {
            lock (_cache)
            {
                return _cache.Remove(question);
            }
        }

        public Response Get(Question question)
        {
            lock (_cache)
            {
                var isCached = _cache.TryGetValue(question, out ResponseCacheItem response);

                if (!isCached)
                    return null;

                response.SetLastUse();
                _cache[question] = response;

                return response.Response;
            }
        }

        public bool Set(Question question, Response response)
        {
            var item = new ResponseCacheItem(response.Server.Address, question, response);

            lock (_cache)
            {
                if (_cache.ContainsKey(question))
                {
                    _cache[question] = item;
                    return true;
                }

                if (_cache.Count >= Size)
                    PushOldest();

                try
                {
                    _cache.Add(question, item);
                }
                catch
                {
                    return false;
                }

                return true;
            }
        }
    }
}