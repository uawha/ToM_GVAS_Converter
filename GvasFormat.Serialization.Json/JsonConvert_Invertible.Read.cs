using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Newtonsoft.Json;

namespace GvasFormat.Serialization.Json
{
    public partial class JsonConvert_Invertible
    {
        public static class Read
        {
            public static GvasFormat.Gvas Gvas(JsonReader reader)
            {
                reader.AssertReadToken(JsonToken.StartObject);

                reader.AssertReadPropertyName(JsonNaming.Gvas.Header);
                GvasFormat.SaveGameHeader header = Read.SaveGameHeader(reader);

                reader.AssertReadPropertyName(JsonNaming.Gvas.Content);
                List<UE_Property> content = Read.Object_as_PropertyList(reader);

                reader.AssertReadToken(JsonToken.EndObject);

                return new Gvas()
                {
                    Header = header,
                    PropertyList = content
                };
            }

            static GvasFormat.SaveGameHeader SaveGameHeader(JsonReader reader)
            {
                var _R = new GvasFormat.SaveGameHeader();
                reader.AssertReadToken(JsonToken.StartObject);
                _R.FileTypeTag = reader.AssertReadPropertyNameValue<int>(JsonNaming.SaveGameHeader.FileTypeTag);
                _R.SaveGameFileVersion = reader.AssertReadPropertyNameValue<int>(JsonNaming.SaveGameHeader.SaveGameFileVersion);
                _R.PackageFileUE4Version = reader.AssertReadPropertyNameValue<int>(JsonNaming.SaveGameHeader.PackageFileUE4Version);
                reader.AssertReadPropertyName(JsonNaming.SaveGameHeader.SavedEngineVersion);
                {
                    _R.SavedEngineVersion = new EngineVersion();
                    reader.AssertReadToken(JsonToken.StartObject);
                    _R.SavedEngineVersion.Major = reader.AssertReadPropertyNameValue<UInt16>(JsonNaming.SaveGameHeader.EngineVersion.Major);
                    _R.SavedEngineVersion.Minor = reader.AssertReadPropertyNameValue<UInt16>(JsonNaming.SaveGameHeader.EngineVersion.Minor);
                    _R.SavedEngineVersion.Patch = reader.AssertReadPropertyNameValue<UInt16>(JsonNaming.SaveGameHeader.EngineVersion.Patch);
                    _R.SavedEngineVersion.ChangeList = reader.AssertReadPropertyNameValue<UInt32>(JsonNaming.SaveGameHeader.EngineVersion.ChangeList);
                    _R.SavedEngineVersion.Branch = reader.AssertReadPropertyNameValue<string>(JsonNaming.SaveGameHeader.EngineVersion.Branch);
                    reader.AssertReadToken(JsonToken.EndObject);
                }
                _R.CustomVersionFormat = reader.AssertReadPropertyNameValue<int>(JsonNaming.SaveGameHeader.CustomVersionFormat);
                reader.AssertReadPropertyName(JsonNaming.SaveGameHeader.CustomVersions);
                {
                    reader.AssertReadToken(JsonToken.StartObject);
                    reader.AssertReadPropertyNameValue<int>(JsonNaming.SaveGameHeader.CustomVersionContainer.Count);
                    reader.AssertReadPropertyName(JsonNaming.SaveGameHeader.CustomVersionContainer.Versions);
                    var version_list = new List<CustomVersion>();
                    reader.AssertReadToken(JsonToken.StartArray);
                    while (true)
                    {
                        var token = reader.AssertRead();
                        if (token == JsonToken.StartObject)
                        {
                            string key_string = reader.AssertReadPropertyNameValue<string>(JsonNaming.SaveGameHeader.CustomVersion.Key);
                            Guid key = Guid.Parse(key_string);
                            int version = reader.AssertReadPropertyNameValue<int>(JsonNaming.SaveGameHeader.CustomVersion.Version);
                            reader.AssertReadToken(JsonToken.EndObject);
                            var _ver = new CustomVersion();
                            _ver.Key = key;
                            _ver.Version = version;
                            version_list.Add(_ver);
                        }
                        else if (token == JsonToken.EndArray)
                        {
                            break;
                        }
                        else
                        {
                            throw new FormatException();
                        }
                    }
                    reader.AssertReadToken(JsonToken.EndObject);
                    _R.CustomVersions = new CustomVersionContainer();
                    _R.CustomVersions.Count = version_list.Count;
                    _R.CustomVersions.Versions = version_list.ToArray();
                }
                _R.SaveGameClassName = reader.AssertReadPropertyNameValue<string>(JsonNaming.SaveGameHeader.SaveGameClassName);
                reader.AssertReadToken(JsonToken.EndObject);
                return _R;
            }

