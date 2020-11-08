namespace GvasFormat
{
    public class UE_StructArray : UE_Array
    {
        public string SA_Name;

        public string SA_ItemType;

        public string SA_StructTypeString;

        public UE_StructArray(string type_string, string item_type, int count, UE_Value[] item_list,
            string sa_name, string sa_item_type, string sa_struct_type_string)
            : base(type_string, item_type, count, item_list)
        {
            this.SA_Name = sa_name;
            this.SA_ItemType = sa_item_type;
            this.SA_StructTypeString = sa_struct_type_string;
        }
    }
}
