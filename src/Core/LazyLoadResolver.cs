using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LazyList.Core
{
    public abstract class LazyLoadResolver<T> : ILazyLoadResolver<T>
    {
        private readonly IDictionary<LazyLoadParameter, T> _resolvedObjects;

        protected LazyLoadResolver()
        {
            _resolvedObjects = new ConcurrentDictionary<LazyLoadParameter, T>();
        }
        public async Task<T> ResolveAsync(LazyLoadParameter parameter)
        {
            if (parameter == null) throw new ArgumentNullException(nameof(parameter));
            if (_resolvedObjects.TryGetValue(parameter, out var resolved)) return resolved;
            resolved = await LoadAsync(parameter);
            if (resolved != null) _resolvedObjects.Add(parameter, resolved);
            return resolved;
        }

        public T Resolve(LazyLoadParameter parameter)
        {
            return ResolveAsync(parameter).GetAwaiter().GetResult();
        }

        protected abstract Task<T> LoadAsync(LazyLoadParameter parameter);
    }
}