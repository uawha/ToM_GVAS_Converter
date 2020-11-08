namespace GvasFormat
{
    // \UnrealEngine-4.22.3-release\Engine\Source\Runtime\Engine\Private\GameplayStatics.cpp
    public class SaveGameHeader
    {
        public const int UE4_SAVEGAME_FILE_TYPE_TAG = 0x53415647; // GVAS

        public int FileTypeTag;

        public int SaveGameFileVersion;

        public int PackageFileUE4Version;

        public EngineVersion SavedEngineVersion;

        public int CustomVersionFormat;

        public CustomVersionContainer CustomVersions;

        public string SaveGameClassName;
    }
}
