using System.Collections.Generic;
using System.Linq;

namespace LazyList.Core
{
    public class LazyListFactory : ILazyLoadListFactory
    {
        private readonly IReadOnlyList<ILazyLoadResolver> _resolvers;

        public LazyListFactory(IEnumerable<ILazyLoadResolver> resolvers)
        {
            _resolvers = resolvers.ToList();
        }

        public IList<T> Create<T>(LazyLoadParameter parameter)
        {
            var resolver = _resolvers.FirstOrDefault(x => x.ResolveType == typeof(T));
            return new LazyList<T>(resolver, parameter);
        }
    }
}