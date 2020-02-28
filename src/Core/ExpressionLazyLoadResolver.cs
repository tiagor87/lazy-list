using System;
using System.Threading.Tasks;

namespace LazyList.Core
{
    public class ExpressionLazyLoadResolver<T> : LazyLoadResolver<T> where T : class
    {
        private readonly Func<LazyLoadParameter, Task<T>> _resolveAsync;

        public ExpressionLazyLoadResolver(Func<LazyLoadParameter, Task<T>> resolveAsync)
        {
            _resolveAsync = resolveAsync;
        }

        protected override async Task<T> LoadAsync(LazyLoadParameter parameter)
        {
            return await _resolveAsync(parameter);
        }
    }
}