            static List<UE_Property> Object_as_PropertyList(JsonReader reader)
            {
                var _R = new List<UE_Property>();
                reader.AssertReadToken(JsonToken.StartObject);
                while (true)
                {
                    var token = reader.AssertRead();
                    if (token == JsonToken.PropertyName)
                    {
                        string property_name = reader.Value.ToString();
                        GvasFormat.UE_Value value = UE_Value(reader);
                        UE_Property property = new UE_Property()
                        {
                            Name = property_name,
                            Value = value
                        };
                        _R.Add(property);
                    }
                    else if (token == JsonToken.EndObject)
                    {
                        break;
                    }
                    else
                    {
                        throw new FormatException();
                    }
                }
                return _R;
            }

            static GvasFormat.UE_Value UE_Value(JsonReader reader, bool skip_start_token = false)
            {
                if (!skip_start_token)
                {
                    reader.AssertReadToken(JsonToken.StartObject);
                }
                string type_string = reader.AssertReadPropertyNameValue<string>(JsonNaming.UE_Value.TypeString);
                // code structure copied from GvasFormat.Serialization.Binary.UE_Property_Reader.Read_UE_Value
                GvasFormat.UE_Value result;
                switch (type_string)
                {
                    //
                    // non-container
                    case UE_Value_TypeString.BoolProperty:
                        bool value_bool = reader.AssertReadPropertyNameValue<bool>(JsonNaming.UE_Value.Value);
                        result = new UE_<bool>(type_string, value_bool);
                        break;
                    case UE_Value_TypeString.ByteProperty:
                        byte value_byte = reader.AssertReadPropertyNameValue<byte>(JsonNaming.UE_Value.Value);
                        result = new UE_<byte>(type_string, value_byte);
                        break;
                    case UE_Value_TypeString.EnumProperty:
                        string enum_type = reader.AssertReadPropertyNameValue<string>(JsonNaming.UE_Enum.EnumType);
                        string enum_value = reader.AssertReadPropertyNameValue<string>(JsonNaming.UE_Value.Value);
                        result = new UE_Enum(type_string, enum_type, enum_value);
                        break;
                    case UE_Value_TypeString.FloatProperty:
                        float value_float = reader.AssertReadPropertyNameValue<float>(JsonNaming.UE_Value.Value);
                        result = new UE_<float>(type_string, value_float);
                        break;
                    case UE_Value_TypeString.IntProperty:
                        int value_int = reader.AssertReadPropertyNameValue<int>(JsonNaming.UE_Value.Value);
                        result = new UE_<int>(type_string, value_int);
                        break;
                    case UE_Value_TypeString.NameProperty:
                        //case UE_ValueTypeString.StrProperty:
                        string value_string = reader.AssertReadPropertyNameValue<string>(JsonNaming.UE_Value.Value);
                        result = new UE_<string>(type_string, value_string);
                        break;
                    //
                    // container
                    // redirect.
                    // UE_Struct ... lots of them are not engine defined, but game author defined.
                    case UE_Value_TypeString.StructProperty:
                        string struct_type = reader.AssertReadPropertyNameValue<string>(JsonNaming.UE_Struct.StructTypeString);
                        reader.AssertReadPropertyName(JsonNaming.UE_Value.Value);
                        switch (struct_type)
                        {
                            case UE_Value_TypeString.Struct.Vector:
                            case UE_Value_TypeString.Struct.Rotator:
                                reader.AssertReadToken(JsonToken.StartArray);
                                float x = reader.AssertReadValue<float>();
                                float y = reader.AssertReadValue<float>();
                                float z = reader.AssertReadValue<float>();
                                reader.AssertReadToken(JsonToken.EndArray);
                                result = new UE_Struct_Vector(struct_type, x, y, z);
                                break;
                            case UE_Value_TypeString.Struct.Quat:
                                reader.AssertReadToken(JsonToken.StartArray);
                                float a = reader.AssertReadValue<float>();
                                float b = reader.AssertReadValue<float>();
                                float c = reader.AssertReadValue<float>();
                                float d = reader.AssertReadValue<float>();
                                reader.AssertReadToken(JsonToken.EndArray);
                                result = new UE_Struct_Quat(struct_type, a, b, c, d);
                                break;
                            default:
                                List<UE_Property> property_list = Read.Object_as_PropertyList(reader);
                                result = new UE_Struct_Generic(struct_type, property_list);
                                break;
                        }
                        break;
                    case UE_Value_TypeString.ArrayProperty:
                        string item_type = reader.AssertReadPropertyNameValue<string>(JsonNaming.UE_Array.ItemType);
                        reader.AssertReadPropertyNameValue<int>(JsonNaming.UE_Map.Count);
                        List<GvasFormat.UE_Value> item_list;
                        if (item_type == UE_Value_TypeString.StructProperty)
                        {
                            string sa_name = reader.AssertReadPropertyNameValue<string>(JsonNaming.UE_StructArray.SA_Name);
                            string sa_item_type = reader.AssertReadPropertyNameValue<string>(JsonNaming.UE_StructArray.SA_ItemType);
                            string sa_struct_type_string = reader.AssertReadPropertyNameValue<string>(JsonNaming.UE_StructArray.SA_StructTypeString);
                            reader.AssertReadPropertyName(JsonNaming.UE_Array.ItemList);
                            item_list = Read.UE_Value_List(reader);
                            result = new UE_StructArray(type_string, item_type, item_list.Count, item_list.ToArray(), sa_name, sa_item_type, sa_struct_type_string);
                        }
                        else
                        {
                            reader.AssertReadPropertyName(JsonNaming.UE_Array.ItemList);
                            item_list = Read.UE_Value_List(reader);
                            result = new UE_Array(type_string, item_type, item_list.Count, item_list.ToArray());
                        }
                        break;
                    case UE_Value_TypeString.MapProperty:
                        string key_type = reader.AssertReadPropertyNameValue<string>(JsonNaming.UE_Map.KeyType);
                        string value_type = reader.AssertReadPropertyNameValue<string>(JsonNaming.UE_Map.ValueType);
                        reader.AssertReadPropertyNameValue<int>(JsonNaming.UE_Map.Count);
                        reader.AssertReadPropertyName(JsonNaming.UE_Map.Map);
                        reader.AssertReadToken(JsonToken.StartArray);
                        var map = new List<UE_Map_KeyValuePair>();
                        while (true)
                        {
                            var token = reader.AssertRead();
                            if (token == JsonToken.StartObject)
                            {
                                reader.AssertReadPropertyName(JsonNaming.UE_Map_KeyValuePair.Key);
                                UE_Value key = Read.UE_Value(reader);
                                reader.AssertReadPropertyName(JsonNaming.UE_Map_KeyValuePair.Value);
                                UE_Value value = Read.UE_Value(reader);
                                reader.AssertReadToken(JsonToken.EndObject);
                                UE_Map_KeyValuePair kvp = new UE_Map_KeyValuePair(key, value);
                                map.Add(kvp);
                            }
                            else if (token == JsonToken.EndArray)
                            {
                                break;
                            }
                            else
                            {
                                throw new FormatException();
                            }
                        }
                        result = new UE_Map(type_string, key_type, value_type, map.Count, map);
                        break;
                    default:
                        throw new FormatException($"Unknown value type `{type_string}`.");
                }
                reader.AssertReadToken(JsonToken.EndObject);
                return result;
            }

            static List<GvasFormat.UE_Value> UE_Value_List(JsonReader reader)
            {
                var _R = new List<GvasFormat.UE_Value>();
                reader.AssertReadToken(JsonToken.StartArray);
                while (true)
                {
                    var token = reader.AssertRead();
                    if (token == JsonToken.StartObject)
                    {
                        _R.Add(Read.UE_Value(reader, true));
                    }
                    else if (token == JsonToken.EndArray)
                    {
                        break;
                    }
                    else
                    {
                        throw new FormatException();
                    }
                }
                return _R;
            }
        }
    }
}
