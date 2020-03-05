using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;

namespace LazyList.Core
{
    public class LazyListFactory : ILazyListFactory
    {
        private static Func<IServiceProvider> _serviceProviderGetter;
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

        public static IList<T> CreateList<T>(object parameter)
        {
            if (ServiceProvider == null) throw new InvalidOperationException("Service Provider is required to execute static creation. Call Init method on Startup.");
            
            var factory = ServiceProvider.GetRequiredService<ILazyListFactory>();
            return factory.Create<T>(new LazyLoadParameter(parameter));
        }

        public static void Init(Func<IServiceProvider> serviceProviderGetter)
        {
            _serviceProviderGetter = serviceProviderGetter;
            _serviceProvider = null;
        }

        private static IServiceProvider ServiceProvider
        {
            get
            {
                if (_serviceProvider != null) return _serviceProvider;
                if (_serviceProviderGetter == null) return null;
                return _serviceProvider = _serviceProviderGetter();
            }
        }
    }
}