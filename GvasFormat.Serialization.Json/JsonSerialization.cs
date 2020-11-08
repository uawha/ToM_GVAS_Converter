using System.Text;
using System.IO;
using Newtonsoft.Json;

namespace GvasFormat.Serialization.Json
{
    public static class JsonSerialization
    {
        public static Gvas ReadGvas(Stream stream)
        {
            Gvas _R;
            using (var sr = new StreamReader(stream))
            using (var json_reader = new JsonTextReader(sr))
            {
                _R = JsonConvert_Invertible.Read.Gvas(json_reader);
            }
            return _R;
        }

        public static Gvas ReadGvas(string text)
        {
            Gvas _R;
            using (var sr = new StringReader(text))
            using (var json_reader = new JsonTextReader(sr))
            {
                _R = JsonConvert_Invertible.Read.Gvas(json_reader);
            }
            return _R;
        }

        public static void WriteGvas(Stream stream, Gvas save, bool compact)
        {
            using (var sw = new StreamWriter(stream, new UTF8Encoding(false, true)))
            using (var json_writer = new JsonTextWriter(sw))
            {
                json_writer.Formatting = Formatting.Indented;
                json_writer.Indentation = 2;
                if (compact) JsonConvert_Compact.Write.Gvas(json_writer, save);
                else JsonConvert_Invertible.Write.Gvas(json_writer, save);
                json_writer.Flush();
            }
        }

        public static string WriteGvas(Gvas save, bool compact)
        {
            var sb = new StringBuilder();
            using (var tw = new StringWriter(sb))
            using (var json_writer = new JsonTextWriter(tw))
            {
                json_writer.Formatting = Formatting.Indented;
                json_writer.Indentation = 2;
                if (compact) JsonConvert_Compact.Write.Gvas(json_writer, save);
                else JsonConvert_Invertible.Write.Gvas(json_writer, save);
                json_writer.Flush();
            }
            return sb.ToString();
        }
    }
}
