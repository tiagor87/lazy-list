using System.Threading.Tasks;

namespace LazyList.Core
{
    public interface ILazyLoadFactory
    {
        T Resolve<T>(LazyLoadParameter parameter);
        Task<T> ResolveAsync<T>(LazyLoadParameter parameter);
    }
}