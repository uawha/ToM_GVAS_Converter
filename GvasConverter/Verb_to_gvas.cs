using System;
using System.IO;
using GvasFormat;
using CommandLine;

namespace GvasConverter
{
    class Verb_to_gvas : ICommandRunner<OptionGroup_IO>
    {
        public (bool, string) Check(OptionGroup_IO args) => args.Check();

        public int Run(OptionGroup_IO args)
        {
            Console.WriteLine("Reading json file.");
            Gvas save;
            using (var stream = File.Open(args.In, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                save = GvasFormat.Serialization.Json.JsonSerialization.ReadGvas(stream);
            }
            Console.WriteLine("Reading done.");
            Console.WriteLine("Saving to SaveGame file.");
            using (var stream = File.Open(args.Out, FileMode.Create, FileAccess.Write, FileShare.Read))
            {
                GvasFormat.Serialization.Binary.BinarySerialization.WriteGvas(stream, save);
            }
            Console.WriteLine("Saving done.");
            return 0;
        }
    }
}
