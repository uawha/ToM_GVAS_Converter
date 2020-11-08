using CommandLine;

namespace GvasConverter
{
    class OptionGroup_ToJson : OptionGroup_IO
    {
        [Option("statistics", Abbreviation = "s")]
        public bool PrintStatistics;

        [Option("compact", Abbreviation = "c")]
        public bool Compact;
    }
}
