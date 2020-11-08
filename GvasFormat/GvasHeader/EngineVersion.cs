using System;

namespace GvasFormat
{
    // \UnrealEngine-4.22.3-release\Engine\Source\Runtime\Core\Public\Misc\EngineVersionBase.h
    // \UnrealEngine-4.22.3-release\Engine\Source\Runtime\Core\Public\Misc\EngineVersion.h
    public struct EngineVersion
    {
        public UInt16 Major;

        public UInt16 Minor;

        public UInt16 Patch;

        public UInt32 ChangeList;

        public string Branch;
    }
}