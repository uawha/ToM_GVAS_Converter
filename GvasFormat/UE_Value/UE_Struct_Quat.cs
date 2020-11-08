namespace GvasFormat
{
    // https://docs.unrealengine.com/en-US/BlueprintAPI/Math/Quat/index.html
    // https://en.wikipedia.org/wiki/Quaternion
    public sealed class UE_Struct_Quat : UE_Struct
    {
        public float A;

        public float B;

        public float C;

        public float D;

        public UE_Struct_Quat(string struct_type, float a, float b, float c, float d) : base()
        {
            StructTypeString = struct_type;
            this.A = a;
            this.B = b;
            this.C = c;
            this.D = d;
        }
    }
}
