namespace GvasFormat
{
    public sealed class UE_Struct_Vector : UE_Struct
    {
        public float X;

        public float Y;

        public float Z;

        public UE_Struct_Vector(string struct_type, float x, float y, float z) : base()
        {
            this.StructTypeString = struct_type;
            this.X = x;
            this.Y = y;
            this.Z = z;
        }
    }
}