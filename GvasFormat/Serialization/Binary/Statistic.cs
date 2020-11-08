using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;

namespace GvasFormat.Serialization.Binary
{
    public static class Statistic
    {
        //
        //
        public static HashSet<string> TypeStringSet = new HashSet<string>();

        public static void PrintToFile_TypeStringSet(string dir)
        {
            var ls = TypeStringSet.ToList();
            ls.Sort();
            File.WriteAllLines(Path.Combine(dir, "Statistic.TypeStringSet.txt"), ls);
        }
        //
        //
        public static HashSet<string> GenericStructTypeSet = new HashSet<string>();

        public static void PrintToFile_GenericStructTypeSet(string dir)
        {
            var ls = GenericStructTypeSet.ToList();
            ls.Sort();
            File.WriteAllLines(Path.Combine(dir, "Statistic.GenericStructTypeSet.txt"), ls);
        }
        //
        //
        public static Dictionary<string, HashSet<string>> EnumTypeSet = new Dictionary<string, HashSet<string>>();

        public static void EnumTypeSet_Add(UE_Enum e)
        {
            if (!EnumTypeSet.TryGetValue(e.EnumType, out var set))
            {
                set = new HashSet<string>();
                EnumTypeSet.Add(e.EnumType, set);
            }
            set.Add(e.Value);
        }

        public static void PrintToFile_EnumTypeSet(string dir)
        {
            var sb = new StringBuilder();
            foreach (var kvp in EnumTypeSet)
            {
                sb.AppendLine(kvp.Key);
                var ls = kvp.Value.ToList();
                ls.Sort();
                foreach (var item in ls)
                {
                    sb.AppendLine(item);
                }
                sb.AppendLine();
            }
            File.WriteAllText(Path.Combine(dir, "Statistic.EnumTypeSet.txt"), sb.ToString());
        }
        //
        //
        public static HashSet<Type> UETypeSet = new HashSet<Type>();

        public static void PrintToFile_UETypeSet(string dir)
        {
            var ls = UETypeSet.Select(x => x.ToString()).ToList();
            ls.Sort();
            File.WriteAllLines(Path.Combine(dir, "Statistic.UETypeSet.txt"), ls);
        }
    }
}
