using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LazyList.Core
{
    public class LazyLoadResolver<T> : ILazyLoadResolver where T : class
    {
        private static readonly object _sync = new object();
        private readonly IDictionary<LazyLoadParameter, object> _resolvedObjects;
        private readonly Func<Task<T>> _resolveAsync;

        public LazyLoadResolver(Func<Task<T>> resolveAsync)
        {
            ResolveType = typeof(T);
            _resolvedObjects = new ConcurrentDictionary<LazyLoadParameter, object>();
            _resolveAsync = resolveAsync;
        }

        public Type ResolveType { get; }
        public Task<object> ResolveAsync(LazyLoadParameter parameter)
        {
            if (parameter == null) throw new ArgumentNullException(nameof(parameter));
            if (_resolvedObjects.TryGetValue(parameter, out var resolved)) return Task.FromResult(resolved);
            lock (_sync)
            {
                if (_resolvedObjects.TryGetValue(parameter, out resolved)) return Task.FromResult(resolved);
                resolved = _resolveAsync().GetAwaiter().GetResult();
                _resolvedObjects.Add(parameter, resolved);
                return Task.FromResult(resolved);
            }
        }

        public object Resolve(LazyLoadParameter parameter)
        {
            return ResolveAsync(parameter).GetAwaiter().GetResult();
        }
    }
}