using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LazyList.Core
{
    public class LazyLoadFactory : ILazyLoadFactory
    {
        private readonly IReadOnlyList<ILazyLoadResolver> _resolvers;

        public LazyLoadFactory(IEnumerable<ILazyLoadResolver> resolvers)
        {
            _resolvers = resolvers.ToList();
        }

        public T Resolve<T>(LazyLoadParameter parameter)
        {
            var resolver = _resolvers.FirstOrDefault(x => x.ResolveType == typeof(T));
            if (resolver == null) return default;
            return (T) resolver.Resolve(parameter);
        }
        
        public async Task<T> ResolveAsync<T>(LazyLoadParameter parameter)
        {
            var resolver = _resolvers.FirstOrDefault(x => x.ResolveType == typeof(T));
            if (resolver == null) return default;
            return (T) await resolver.ResolveAsync(parameter);
        }
    }
}