using System.Collections.Generic;

namespace GvasFormat
{
    public sealed class UE_Map : UE_Value
    {
        public string KeyType;

        public string ValueType;

        public int Count;

        public List<UE_Map_KeyValuePair> Map;

        public UE_Map(string type_string, string key_type, string value_type, int count, List<UE_Map_KeyValuePair> content)
        {
            this.TypeString = type_string;
            this.KeyType = key_type;
            this.ValueType = value_type;
            this.Count = count;
            this.Map = content;
        }
    }
}