using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace GvasFormat.Serialization.Binary
{
    class UE_Property_Reader
    {
        internal UE_Property_Reader(UE_BinaryReader reader)
        {
            this.reader = reader;
        }

        UE_BinaryReader reader;

        class ValueInfo
        {
            internal string type;
            internal long length;
            internal PositionToken position;
        }

        internal UE_Property Read()
        {
            if (reader.PeekChar() < 0)
            {
                throw new EndOfStreamException();
            }
            var name = reader.ReadString();
            if (name == null)
            {
                return null;
            }
            else if (name == UE_None.Identifier)
            {
                return UE_None.Get();
            }
            else
            {
                var value_info = new ValueInfo();
                value_info.type = reader.ReadString();
                value_info.length = reader.ReadInt64();
                value_info.position = PositionToken.Normal;
                var value = Read_UE_Value(value_info);
                return new UE_Property() { Name = name, Value = value };
            }
        }

        UE_Value Read_UE_Value(ValueInfo value_info)
        {
            UE_Value result;
            var itemOffset = reader.BaseStream.Position;
            switch (value_info.type)
            {
                //
                // non-container
                case UE_Value_TypeString.BoolProperty:
                    result = Read_UE_Bool(value_info);
                    break;
                case UE_Value_TypeString.ByteProperty:
                    result = Read_UE_Byte(value_info);
                    break;
                case UE_Value_TypeString.EnumProperty:
                    result = Read_UE_Enum(value_info);
                    break;
                case UE_Value_TypeString.FloatProperty:
                    result = Read_UE_Float32(value_info);
                    break;
                case UE_Value_TypeString.IntProperty:
                    result = Read_UE_Int32(value_info);
                    break;
                case UE_Value_TypeString.NameProperty:
                    //case UE_ValueTypeString.StrProperty:
                    result = Read_UE_String(value_info);
                    break;
                //
                // container
                // redirect.
                // UE_Struct ... lots of them are not engine defined, but game author defined.
                case UE_Value_TypeString.StructProperty:
                    string struct_type = Read_UE_Struct_Header();
                    // do not include this line into Read_UE_Struct() because 
                    // Read_UE_StructArray() needs Read_UE_Struct(struct_type)
                    result = Read_UE_Struct(struct_type);
                    break;
                case UE_Value_TypeString.ArrayProperty:
                    string item_type = Read_UE_Array_Header();
                    if (item_type == UE_Value_TypeString.StructProperty)
                    {
                        result = Read_UE_StructArray(value_info, item_type);
                    }
                    else
                    {
                        result = Read_UE_Array(value_info, item_type);
                    }
                    break;
                case UE_Value_TypeString.MapProperty:
                    result = Read_UE_Map(value_info);
                    break;
                default:
                    throw new FormatException($"Offset 0x{itemOffset:X8}: unknown value type `{value_info.type}`.");
            }
            Statistic.TypeStringSet.Add(value_info.type);
            Statistic.UETypeSet.Add(result.GetType());
            return result;
        }

        UE_Struct Read_UE_Struct(string struct_type)
        {
            UE_Struct result;
            switch (struct_type)
            {
                case UE_Value_TypeString.Struct.Vector:
                case UE_Value_TypeString.Struct.Rotator:
                    result = Read_UE_Struct_Vector(struct_type);
                    break;
                case UE_Value_TypeString.Struct.Quat:
                    result = Read_UE_Struct_Quat(struct_type);
                    break;
                default:
                    result = Read_UE_Struct_Generic(struct_type);
                    break;
            }
            Statistic.UETypeSet.Add(result.GetType());
            return result;
        }

        #region Read Method -> Non-container Type

        UE_<bool> Read_UE_Bool(ValueInfo value_info)
        {
            bool value;
            long value_pos = reader.BaseStream.Position;
            int val;
            if (value_info.position == PositionToken.InsideMap)
            {
                val = reader.ReadByte();
                if (val == 0)
                {
                    value = false;
                }
                else if (val == 1)
                {
                    value = true;
                }
                else
                {
                    throw new FormatException($"Offset: 0x{value_pos:X8}. Expected bool value 0x00 or 0x01, but was 0x{val:X2}");
                }
            }
            else if (value_info.position == PositionToken.Normal)
            {
                if (value_info.length != 0)
                {
                    throw new FormatException($"Offset: 0x{value_pos:X8}. Expected bool value length 0, but was {value_info.length}");
                }
                val = reader.ReadInt16();
                if (val == 0)
                {
                    value = false;
                }
                else if (val == 1)
                {
                    value = true;
                }
                else
                {
                    throw new InvalidOperationException($"Offset: 0x{value_pos:X8}. Expected bool value, but was {val}");
                }
            }
            else throw new NotImplementedException(value_info.position.ToString());
            return new UE_<bool>(value_info.type, value);
        }

        UE_<byte> Read_UE_Byte(ValueInfo value_info)
        {
            byte value;
            if (value_info.position == PositionToken.Normal)
            {
                if (value_info.length != 1)
                {
                    throw new Exception();
                }
                var terminator = reader.ReadByte();
                if (terminator != 0)
                {
                    // 05 00 00 00 None 00 // 4 5
                    //reader.BaseStream.Position += 8;
                    reader.ReadInt64();
                    // why there is a "None" string here? LOL.
                    terminator = reader.ReadByte();
                    if (terminator != 0)
                    {
                        throw new FormatException($"Offset: 0x{reader.BaseStream.Position - 1:X8}. Expected terminator (0x00), but was (0x{terminator:X2})");
                    }
                }
                value = reader.ReadByte();
            }
            //else if (position == PositionToken.InsideArray)
            //{
            //    Value = reader.ReadByte();
            //}
            else throw new NotImplementedException(value_info.position.ToString());
            return new UE_<byte>(value_info.type, value);
        }

        UE_Enum Read_UE_Enum(ValueInfo value_info)
        {
            string enum_type;
            string value;
            if (value_info.position == PositionToken.Normal)
            {
                enum_type = reader.ReadString();
                var terminator = reader.ReadByte();
                if (terminator != 0)
                {
                    throw new FormatException($"Offset: 0x{reader.BaseStream.Position - 1:X8}. Expected terminator (0x00), but was (0x{terminator:X2})");
                }
                value = reader.ReadString();
            }
            else if (value_info.position == PositionToken.InsideArray)
            {
                value = reader.ReadString();
                int index = value.IndexOf("::");
                if (index < 0)
                {
                    throw new FormatException($"Offset: 0x{reader.BaseStream.Position - 1:X8}. Where is \"::\"? Should be an enum value.");
                }
                enum_type = value.Substring(0, index);
            }
            else throw new NotImplementedException(value_info.position.ToString());
            UE_Enum result = new UE_Enum(value_info.type, enum_type, value);
            Statistic.EnumTypeSet_Add(result);
            return result;
        }

        UE_<float> Read_UE_Float32(ValueInfo value_info)
        {
            float value;
            if (value_info.position == PositionToken.Normal)
            {
                var terminator = reader.ReadByte();
                if (terminator != 0)
                {
                    throw new FormatException($"Offset: 0x{reader.BaseStream.Position - 1:X8}. Expected terminator (0x00), but was (0x{terminator:X2})");
                }
                if (value_info.length != SerializationConstant.I64_Size_FloatProperty)
                {
                    throw new FormatException($"Expected float value of length {SerializationConstant.I64_Size_FloatProperty}, but was {value_info.length}");
                }
                value = reader.ReadSingle();
            }
            else if (value_info.position == PositionToken.InsideArray)
            {
                value = reader.ReadSingle();
            }
            else throw new NotImplementedException(value_info.position.ToString());
            return new UE_<float>(value_info.type, value);
        }

        UE_<int> Read_UE_Int32(ValueInfo value_info)
        {
            int value;
            if (value_info.position == PositionToken.Normal)
            {
                var terminator = reader.ReadByte();
                if (terminator != 0)
                {
                    throw new FormatException($"Offset: 0x{reader.BaseStream.Position - 1:X8}. Expected terminator (0x00), but was (0x{terminator:X2})");
                }
                if (value_info.length != SerializationConstant.I64_Size_IntProperty)
                {
                    throw new FormatException($"Expected int value of length {SerializationConstant.I64_Size_IntProperty}, but was {value_info.length}");
                }
                value = reader.ReadInt32();
            }
            else if (value_info.position == PositionToken.InsideArray)
            {
                value = reader.ReadInt32();
            }
            else throw new NotImplementedException(value_info.position.ToString());
            return new UE_<int>(value_info.type, value);
        }

        UE_<string> Read_UE_String(ValueInfo value_info)
        {
            string value;
            if (value_info.position == PositionToken.Normal)
            {
                var terminator = reader.ReadByte();
                if (terminator != 0)
                {
                    throw new FormatException($"Offset: 0x{reader.BaseStream.Position - 1:X8}. Expected terminator (0x00), but was (0x{terminator:X2})");
                }
                value = reader.ReadString();
            }
            else if (value_info.position == PositionToken.InsideMap)
            {
                value = reader.ReadString();
            }
            else throw new NotImplementedException(value_info.position.ToString());
            return new UE_<string>(value_info.type, value);
        }

        #endregion

        #region Read Method -> Container Type

        UE_Map Read_UE_Map(ValueInfo value_info)
        {
            string key_type = reader.ReadString();
            string value_type = reader.ReadString();
            long unknown_position = reader.BaseStream.Position;
            var unknown = reader.ReadBytes(5);
            if (unknown.Any(b => b != 0))
            {
                throw new InvalidOperationException($"Offset 0x{unknown_position:X8}: the 5 bytes are expected to be all 0. I cannot do json to gvas conversion in this case.");
            }
            int count = reader.ReadInt32();
            List<UE_Map_KeyValuePair> content = new List<UE_Map_KeyValuePair>();
            ValueInfo value_info_temp = new ValueInfo();
            value_info_temp.length = -1;
            value_info_temp.position = PositionToken.InsideMap;
            for (int i = 0; i < count; i++)
            {
                value_info_temp.type = key_type;
                UE_Value key = Read_UE_Value(value_info_temp);
                value_info_temp.type = value_type;
                UE_Value value = Read_UE_Value(value_info_temp);
                content.Add(new UE_Map_KeyValuePair(key, value));
            }
            return new UE_Map(value_info.type, key_type, value_type, count, content);
        }

        string Read_UE_Array_Header()
        {
            string item_type = reader.ReadString();
            var terminator = reader.ReadByte();
            if (terminator != 0)
            {
                throw new FormatException($"Offset: 0x{reader.BaseStream.Position - 1:X8}. Expected terminator (0x00), but was (0x{terminator:X2})");
            }
            return item_type;
        }

        UE_Array Read_UE_Array(ValueInfo value_info, string item_type)
        {
            int count = reader.ReadInt32();
            UE_Value[] item_list = new UE_Value[count];
            ValueInfo value_info_temp = new ValueInfo();
            value_info_temp.type = item_type;
            value_info_temp.length = -1;
            value_info_temp.position = PositionToken.InsideArray;
            for (var i = 0; i < count; i++)
            {
                item_list[i] = Read_UE_Value(value_info_temp);
            }
            return new UE_Array(value_info.type, item_type, count, item_list);
        }

        string Read_UE_Struct_Header()
        {
            string struct_type = reader.ReadString();
            var id = new Guid(reader.ReadBytes(16));
            if (id != Guid.Empty)
            {
                throw new FormatException($"Offset: 0x{reader.BaseStream.Position - 16:X8}. Expected struct ID {Guid.Empty}, but was {id}");
            }
            var terminator = reader.ReadByte();
            if (terminator != 0)
            {
                throw new FormatException($"Offset: 0x{reader.BaseStream.Position - 1:X8}. Expected terminator (0x00), but was (0x{terminator:X2})");
            }
            return struct_type;
        }

        UE_StructArray Read_UE_StructArray(ValueInfo value_info, string item_type)
        {
            int count = reader.ReadInt32();
            if (reader.PeekChar() < 0)
            {
                throw new Exception();
            }
            string sa_name = reader.ReadString();
            //if (SA_Name != array_name) { throw new Exception(); }
            string sa_item_type = reader.ReadString();
            if (sa_item_type != item_type) { throw new Exception(); }
            var __valueLength = reader.ReadInt64(); // after struct header terminator.
            UE_Struct[] item_list = new UE_Struct[count];
            string sa_struct_type_string = Read_UE_Struct_Header();
            // __valueLength begins here.
            for (int i = 0; i < count; i++)
            {
                item_list[i] = Read_UE_Struct(sa_struct_type_string);
            }
            return new UE_StructArray(value_info.type, item_type, count, item_list, sa_name, sa_item_type, sa_struct_type_string);
        }

        #endregion

        #region Read Method -> Struct Type

        UE_Struct_Vector Read_UE_Struct_Vector(string struct_type)
        {
            float x = reader.ReadSingle();
            float y = reader.ReadSingle();
            float z = reader.ReadSingle();
            return new UE_Struct_Vector(struct_type, x, y, z);
        }

        UE_Struct_Quat Read_UE_Struct_Quat(string struct_type)
        {
            float a = reader.ReadSingle();
            float b = reader.ReadSingle();
            float c = reader.ReadSingle();
            float d = reader.ReadSingle();
            return new UE_Struct_Quat(struct_type, a, b, c, d);
        }

        UE_Struct_Generic Read_UE_Struct_Generic(string struct_type)
        {
            List<UE_Property> property_list = new List<UE_Property>();
            while (Read() is UE_Property prop)
            {
                if (prop is UE_None) break;
                property_list.Add(prop);
            }
            Statistic.GenericStructTypeSet.Add(struct_type);
            return new UE_Struct_Generic(struct_type, property_list);
        }

        #endregion
    }
}
