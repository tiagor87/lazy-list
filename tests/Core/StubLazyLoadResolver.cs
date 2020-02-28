using System;
using System.Threading.Tasks;
using LazyList.Core;

namespace LazyList.Tests.Core
{
    public class StubLazyLoadResolver : ILazyLoadResolver
    {
        public Type ResolveType { get; }
        public Task<object> ResolveAsync(LazyLoadParameter parameter)
        {
            throw new NotImplementedException();
        }

        public object Resolve(LazyLoadParameter parameter)
        {
            throw new NotImplementedException();
        }
    }
}