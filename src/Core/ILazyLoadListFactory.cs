using System.Collections.Generic;

namespace LazyList.Core
{
    public interface ILazyLoadListFactory
    {
        IList<T> Create<T>(LazyLoadParameter parameter);
    }
}