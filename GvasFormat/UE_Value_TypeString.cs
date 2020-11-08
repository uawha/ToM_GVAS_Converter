namespace GvasFormat
{
    /// <summary>
    /// NOTE. Do not confuse yourself by their naming. A "BoolProperty" is really used as a type identifier.
    /// A "property", in normal sense, has a name, and a value of some type.
    /// </summary>
    public static class UE_Value_TypeString
    {
        public const string BoolProperty = "BoolProperty";
        public const string IntProperty = "IntProperty";
        public const string FloatProperty = "FloatProperty";
        public const string NameProperty = "NameProperty";
        //public const string StrProperty = "StrProperty"; // (not used)
        //public const string TextProperty = "TextProperty"; // (not used)
        public const string EnumProperty = "EnumProperty";
        public const string StructProperty = "StructProperty";
        public const string ArrayProperty = "ArrayProperty";
        public const string MapProperty = "MapProperty";
        public const string ByteProperty = "ByteProperty";

        public static class Struct
        {
            //public const string DateTime = "DateTime";
            //public const string Guid = "Guid";
            public const string Vector = "Vector";
            public const string Rotator = "Rotator";
            //public const string LinearColor = "LinearColor";
            public const string Quat = "Quat";
        }
    }
}
