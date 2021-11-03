using System;
using System.Collections.Generic;
using TransportType = Resolution.Protocol.TransportType;

namespace Resolution.Common
{
    internal class ResolverContextBuilder
    {
        private ResolverContext _resolverContext;
        private IList<Func<ResolverContext, ResolverContext>> _buildSteps;

        public ResolverContextBuilder()
        {
            Init();
        }

        private void Init()
        {
            _resolverContext = new ResolverContext();
            _buildSteps = new List<Func<ResolverContext, ResolverContext>>();
        }

        public ResolverContextBuilder AddDnsServer(string ipAddress, int port = 53)
        {
            _buildSteps.Add(x =>
            {
                x = x.AddDnsServer(ipAddress, port);
                return x;
            });
            return this;
        }

        public ResolverContext Build()
        {
            foreach (var step in _buildSteps)
            {
                step(_resolverContext);
            }

            return _resolverContext;
        }

        public ResolverContextBuilder CacheSize(int value)
        {
            _buildSteps.Add(x =>
            {
                x = x.SetCacheSize(value);
                return x;
            });
            return this;
        }

        public void Clear()
        {
            Init();
        }

        public ResolverContextBuilder Retries(int value)
        {
            _buildSteps.Add(x =>
            {
                x = x.SetRetries(value);
                return x;
            });
            return this;
        }

        public ResolverContextBuilder Timeout(int value)
        {
            _buildSteps.Add(x =>
            {
                x = x.SetTimeout(value);
                return x;
            });
            return this;
        }

        public ResolverContextBuilder TransportType(TransportType value)
        {
            _buildSteps.Add(x =>
            {
                x = x.SetTransportType(value);
                return x;
            });
            return this;
        }

        public ResolverContextBuilder UseCache(bool value)
        {
            _buildSteps.Add(x =>
            {
                x = x.SetUseCache(value);
                return x;
            });
            return this;
        }

        public ResolverContextBuilder UseRecursion(bool value)
        {
            _buildSteps.Add(x =>
            {
                x = x.SetUseRecursion(value);
                return x;
            });
            return this;
        }
    }
}