namespace LazyList.Core
{
    public class LazyLoadParameter
    {
        public object Value { get; }

        public LazyLoadParameter(object value)
        {
            Value = value;
        }
        
        public static LazyLoadParameter Null => new LazyLoadParameter(null);
        
        public static implicit operator LazyLoadParameter(int value) => new LazyLoadParameter(value);
        public static implicit operator LazyLoadParameter(string value) => new LazyLoadParameter(value);
        public static implicit operator LazyLoadParameter(long value) => new LazyLoadParameter(value);

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj is LazyLoadParameter parameter && Equals(Value, parameter.Value);
        }

        public override int GetHashCode()
        {
            return Value == null ? 0 : Value.GetHashCode();
        }
    }
}