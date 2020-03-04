using System;
using System.Threading.Tasks;

namespace LazyList.Core
{
    public class ExpressionLazyLoadResolver<T> : LazyLoadResolver where T : class
    {
        private readonly Func<LazyLoadParameter, Task<T>> _resolveAsync;

        public ExpressionLazyLoadResolver(Func<LazyLoadParameter, Task<T>> resolveAsync) : base(typeof(T))
        {
            _resolveAsync = resolveAsync;
        }

        protected override async Task<object> LoadAsync(LazyLoadParameter parameter)
        {
            return await _resolveAsync(parameter);
        }
    }
}