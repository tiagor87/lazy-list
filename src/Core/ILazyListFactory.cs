using System.Collections.Generic;

namespace LazyList.Core
{
    public interface ILazyListFactory
    {
        IList<T> Create<T>(LazyLoadParameter parameter);
    }
}