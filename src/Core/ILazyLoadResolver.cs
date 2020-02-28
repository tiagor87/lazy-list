using System;
using System.Threading.Tasks;

namespace LazyList.Core
{
    public interface ILazyLoadResolver
    {
        Type ResolveType { get; }
        Task<object> ResolveAsync(LazyLoadParameter parameter);
        object Resolve(LazyLoadParameter parameter);
    }
}