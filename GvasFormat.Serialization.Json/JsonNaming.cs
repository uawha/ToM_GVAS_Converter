namespace GvasFormat.Serialization.Json
{
    public static class JsonNaming
    {
        public static class Gvas
        {
            public const string Header = "header";
            public const string Content = "content";
        }

        public static class SaveGameHeader
        {
            public const string FileTypeTag = "`FileTypeTag`";
            public const string SaveGameFileVersion = "`SaveGameFileVersion`";
            public const string PackageFileUE4Version = "`PackageFileUE4Version`";
            public const string SavedEngineVersion = "`SavedEngineVersion`";
            public const string CustomVersionFormat = "`CustomVersionFormat`";
            public const string CustomVersions = "`CustomVersions`";
            public const string SaveGameClassName = "`SaveGameClassName`";

            public static class EngineVersion
            {
                public const string Major = "`Major`";
                public const string Minor = "`Minor`";
                public const string Patch = "`Patch`";
                public const string ChangeList = "`ChangeList`";
                public const string Branch = "`Branch`";
            }

            public static class CustomVersionContainer
            {
                public const string Count = "count";
                public const string Versions = "`Versions`";
            }

            public static class CustomVersion
            {
                public const string Key = "`Key`";
                public const string Version = "`Version`";
            }
        }

        public static class UE_Value
        {
            public const string TypeString = "type";
            public const string Value = "value";
        }

        public static class UE_Array
        {
            public const string ItemType = "item_type";
            public const string Count = "count";
            public const string ItemList = "item_list";
        }

        public static class UE_Enum
        {
            public const string EnumType = "enum_type";
        }

        public static class UE_Map
        {
            public const string KeyType = "key_type";
            public const string ValueType = "value_type";
            public const string Count = "count";
            public const string Map = "key_value_pair_list";
        }

        public static class UE_Map_KeyValuePair
        {
            public const string Key = "key";
            public const string Value = "value";
        }

        public static class UE_Struct
        {
            public const string StructTypeString = "struct_type";
        }

        public static class UE_StructArray
        {
            public const string SA_Name = "struct_array_name";
            public const string SA_ItemType = "struct_array_item_type";
            public const string SA_StructTypeString = "struct_array_struct_type";
        }
    }
}
