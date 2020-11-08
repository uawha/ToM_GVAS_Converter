using System;
using CommandLine;

namespace GvasConverter
{
    class Program
    {
        const string Usage =
@"
Usage:

1. GvasConverter to-json {--in|-i}:<file> {--out|-o}:<file> [{--statistics|-s}] [{--compact|-c}]
2. GvasConverter to-savg {--in|-i}:<file> {--out|-o}:<file>

";

        static int Main(string[] args)
        {
            var work = new ProgramFramework()
                .SetMessageOnParseFailure(Usage)
                .RegisterCommandLineVerb<OptionGroup_ToJson>("to-json", new Verb_to_json())
                .RegisterCommandLineVerb<OptionGroup_IO>("to-savg", new Verb_to_gvas())
                .Init();

            int r = work.Run(args);
            Console.WriteLine("All done.");
            return r;
        }
    }
}
