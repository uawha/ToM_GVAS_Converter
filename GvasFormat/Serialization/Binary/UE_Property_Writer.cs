using System;
using System.IO;

namespace GvasFormat.Serialization.Binary
{
    class UE_Property_Writer
    {
        internal UE_Property_Writer(UE_BinaryWriter writer)
        {
            this.writer = writer;
        }

        UE_BinaryWriter writer;

        internal void Write(UE_Property property, PositionToken position)
        {
            writer.Write(property.Name);
            Write_UE_Value(property.Value, position);
        }

        void Write_UE_Value(UE_Value value, PositionToken position)
        {
            Type value_cs_type = value.GetType();
            // Non-container Type
            if (value_cs_type == typeof(UE_<bool>))
            {
                Write((UE_<bool>)value, position);
            }
            else if (value_cs_type == typeof(UE_<byte>))
            {
                Write((UE_<byte>)value, position);
            }
            else if (value_cs_type == typeof(UE_Enum))
            {
                Write((UE_Enum)value, position);
            }
            else if (value_cs_type == typeof(UE_<float>))
            {
                Write((UE_<float>)value, position);
            }
            else if (value_cs_type == typeof(UE_<int>))
            {
                Write((UE_<int>)value, position);
            }
            else if (value_cs_type == typeof(UE_<string>))
            {
                Write((UE_<string>)value, position);
            }
            // Container Type
            else if (value_cs_type == typeof(UE_Map))
            {
                Write((UE_Map)value, position);
            }
            else if (value_cs_type == typeof(UE_Array))
            {
                Write((UE_Array)value, position);
            }
            else if (value_cs_type == typeof(UE_StructArray))
            {
                Write((UE_StructArray)value, position);
            }
            // Struct Type
            else if (value_cs_type == typeof(UE_Struct_Vector))
            {
                Write((UE_Struct_Vector)value, position);
            }
            else if (value_cs_type == typeof(UE_Struct_Quat))
            {
                Write((UE_Struct_Quat)value, position);
            }
            else if (value_cs_type == typeof(UE_Struct_Generic))
            {
                Write((UE_Struct_Generic)value, position);
            }
            else
            {
                throw new NotImplementedException(value_cs_type.ToString());
            }
        }

        void Write_UE_Value_Hair(string type, long length)
        {
            writer.Write(type);
            writer.Write(length);
        }

        internal void Write_UE_None()
        {
            writer.Write(UE_None.Identifier);
        }

        #region Write Method -> Non-container Type

        void Write(UE_<bool> value, PositionToken position)
        {
            if (position == PositionToken.InsideMap)
            {
                writer.Write((byte)(value.Value ? 1 : 0));
            }
            else if (position == PositionToken.Normal)
            {
                Write_UE_Value_Hair(value.TypeString, 0);
                writer.Write((Int16)(value.Value ? 1 : 0));
            }
            else throw new NotImplementedException(position.ToString());
        }

        void Write(UE_<byte> value, PositionToken position)
        {
            if (position == PositionToken.Normal)
            {
                Write_UE_Value_Hair(value.TypeString, 1);
                Write_UE_None();
                writer.Write((byte)0);
                writer.Write(value.Value);
            }
            //else if (position == PositionToken.InsideArray)
            //{
            //    writer.Write(Value);
            //}
            else throw new NotImplementedException(position.ToString());
        }

        void Write(UE_Enum value, PositionToken position)
        {
            if (position == PositionToken.Normal)
            {
                byte[] _R;
                using (var ms = new MemoryStream())
                using (var bw = new UE_BinaryWriter(ms, true))
                {
                    bw.Write(value.Value);
                    _R = ms.ToArray(); // cannot do Stream.CopyTo, reason unknown
                }
                long length = _R.Length;
                //
                Write_UE_Value_Hair(value.TypeString, length);
                writer.Write(value.EnumType);
                writer.Write((byte)0);
                writer.Write(_R, 0, _R.Length);
            }
            else if (position == PositionToken.InsideArray)
            {
                writer.Write(value.Value);
            }
            else throw new NotImplementedException(position.ToString());
        }

        void Write(UE_<float> value, PositionToken position)
        {
            if (position == PositionToken.Normal)
            {
                Write_UE_Value_Hair(value.TypeString, SerializationConstant.I64_Size_FloatProperty);
                writer.Write((byte)0);
                writer.Write(value.Value);
            }
            else if (position == PositionToken.InsideArray)
            {
                writer.Write(value.Value);
            }
            else throw new NotImplementedException(position.ToString());
        }

        void Write(UE_<int> value, PositionToken position)
        {
            if (position == PositionToken.Normal)
            {
                Write_UE_Value_Hair(value.TypeString, SerializationConstant.I64_Size_IntProperty);
                writer.Write((byte)0);
                writer.Write(value.Value);
            }
            else if (position == PositionToken.InsideArray)
            {
                writer.Write(value.Value);
            }
            else throw new NotImplementedException(position.ToString());
        }

        void Write(UE_<string> value, PositionToken position)
        {
            if (position == PositionToken.Normal)
            {
                byte[] _R;
                using (var ms = new MemoryStream())
                using (var bw = new UE_BinaryWriter(ms, true))
                {
                    bw.Write(value.Value);
                    _R = ms.ToArray(); // cannot do Stream.CopyTo, reason unknown
                }
                long length = _R.Length;
                //
                Write_UE_Value_Hair(value.TypeString, length);
                writer.Write((byte)0);
                writer.Write(_R, 0, _R.Length);
            }
            else if (position == PositionToken.InsideMap)
            {
                writer.Write(value.Value);
            }
            else throw new NotImplementedException(position.ToString());
        }

