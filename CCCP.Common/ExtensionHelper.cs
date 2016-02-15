using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCCP.Common
{
    public static class ExtensionHelper
    {
        public static bool IsEquals(this string self, string compareStr)
        {
            return (self.Trim().ToUpper() == compareStr.Trim().ToUpper());
        }

        public static string ToEnumString(this Enum self)
        {
            return self.ToString().Replace("_", " ");
        }

        public static T ToEnum<T>(this string self, out bool result)
        {
            self = self.Replace(" ", "_");
            result = Enum.IsDefined(typeof(T), self);
            if (result) return (T)Enum.Parse(typeof(T), self);
            else return default(T);
        }
        public static T ToEnum<T>(this string self)
        {
            self = self.Replace(" ", "_");
            bool result = Enum.IsDefined(typeof(T), self);
            if (result) return (T)Enum.Parse(typeof(T), self);
            else return default(T);
        }
    }
}
