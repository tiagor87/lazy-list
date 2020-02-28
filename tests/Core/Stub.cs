namespace LazyList.Tests.Core
{
    public class Stub
    {
        private static readonly object _sync = new object();
        private static int _instance; 

        public Stub()
        {
            lock (_sync)
            {
                _instance++;
                Instance = _instance;
            }
        }
        public int Instance { get; }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            if (ReferenceEquals(this, obj)) return true;
            
            return obj is Stub stub &&
                   Instance == stub.Instance;
        }

        public override int GetHashCode()
        {
            return Instance.GetHashCode();
        }
    }
}