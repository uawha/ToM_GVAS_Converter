using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace GvasFormat.Serialization.Json
{
    public partial class JsonConvert_Invertible
    {
        public static class Write
        {
            public static void Gvas(JsonWriter writer, GvasFormat.Gvas gvas)
            {
                writer.WriteStartObject();

                writer.WritePropertyName(JsonNaming.Gvas.Header);
                Write.SaveGameHeader(writer, gvas.Header);

                writer.WritePropertyName(JsonNaming.Gvas.Content);
                Write.PropertyList_as_Object(writer, gvas.PropertyList);

                writer.WriteEndObject();
            }

            static void SaveGameHeader(JsonWriter writer, SaveGameHeader header)
            {
                writer.WriteStartObject();

                writer.WritePropertyName(JsonNaming.SaveGameHeader.FileTypeTag);
                writer.WriteValue(header.FileTypeTag);

                writer.WritePropertyName(JsonNaming.SaveGameHeader.SaveGameFileVersion);
                writer.WriteValue(header.SaveGameFileVersion);

                writer.WritePropertyName(JsonNaming.SaveGameHeader.PackageFileUE4Version);
                writer.WriteValue(header.PackageFileUE4Version);

                writer.WritePropertyName(JsonNaming.SaveGameHeader.SavedEngineVersion);
                {
                    writer.WriteStartObject();

                    writer.WritePropertyName(JsonNaming.SaveGameHeader.EngineVersion.Major);
                    writer.WriteValue(header.SavedEngineVersion.Major);

                    writer.WritePropertyName(JsonNaming.SaveGameHeader.EngineVersion.Minor);
                    writer.WriteValue(header.SavedEngineVersion.Minor);

                    writer.WritePropertyName(JsonNaming.SaveGameHeader.EngineVersion.Patch);
                    writer.WriteValue(header.SavedEngineVersion.Patch);

                    writer.WritePropertyName(JsonNaming.SaveGameHeader.EngineVersion.ChangeList);
                    writer.WriteValue(header.SavedEngineVersion.ChangeList);

                    writer.WritePropertyName(JsonNaming.SaveGameHeader.EngineVersion.Branch);
                    writer.WriteValue(header.SavedEngineVersion.Branch);

                    writer.WriteEndObject();
                }

                writer.WritePropertyName(JsonNaming.SaveGameHeader.CustomVersionFormat);
                writer.WriteValue(header.CustomVersionFormat);

                writer.WritePropertyName(JsonNaming.SaveGameHeader.CustomVersions);
                {
                    writer.WriteStartObject();

                    writer.WritePropertyName(JsonNaming.SaveGameHeader.CustomVersionContainer.Count);
                    writer.WriteValue(header.CustomVersions.Count);

                    writer.WritePropertyName(JsonNaming.SaveGameHeader.CustomVersionContainer.Versions);
                    {
                        writer.WriteStartArray();

                        foreach (CustomVersion version in header.CustomVersions.Versions)
                        {
                            writer.WriteStartObject();

                            writer.WritePropertyName(JsonNaming.SaveGameHeader.CustomVersion.Key);
                            writer.WriteValue(version.Key.ToString());

                            writer.WritePropertyName(JsonNaming.SaveGameHeader.CustomVersion.Version);
                            writer.WriteValue(version.Version);

                            writer.WriteEndObject();
                        }

                        writer.WriteEndArray();
                    }

                    writer.WriteEndObject();
                }

                writer.WritePropertyName(JsonNaming.SaveGameHeader.SaveGameClassName);
                writer.WriteValue(header.SaveGameClassName);

                writer.WriteEndObject();
            }

            static void PropertyList_as_Object(JsonWriter writer, IEnumerable<UE_Property> property_list)
            {
                writer.WriteStartObject();

                foreach (UE_Property property in property_list)
                {
                    writer.WritePropertyName(property.Name);
                    Write.UE_Value(writer, property.Value);
                }

                writer.WriteEndObject();
            }

            static void UE_Value(JsonWriter writer, GvasFormat.UE_Value value)
            {
                writer.WriteStartObject();

                writer.WritePropertyName(JsonNaming.UE_Value.TypeString);
                writer.WriteValue(value.TypeString);

                Type value_cs_type = value.GetType();
                // Non-container Type
                if (value_cs_type == typeof(UE_<bool>))
                {
                    writer.WritePropertyName(JsonNaming.UE_Value.Value);
                    writer.WriteValue(((UE_<bool>)value).Value);
                }
                else if (value_cs_type == typeof(UE_<byte>))
                {
                    writer.WritePropertyName(JsonNaming.UE_Value.Value);
                    writer.WriteValue(((UE_<byte>)value).Value);
                }
                else if (value_cs_type == typeof(UE_Enum))
                {
                    UE_Enum value_enum = (UE_Enum)value;

                    writer.WritePropertyName(JsonNaming.UE_Enum.EnumType);
                    writer.WriteValue(value_enum.EnumType);

                    writer.WritePropertyName(JsonNaming.UE_Value.Value);
                    writer.WriteValue(value_enum.Value);
                }
                else if (value_cs_type == typeof(UE_<float>))
                {
                    writer.WritePropertyName(JsonNaming.UE_Value.Value);
                    writer.WriteValue(((UE_<float>)value).Value);
                }
                else if (value_cs_type == typeof(UE_<int>))
                {
                    writer.WritePropertyName(JsonNaming.UE_Value.Value);
                    writer.WriteValue(((UE_<int>)value).Value);
                }
                else if (value_cs_type == typeof(UE_<string>))
                {
                    writer.WritePropertyName(JsonNaming.UE_Value.Value);
                    writer.WriteValue(((UE_<string>)value).Value);
                }
                // Container Type
                else if (value_cs_type == typeof(UE_Map))
                {
                    UE_Map value_map = (UE_Map)value;

                    writer.WritePropertyName(JsonNaming.UE_Map.KeyType);
                    writer.WriteValue(value_map.KeyType);

                    writer.WritePropertyName(JsonNaming.UE_Map.ValueType);
                    writer.WriteValue(value_map.ValueType);

                    writer.WritePropertyName(JsonNaming.UE_Map.Count);
                    writer.WriteValue(value_map.Count);

                    writer.WritePropertyName(JsonNaming.UE_Map.Map);
                    {
                        writer.WriteStartArray();

                        foreach (UE_Map_KeyValuePair keyValuePair in value_map.Map)
                        {
                            writer.WriteStartObject();

                            writer.WritePropertyName(JsonNaming.UE_Map_KeyValuePair.Key);
                            Write.UE_Value(writer, keyValuePair.Key);

                            writer.WritePropertyName(JsonNaming.UE_Map_KeyValuePair.Value);
                            Write.UE_Value(writer, keyValuePair.Value);

                            writer.WriteEndObject();
                        }

                        writer.WriteEndArray();
                    }
                }
                else if (value_cs_type == typeof(UE_Array))
                {
                    UE_Array value_array = (UE_Array)value;

                    writer.WritePropertyName(JsonNaming.UE_Array.ItemType);
                    writer.WriteValue(value_array.ItemType);

                    writer.WritePropertyName(JsonNaming.UE_Array.Count);
                    writer.WriteValue(value_array.Count);

                    writer.WritePropertyName(JsonNaming.UE_Array.ItemList);
                    {
                        writer.WriteStartArray();

                        foreach (UE_Value item in value_array.ItemList)
                        {
                            Write.UE_Value(writer, item);
                        }

                        writer.WriteEndArray();
                    }
                }
                else if (value_cs_type == typeof(UE_StructArray))
                {
                    UE_Array value_array = (UE_Array)value;

                    writer.WritePropertyName(JsonNaming.UE_Array.ItemType);
                    writer.WriteValue(value_array.ItemType);

                    writer.WritePropertyName(JsonNaming.UE_Array.Count);
                    writer.WriteValue(value_array.Count);

                    UE_StructArray value_struct_array = (UE_StructArray)value;

                    writer.WritePropertyName(JsonNaming.UE_StructArray.SA_Name);
                    writer.WriteValue(value_struct_array.SA_Name);

                    writer.WritePropertyName(JsonNaming.UE_StructArray.SA_ItemType);
                    writer.WriteValue(value_struct_array.SA_ItemType);

                    writer.WritePropertyName(JsonNaming.UE_StructArray.SA_StructTypeString);
                    writer.WriteValue(value_struct_array.SA_StructTypeString);

                    writer.WritePropertyName(JsonNaming.UE_Array.ItemList);
                    {
                        writer.WriteStartArray();

                        foreach (UE_Value item in value_array.ItemList)
                        {
                            Write.UE_Value(writer, item);
                        }

                        writer.WriteEndArray();
                    }
                }
                // Struct Type
                else if (value_cs_type == typeof(UE_Struct_Vector))
                {
                    UE_Struct value_struct = (UE_Struct)value;

                    writer.WritePropertyName(JsonNaming.UE_Struct.StructTypeString);
                    writer.WriteValue(value_struct.StructTypeString);

                    writer.WritePropertyName(JsonNaming.UE_Value.Value);
                    UE_Struct_Vector value_struct_vector = (UE_Struct_Vector)value;
                    {
                        writer.WriteStartArray();

                        writer.WriteValue(value_struct_vector.X);
                        writer.WriteValue(value_struct_vector.Y);
                        writer.WriteValue(value_struct_vector.Z);

                        writer.WriteEndArray();
                    }
                }
                else if (value_cs_type == typeof(UE_Struct_Quat))
                {
                    UE_Struct value_struct = (UE_Struct)value;

                    writer.WritePropertyName(JsonNaming.UE_Struct.StructTypeString);
                    writer.WriteValue(value_struct.StructTypeString);

                    writer.WritePropertyName(JsonNaming.UE_Value.Value);
                    UE_Struct_Quat value_struct_quat = (UE_Struct_Quat)value;
                    {
                        writer.WriteStartArray();

                        writer.WriteValue(value_struct_quat.A);
                        writer.WriteValue(value_struct_quat.B);
                        writer.WriteValue(value_struct_quat.C);
                        writer.WriteValue(value_struct_quat.D);

                        writer.WriteEndArray();
                    }
                }
                else if (value_cs_type == typeof(UE_Struct_Generic))
                {
                    UE_Struct value_struct = (UE_Struct)value;

                    writer.WritePropertyName(JsonNaming.UE_Struct.StructTypeString);
                    writer.WriteValue(value_struct.StructTypeString);

                    writer.WritePropertyName(JsonNaming.UE_Value.Value);
                    UE_Struct_Generic value_struct_generic = (UE_Struct_Generic)value;
                    Write.PropertyList_as_Object(writer, value_struct_generic.PropertyList);
                }
                else
                {
                    throw new NotImplementedException(value_cs_type.ToString());
                }

                writer.WriteEndObject();
            }
        }
    }
}
