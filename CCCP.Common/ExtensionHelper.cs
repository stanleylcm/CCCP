using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCCP.Common
{
    public static class ExtensionHelper
    {
        // string
        public static bool IsEquals(this string self, string compareStr)
        {
            return (self.Trim().ToUpper() == compareStr.Trim().ToUpper());
        }
        public static bool IsNullOrEmpty(this string self)
        {
            return string.IsNullOrEmpty(self);
        }

        public static bool IsContains(this string self, string compareStr)
        {
            return (self.Trim().ToUpper().IndexOf(compareStr.Trim().ToUpper()) >= 0);
        }

        public static string ToEnumString(this Enum self)
        {
            return self.ToString().Replace("_", " ");
        }
        public static T ToEnum<T>(this string self, out bool result)
        {
            if (self.IsNullOrEmpty()) self = "";
            self = self.Replace(" ", "_");
            result = Enum.IsDefined(typeof(T), self);
            if (result) return (T)Enum.Parse(typeof(T), self);
            else return default(T);
        }
        public static T ToEnum<T>(this string self)
        {
            if (self.IsNullOrEmpty()) self = "";
            self = self.Replace(" ", "_");
            bool result = Enum.IsDefined(typeof(T), self);
            if (result) return (T)Enum.Parse(typeof(T), self);
            else return default(T);
        }
    }
}
