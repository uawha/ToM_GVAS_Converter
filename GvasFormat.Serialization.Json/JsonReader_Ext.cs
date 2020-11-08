using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace GvasFormat.Serialization.Json
{
    static class JsonReader_Ext
    {
        public static JsonToken AssertRead(this JsonReader reader)
        {
            if (!reader.Read())
            {
                throw new Exception("JsonReader Read -> End.");
            }
            return reader.TokenType;
        }

        public static void AssertReadToken(this JsonReader reader, JsonToken token)
        {
            if (!reader.Read())
            {
                throw new Exception("JsonReader Read -> End.");
            }
            if (reader.TokenType != token)
            {
                throw new FormatException($"JsonReader Read -> Expect {token}, but read {reader.TokenType}");
            }
        }

        internal static Dictionary<JsonToken, HashSet<Type>> AtomCompatible = new Dictionary<JsonToken, HashSet<Type>>()
        {
            [JsonToken.Integer] = new HashSet<Type>()
            {
                typeof(Int64), typeof(UInt64),
                typeof(Int32), typeof(UInt32),
                typeof(Int16), typeof(UInt16),
                typeof(Byte)
            },

            [JsonToken.Float] = new HashSet<Type>()
            {
                typeof(Double), typeof(Single)
            },

            [JsonToken.String] = new HashSet<Type>()
            {
                typeof(String)
            },

            [JsonToken.Boolean] = new HashSet<Type>()
            {
                typeof(Boolean)
            }
        };

        public static void AssertReadPropertyName(this JsonReader reader, string propertyName)
        {
            AssertReadToken(reader, JsonToken.PropertyName);
            if (reader.Value.ToString() != propertyName)
            {
                throw new FormatException();
            }
        }

        public static string ReadPropertyName(this JsonReader reader)
        {
            AssertReadToken(reader, JsonToken.PropertyName);
            return reader.Value.ToString();
        }

        static T ConvertInteger<T>(long value)
        {
            Type target_type = typeof(T);
            if (target_type == typeof(Int64))
            {
                return (T)(Object)value;
            }
            else if (target_type == typeof(UInt64))
            {
                return (T)(Object)(Convert.ToUInt64(value));
            }
            else if (target_type == typeof(Int32))
            {
                return (T)(Object)(Convert.ToInt32(value));
            }
            else if (target_type == typeof(UInt32))
            {
                return (T)(Object)(Convert.ToUInt32(value));
            }
            else if (target_type == typeof(Int16))
            {
                return (T)(Object)(Convert.ToInt16(value));
            }
            else if (target_type == typeof(UInt16))
            {
                return (T)(Object)(Convert.ToUInt16(value));
            }
            else if (target_type == typeof(Byte))
            {
                return (T)(Object)(Convert.ToByte(value));
            }
            else
            {
                throw new Exception();
            }
        }

        static T ConvertFloat<T>(double value)
        {
            Type target_type = typeof(T);
            if (target_type == typeof(Single))
            {
                return (T)(Object)(Convert.ToSingle(value));
            }
            else if (target_type == typeof(Double))
            {
                return (T)(Object)value;
            }
            else
            {
                throw new Exception();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T">bool, byte,(u)int(16|32|64), float,double, string</typeparam>
        /// <param name="reader"></param>
        /// <returns></returns>
        public static T AssertReadValue<T>(this JsonReader reader)
        {
            JsonToken token = AssertRead(reader);
            if (AtomCompatible.TryGetValue(token, out HashSet<Type> value))
            {
                //if (value.Contains(typeof(T)))
                //{
                if (token == JsonToken.Integer)
                {
                    return ConvertInteger<T>((long)(reader.Value));
                }
                else if (token == JsonToken.Float)
                {
                    return ConvertFloat<T>((double)(reader.Value));
                }
                else return (T)(reader.Value);
                //}
                //else
                //{
                //    throw new FormatException();
                //}
            }
            else
            {
                throw new FormatException();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T">bool, byte,(u)int(16|32|64), float,double, string</typeparam>
        /// <param name="reader"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public static T AssertReadPropertyNameValue<T>(this JsonReader reader, string propertyName)
        {
            AssertReadPropertyName(reader, propertyName);
            return AssertReadValue<T>(reader);
        }
    }
}