        #endregion

        #region Write Method -> Container Type

        void Write(UE_Map value, PositionToken position)
        {
            if (position == PositionToken.Normal)
            {
                byte[] _R;
                using (var ms = new MemoryStream())
                using (var bw = new UE_BinaryWriter(ms, true))
                {
                    bw.Write((Int32)0);
                    bw.Write(value.Map.Count);
                    var ue_prop_w = new UE_Property_Writer(bw);
                    for (int i = 0; i < value.Map.Count; i++)
                    {
                        ue_prop_w.Write_UE_Value(value.Map[i].Key, PositionToken.InsideMap);
                        ue_prop_w.Write_UE_Value(value.Map[i].Value, PositionToken.InsideMap);
                    }
                    _R = ms.ToArray(); // cannot do Stream.CopyTo, reason unknown
                }
                long length = _R.Length;
                //
                Write_UE_Value_Hair(value.TypeString, length);
                writer.Write(value.KeyType);
                writer.Write(value.ValueType);
                writer.Write((byte)0);
                writer.Write(_R, 0, _R.Length);
            }
            else throw new NotImplementedException(position.ToString());
        }

        void Write_UE_Array_Header(string item_type)
        {
            writer.Write(item_type);
            writer.Write((byte)0);
        }

        void Write(UE_Array value, PositionToken position)
        {
            long length;
            byte[] _R;
            using (var ms = new MemoryStream())
            using (var bw = new UE_BinaryWriter(ms, true))
            {
                var ue_prop_w = new UE_Property_Writer(bw);
                for (int i = 0; i < value.ItemList.Length; i++)
                {
                    ue_prop_w.Write_UE_Value(value.ItemList[i], PositionToken.InsideArray);
                }
                _R = ms.ToArray();
            }
            length = 4 + _R.Length;
            //
            Write_UE_Value_Hair(value.TypeString, length);
            Write_UE_Array_Header(value.ItemType);
            writer.Write(value.ItemList.Length);
            writer.Write(_R, 0, _R.Length);
        }

        void Write_UE_Struct_Header(string struct_type)
        {
            writer.Write(struct_type);
            // guid empty.
            writer.Write(0L);
            writer.Write(0L);
            // t
            writer.Write((byte)0);
        }

        void Write(UE_StructArray value, PositionToken position)
        {
            long length;
            byte[] _R;
            using (var ms = new MemoryStream())
            using (var bw = new UE_BinaryWriter(ms, true))
            {
                long length_length;
                byte[] _R_R;
                using (var ms_ms = new MemoryStream())
                using (var bw_bw = new UE_BinaryWriter(ms_ms, true))
                {
                    var ue_prop_w_w = new UE_Property_Writer(bw_bw);
                    for (int i = 0; i < value.ItemList.Length; i++)
                    {
                        ue_prop_w_w.Write_UE_Value(value.ItemList[i], PositionToken.InsideArray);
                    }
                    _R_R = ms_ms.ToArray();
                }
                length_length = _R_R.Length;
                //
                bw.Write(value.SA_Name);
                bw.Write(value.SA_ItemType);
                bw.Write(length_length);
                var ue_prop_w = new UE_Property_Writer(bw);
                ue_prop_w.Write_UE_Struct_Header(value.SA_StructTypeString);
                bw.Write(_R_R, 0, _R_R.Length);
                //
                _R = ms.ToArray();
            }
            length = 4 + _R.Length;
            //
            Write_UE_Value_Hair(value.TypeString, length);
            Write_UE_Array_Header(value.ItemType);
            writer.Write(value.ItemList.Length);
            writer.Write(_R, 0, _R.Length);
        }

        #endregion

        #region Write Method -> Struct Type

        void Write(UE_Struct_Vector value, PositionToken position)
        {
            if (position == PositionToken.Normal || position == PositionToken.InsideArray)
            {
                Write_UE_Value_Hair(value.TypeString, 12);
                //
                Write_UE_Struct_Header(value.StructTypeString);
                writer.Write(value.X);
                writer.Write(value.Y);
                writer.Write(value.Z);
            }
            else throw new NotImplementedException(position.ToString());
        }

        void Write(UE_Struct_Quat value, PositionToken position)
        {
            if (position == PositionToken.Normal || position == PositionToken.InsideArray)
            {
                Write_UE_Value_Hair(value.TypeString, 16);
                //
                Write_UE_Struct_Header(value.StructTypeString);
                writer.Write(value.A);
                writer.Write(value.B);
                writer.Write(value.C);
                writer.Write(value.D);
            }
            else throw new NotImplementedException(position.ToString());
        }

        void Write(UE_Struct_Generic value, PositionToken position)
        {
            if (position == PositionToken.Normal)
            {
                byte[] _R;
                using (var ms = new MemoryStream())
                using (var bw = new UE_BinaryWriter(ms, true))
                {
                    var ue_prop_w = new UE_Property_Writer(bw);
                    foreach (var prop in value.PropertyList)
                    {
                        ue_prop_w.Write(prop, PositionToken.Normal);
                    }
                    ue_prop_w.Write_UE_None();
                    //
                    _R = ms.ToArray();
                }
                long I64_Length = _R.Length;
                Write_UE_Value_Hair(value.TypeString, I64_Length);
                //
                Write_UE_Struct_Header(value.StructTypeString);
                writer.Write(_R, 0, _R.Length);
            }
            else if (position == PositionToken.InsideArray)
            {
                foreach (var prop in value.PropertyList)
                {
                    Write(prop, PositionToken.Normal);
                }
                Write_UE_None();
            }
            else throw new NotImplementedException(position.ToString());
        }

        #endregion
    }
}
