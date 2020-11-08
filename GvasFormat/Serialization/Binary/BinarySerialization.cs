using System;
using System.IO;
using System.Collections.Generic;

namespace GvasFormat.Serialization.Binary
{
    public static class BinarySerialization
    {
        public static Gvas ReadGvas(Stream stream)
        {
            using (var reader = new UE_BinaryReader(stream, true))
            {
                int type_tag = reader.ReadInt32();
                if (type_tag != GvasFormat.SaveGameHeader.UE4_SAVEGAME_FILE_TYPE_TAG)
                {
                    throw new NotSupportedException("Only `GVAS` file is supported.");
                }
                var result = new Gvas();
                //
                result.Header = SaveGameHeader.Read(reader);
                //
                var list = new List<UE_Property>();
                var ue_prop_reader = new UE_Property_Reader(reader);
                while (ue_prop_reader.Read() is UE_Property prop)
                {
                    if (prop is UE_None)
                    {
                        break;
                    }
                    list.Add(prop);
                }
                result.PropertyList = list;
                //
                return result;
            }
        }

        public static void WriteGvas(Stream stream, Gvas save)
        {
            using (var writer = new UE_BinaryWriter(stream, true))
            {
                SaveGameHeader.Write(writer, save.Header);
                var ue_prop_writer = new UE_Property_Writer(writer);
                for (int i = 0; i < save.PropertyList.Count; i++)
                {
                    ue_prop_writer.Write(save.PropertyList[i], PositionToken.Normal);
                }
                ue_prop_writer.Write_UE_None();
                string Null_End = null;
                writer.Write(Null_End);
            }
        }
    }
}
