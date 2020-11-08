using System.Collections.Generic;

namespace GvasFormat
{
    public sealed class UE_Struct_Generic : UE_Struct
    {
        public List<UE_Property> PropertyList;

        public UE_Struct_Generic(string struct_type, List<UE_Property> property_list) : base()
        {
            this.StructTypeString = struct_type;
            this.PropertyList = property_list;
        }
    }
}