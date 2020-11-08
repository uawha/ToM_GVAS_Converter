using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace GvasFormat.Serialization.Json
{
    public class JsonConvert_Compact
    {
        public static class Write
        {
            public static void Gvas(JsonWriter writer, GvasFormat.Gvas gvas)
            {
                //writer.WriteStartObject();

                //writer.WritePropertyName(JsonNaming.Gvas.Content);
                Write.PropertyList_as_Object(writer, gvas.PropertyList);

                //writer.WriteEndObject();
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
                Type value_cs_type = value.GetType();
                // Non-container Type
                if (value_cs_type == typeof(UE_<bool>))
                {
                    writer.WriteValue(((UE_<bool>)value).Value);
                }
                else if (value_cs_type == typeof(UE_<byte>))
                {
                    writer.WriteValue(((UE_<byte>)value).Value);
                }
                else if (value_cs_type == typeof(UE_Enum))
                {
                    UE_Enum value_enum = (UE_Enum)value;
                    writer.WriteValue(value_enum.Value);
                }
                else if (value_cs_type == typeof(UE_<float>))
                {
                    writer.WriteValue(((UE_<float>)value).Value);
                }
                else if (value_cs_type == typeof(UE_<int>))
                {
                    writer.WriteValue(((UE_<int>)value).Value);
                }
                else if (value_cs_type == typeof(UE_<string>))
                {
                    writer.WriteValue(((UE_<string>)value).Value);
                }
                // Container Type
                else if (value_cs_type == typeof(UE_Map))
                {
                    writer.WriteStartObject();

                    UE_Map value_map = (UE_Map)value;

                    writer.WritePropertyName(JsonNaming.UE_Map.Map);
                    {
                        writer.WriteStartArray();

                        foreach (UE_Map_KeyValuePair keyValuePair in value_map.Map)
                        {
                            writer.WriteStartArray();

                            Write.UE_Value(writer, keyValuePair.Key);
                            Write.UE_Value(writer, keyValuePair.Value);

                            writer.WriteEndArray();
                        }

                        writer.WriteEndArray();
                    }

                    writer.WriteEndObject();
                }
                else if (value_cs_type == typeof(UE_Array) || value_cs_type == typeof(UE_StructArray))
                {
                    UE_Array value_array = (UE_Array)value;

                    writer.WriteStartArray();

                    foreach (UE_Value item in value_array.ItemList)
                    {
                        Write.UE_Value(writer, item);
                    }

                    writer.WriteEndArray();
                }
                // Struct Type
                else if (value_cs_type == typeof(UE_Struct_Vector))
                {
                    UE_Struct_Vector value_struct_vector = (UE_Struct_Vector)value;

                    writer.WriteStartArray();

                    writer.WriteValue(value_struct_vector.X);
                    writer.WriteValue(value_struct_vector.Y);
                    writer.WriteValue(value_struct_vector.Z);

                    writer.WriteEndArray();
                }
                else if (value_cs_type == typeof(UE_Struct_Quat))
                {
                    UE_Struct_Quat value_struct_quat = (UE_Struct_Quat)value;

                    writer.WriteStartArray();

                    writer.WriteValue(value_struct_quat.A);
                    writer.WriteValue(value_struct_quat.B);
                    writer.WriteValue(value_struct_quat.C);
                    writer.WriteValue(value_struct_quat.D);

                    writer.WriteEndArray();
                }
                else if (value_cs_type == typeof(UE_Struct_Generic))
                {
                    UE_Struct_Generic value_struct_generic = (UE_Struct_Generic)value;

                    Write.PropertyList_as_Object(writer, value_struct_generic.PropertyList);
                }
                else
                {
                    throw new NotImplementedException(value_cs_type.ToString());
                }
            }
        }
    }
}
