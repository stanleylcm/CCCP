using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace CCCP.Common
{
    public static class ExtensionHelper
    {
        // string
        public static bool IsEquals(this string self, string comparer)
        {
            return (self.Trim().ToUpper() == comparer.Trim().ToUpper());
        }
        public static bool IsNullOrEmpty(this string self)
        {
            return string.IsNullOrEmpty(self);
        }
        public static bool IsContains(this string self, string comparer)
        {
            return (self.Trim().ToUpper().IndexOf(comparer.Trim().ToUpper()) >= 0);
        }

        // Enum
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

        // PropertyInfo
        public static bool Contains(this PropertyInfo[] self, string comparer)
        {
            foreach (PropertyInfo p in self) if (p.Name.IsEquals(comparer)) return true;
            return false;
        }
    }
}
