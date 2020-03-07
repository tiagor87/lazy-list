using System;
using System.Collections.Generic;

namespace LazyList.Core
{
    public class LazyListFactory : ILazyListFactory, IDisposable
    {
        private static ILazyListFactory _instance;
        private readonly IDisposable _scope;
        private readonly IServiceProvider _provider;
        private bool _disposed;

        public LazyListFactory(IDisposable scope, IServiceProvider provider)
        {
            _scope = scope;
            _provider = provider;
        }

        public IList<T> Create<T>(LazyLoadParameter parameter)
        {
            var resolver = (ILazyLoadResolver<IEnumerable<T>>) _provider.GetService(typeof(ILazyLoadResolver<IEnumerable<T>>));
            return new LazyList<T>(resolver, parameter);
        }

        public static IList<T> CreateList<T>(object parameter)
        {
            if (_instance == null) throw new InvalidOperationException($"There is not instance for {nameof(ILazyListFactory)}.");
            return _instance.Create<T>(new LazyLoadParameter(parameter));
        }

        public static void RegisterInstance(ILazyListFactory lazyListFactory)
        {
            _instance = lazyListFactory;
        }

        ~LazyListFactory()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (_disposed) return;
            if (disposing) _scope.Dispose();
            _disposed = true;
        }
    }
}