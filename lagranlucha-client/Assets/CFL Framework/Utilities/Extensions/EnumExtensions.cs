using System;
using System.Collections.Generic;
using System.Linq;

namespace CFLFramework.Utilities.Extensions
{
    public static partial class EnumExtensions
    {
        public static int GetEnumByName<TEnum>(string enumString) where TEnum : struct
        {
            TEnum genericEnum;
            if (Enum.TryParse<TEnum>(enumString, true, out genericEnum))
                return Convert.ToInt32(genericEnum);
            else
                return 0;
        }

        public static TEnum GetRandomEnum<TEnum>()
        {
            Array enums = System.Enum.GetValues(typeof(TEnum));
            TEnum value = (TEnum)enums.GetValue(UnityEngine.Random.Range(0, enums.Length));
            return value;
        }

        public static string GetEnumName<TEnum>(int index)
        {
            return Enum.GetName(typeof(TEnum), index).ToLower();
        }

        public static List<string> GetEnumsAsStringList<TEnum>()
        {
            return Enum.GetValues(typeof(TEnum)).Cast<TEnum>().Select(v => v.ToString().ToLower()).ToList();
        }
    }
}
