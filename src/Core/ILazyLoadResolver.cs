using System.Threading.Tasks;

namespace LazyList.Core
{
    public interface ILazyLoadResolver<T>
    {
        Task<T> ResolveAsync(LazyLoadParameter parameter);
        T Resolve(LazyLoadParameter parameter);
    }
}