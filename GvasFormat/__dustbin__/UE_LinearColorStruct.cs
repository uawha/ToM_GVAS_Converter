using System.Diagnostics;
using System.IO;
using Newtonsoft.Json;

namespace GvasFormat.Serialization.UETypes
{
    [DebuggerDisplay("R = {R}, G = {G}, B = {B}, A = {A}", Name = "{Name}")]
    [JsonObject(MemberSerialization.OptIn)]
    public sealed class UE_LinearColorStruct : UE_Struct
    {
        [JsonProperty("R", Required = Required.Always, Order = 100)]
        public float R;

        [JsonProperty("G", Required = Required.Always, Order = 101)]
        public float G;

        [JsonProperty("B", Required = Required.Always, Order = 102)]
        public float B;

        [JsonProperty("A", Required = Required.Always, Order = 103)]
        public float A;

        UE_LinearColorStruct() : base() { StructTypeString = UE_StructValueTypeString.LinearColor; }

        public UE_LinearColorStruct(BinaryReader reader) : this()
        {
            R = reader.ReadSingle();
            G = reader.ReadSingle();
            B = reader.ReadSingle();
            A = reader.ReadSingle();
        }
    }
}