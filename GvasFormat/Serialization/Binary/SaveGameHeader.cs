using System;

namespace GvasFormat.Serialization.Binary
{
    static class SaveGameHeader
    {
        internal static GvasFormat.SaveGameHeader Read(UE_BinaryReader reader)
        {
            var result = new GvasFormat.SaveGameHeader();
            result.FileTypeTag = GvasFormat.SaveGameHeader.UE4_SAVEGAME_FILE_TYPE_TAG;
            {
                result.SaveGameFileVersion = reader.ReadInt32();
                result.PackageFileUE4Version = reader.ReadInt32();
            }
            //
            var engine_version = new EngineVersion();
            {
                engine_version.Major = reader.ReadUInt16();
                engine_version.Minor = reader.ReadUInt16();
                engine_version.Patch = reader.ReadUInt16();
                engine_version.ChangeList = reader.ReadUInt32();
                engine_version.Branch = reader.ReadString();
            }
            result.SavedEngineVersion = engine_version;
            //
            result.CustomVersionFormat = reader.ReadInt32();
            //
            var version_container = new CustomVersionContainer();
            {
                version_container.Count = reader.ReadInt32();
                version_container.Versions = new CustomVersion[version_container.Count];
                for (var i = 0; i < version_container.Count; i++)
                {
                    var version = new CustomVersion();
                    {
                        version.Key = new Guid(reader.ReadBytes(16));
                        version.Version = reader.ReadInt32();
                    }
                    version_container.Versions[i] = version;
                }
            }
            result.CustomVersions = version_container;
            //
            result.SaveGameClassName = reader.ReadString();
            return result;
        }

        internal static void Write(UE_BinaryWriter writer, GvasFormat.SaveGameHeader header)
        {
            writer.Write(header.FileTypeTag);
            writer.Write(header.SaveGameFileVersion);
            writer.Write(header.PackageFileUE4Version);
            //
            writer.Write(header.SavedEngineVersion.Major);
            writer.Write(header.SavedEngineVersion.Minor);
            writer.Write(header.SavedEngineVersion.Patch);
            writer.Write(header.SavedEngineVersion.ChangeList);
            writer.Write(header.SavedEngineVersion.Branch);
            //
            writer.Write(header.CustomVersionFormat);
            writer.Write(header.CustomVersions.Count);
            for (int i = 0; i < header.CustomVersions.Versions.Length; i++)
            {
                var v = header.CustomVersions.Versions[i];
                var bin = v.Key.ToByteArray();
                writer.Write(bin, 0, bin.Length);
                writer.Write(v.Version);
            }
            //
            writer.Write(header.SaveGameClassName);
        }
    }
}
