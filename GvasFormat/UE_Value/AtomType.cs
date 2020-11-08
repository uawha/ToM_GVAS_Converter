namespace GvasFormat
{
    public sealed class UE_<T> : UE_Value
    {
        public T Value;

        public UE_(string type_string, T value)
        {
            this.TypeString = type_string;
            this.Value = value;
        }
    }
}
