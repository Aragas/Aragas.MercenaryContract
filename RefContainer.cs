namespace Aragas
{
    public class RefContainer<T> where T : struct
    {
        public T Value { get; set; }

        public RefContainer(T value)
        {
            Value = value;
        }
    }
}