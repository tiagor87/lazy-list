using System.Threading.Tasks;
using LazyList.Core;

namespace LazyList.Tests.Core
{
    public class StubLazyLoadResolver<T> : ILazyLoadResolver<T> where T : class
    {
        public Task<T> ResolveAsync(LazyLoadParameter parameter)
        {
            return Task.FromResult((T) null);
        }

        public T Resolve(LazyLoadParameter parameter)
        {
            return null;
        }
    }
    
    public class Stub2LazyLoadResolver : ILazyLoadResolver<Stub>
    {
        public Task<Stub> ResolveAsync(LazyLoadParameter parameter)
        {
            return Task.FromResult((Stub) null);
        }

        public Stub Resolve(LazyLoadParameter parameter)
        {
            return null;
        }
    }
}