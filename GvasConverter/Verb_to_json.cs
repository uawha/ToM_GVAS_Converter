using System;
using System.IO;
using CommandLine;
using GvasFormat.Serialization.Binary;

namespace GvasConverter
{
    class Verb_to_json : ICommandRunner<OptionGroup_ToJson>
    {
        public (bool, string) Check(OptionGroup_ToJson args) => args.Check();

        public int Run(OptionGroup_ToJson args)
        {
            Console.WriteLine("Reading UE4 SaveGame file.");
            GvasFormat.Gvas save;
            using (var stream = File.Open(args.In, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                save = BinarySerialization.ReadGvas(stream);
            }
            Console.WriteLine("Reading done.");
            Console.WriteLine("Converting to json.");
            using (var fs = File.Create(args.Out))
            {
                GvasFormat.Serialization.Json.JsonSerialization.WriteGvas(fs, save, args.Compact);
            }
            Console.WriteLine("Converting done.");
            if (args.Compact)
            {
                Console.WriteLine("Note! I can not convert compact json back to gvas because type informations are not included.");
            }
            if (args.PrintStatistics)
            {
                Console.WriteLine("Printing statistics.");
                string dir = Path.GetDirectoryName(Path.GetFullPath(args.Out));
                Statistic.PrintToFile_EnumTypeSet(dir);
                Statistic.PrintToFile_GenericStructTypeSet(dir);
                Statistic.PrintToFile_UETypeSet(dir);
                Statistic.PrintToFile_TypeStringSet(dir);
                Console.WriteLine("Printing done.");
            }
            return 0;
        }
    }
}
