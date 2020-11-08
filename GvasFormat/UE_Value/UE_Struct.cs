namespace GvasFormat
{
    public abstract class UE_Struct : UE_Value
    {
        public string StructTypeString;

        protected UE_Struct() { TypeString = UE_Value_TypeString.StructProperty; }
    }
}