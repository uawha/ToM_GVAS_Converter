using System;
using System.Diagnostics;
using System.IO;
using Newtonsoft.Json;

namespace GvasFormat.Serialization.UETypes
{
    [DebuggerDisplay("{Value}", Name = "{Name}")]
    [JsonObject(MemberSerialization.OptIn)]
    public sealed class UE_DateTimeStruct : UE_Struct
    {
        [JsonProperty("value", Required = Required.Always, Order = 100)]
        public DateTime Value;

        UE_DateTimeStruct() : base() { StructTypeString = UE_StructValueTypeString.DateTime; }

        public UE_DateTimeStruct(BinaryReader reader) : this()
        {
            Value = DateTime.FromBinary(reader.ReadInt64());
        }
    }
}