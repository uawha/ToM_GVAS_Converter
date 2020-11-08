namespace GvasFormat
{
    public sealed class UE_Enum : UE_Value
    {
        public string EnumType;

        public string Value;

        public UE_Enum(string type_string, string enum_type, string value)
        {
            this.TypeString = type_string;
            this.EnumType = enum_type;
            this.Value = value;
        }
    }
}