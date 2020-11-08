using System.IO;
using CommandLine;

namespace GvasConverter
{
    class OptionGroup_IO
    {
        [Option("in", Abbreviation = "i")]
        [OptionRequire]
        public string In;

        [Option("out", Abbreviation = "o")]
        [OptionRequire]
        public string Out;

        public (bool, string) Check()
        {
            if (!File.Exists(In))
            {
                return (false, $"File does not exist:{In}");
            }
            if (In.ToLower() == Out.ToLower())
            {
                return (false, $"Input file and output file cannot be the same.");
            }
            return (true, null);
        }
    }
}
