using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;

namespace LazyList.Core
{
    public class LazyListFactory : ILazyListFactory
    {
        private static IServiceProvider _serviceProvider;
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

        public static IList<T> CreateList<T>(LazyLoadParameter parameter)
        {
            if (_serviceProvider == null) throw new InvalidOperationException("Service Provider is required to execute static creation. Call Init method on Startup.");
            
            var factory = _serviceProvider.GetRequiredService<ILazyListFactory>();
            return factory.Create<T>(parameter);
        }

        public static void Init(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
    }
}