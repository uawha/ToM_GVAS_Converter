namespace GvasFormat
{
    public class UE_Array : UE_Value
    {
        public string ItemType;

        public int Count;

        public UE_Value[] ItemList;

        public UE_Array(string type_string, string item_type, int count, UE_Value[] item_list)
        {
            this.TypeString = type_string;
            this.ItemType = item_type;
            this.Count = count;
            this.ItemList = item_list;
        }
    }
}