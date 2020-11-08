namespace GvasFormat
{
    public sealed class UE_None : UE_Property
    {
        public string None = Identifier;

        public const string Identifier = "None";

        UE_None() { }
        static UE_None __static_value__ = new UE_None() { };
        public static UE_None Get() => __static_value__;
    }
}