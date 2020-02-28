using System.Collections;
using System.Collections.Generic;

namespace LazyList.Core
{
    public class LazyList<T> : IList<T>
    {
        private object _sync = new object();
        private bool _isLoaded;
        private readonly List<T> _list;
        private readonly ILazyLoadResolver _resolver;
        private readonly LazyLoadParameter _parameter;

        public LazyList(ILazyLoadResolver resolver, LazyLoadParameter parameter = null)
        {
            _resolver = resolver;
            _parameter = parameter ?? LazyLoadParameter.Null;
            _list = new List<T>();
            _isLoaded = false;
        }
        public IEnumerator<T> GetEnumerator()
        {
            LoadData();
            return _list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(T item)
        {
            _list.Add(item);
        }

        public void Clear()
        {
            LoadData();
            _list.Clear();
        }

        public bool Contains(T item)
        {
            LoadData();
            return _list.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            LoadData();
            _list.CopyTo(array, arrayIndex);
        }

        public bool Remove(T item)
        {
            LoadData();
            return _list.Remove(item);
        }

        public int Count
        {
            get
            {
                LoadData();
                return _list.Count;
            }
        }

        public bool IsReadOnly => false;
        public int IndexOf(T item)
        {
            LoadData();
            return _list.IndexOf(item);
        }

        public void Insert(int index, T item)
        {
            LoadData();
            _list.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            LoadData();
            _list.RemoveAt(index);
        }

        public T this[int index]
        {
            get
            {
                LoadData();
                return _list[index];
            }
            set
            {
                LoadData();
                _list[index] = value;
            }
        }

        private void LoadData()
        {
            if (_isLoaded || _resolver == null) return;
            lock (_sync)
            {
                if (_isLoaded) return;
                var list = (IEnumerable<T>) _resolver.Resolve(_parameter);
                _list.InsertRange(0, list);
                _isLoaded = true;
            }
        }
    }
}
