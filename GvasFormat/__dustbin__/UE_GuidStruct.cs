using System;
using System.Diagnostics;
using System.IO;
using Newtonsoft.Json;

namespace GvasFormat.Serialization.UETypes
{
    [DebuggerDisplay("{Value}", Name = "{Name}")]
    [JsonObject(MemberSerialization.OptIn)]
    public sealed class UE_GuidStruct : UE_Struct
    {
        [JsonProperty("value", Required = Required.Always, Order = 100)]
        public Guid Value;

        UE_GuidStruct() : base() { StructTypeString = UE_StructValueTypeString.Guid; }

        public UE_GuidStruct(BinaryReader reader) : this()
        {
            Value = new Guid(reader.ReadBytes(16));
        }
    }
}