using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LazyList.Core
{
    public abstract class LazyLoadResolver<T> : ILazyLoadResolver where T : class
    {
        private static readonly object _sync = new object();
        private readonly IDictionary<LazyLoadParameter, object> _resolvedObjects;

        protected LazyLoadResolver()
        {
            ResolveType = typeof(T);
            _resolvedObjects = new ConcurrentDictionary<LazyLoadParameter, object>();
        }

        public Type ResolveType { get; }
        public Task<object> ResolveAsync(LazyLoadParameter parameter)
        {
            if (parameter == null) throw new ArgumentNullException(nameof(parameter));
            lock (_sync)
            {
                if (_resolvedObjects.TryGetValue(parameter, out var resolved)) return Task.FromResult(resolved);
                resolved = LoadAsync(parameter).GetAwaiter().GetResult();
                _resolvedObjects.Add(parameter, resolved);
                return Task.FromResult(resolved);
            }
        }

        public object Resolve(LazyLoadParameter parameter)
        {
            return ResolveAsync(parameter).GetAwaiter().GetResult();
        }

        protected abstract Task<T> LoadAsync(LazyLoadParameter parameter);
    }
}