﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using log4net;
using System.Diagnostics;

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
        public static string Left(this string self, int length)
        {
            if (length >= 0)
            {
                if (length >= self.Length) return self;
                else return self.Substring(0, length);
            }
            else return "";
        }
        public static string Right(this string self, int length)
        {
            if (length >= 0)
            {
                if (length >= self.Length) return self;
                else return self.Substring(self.Length - length, length);
            }
            else return "";
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

        // Exception
        public static void LogError(this Exception self, string sourceData = "")
        {
            StackTrace stackTrace = new StackTrace();
            StackFrame frame = new StackFrame(1);
            var currentMethod = frame.GetMethod(); //Gets the current method name
            var type = currentMethod.DeclaringType;

            ILog logger = LogManager.GetLogger(type + "-" + currentMethod.Name);

            log4net.GlobalContext.Properties["SourceData"] = sourceData;
            log4net.GlobalContext.Properties["ExceptionMessage"] = self.Message;
            log4net.GlobalContext.Properties["StackTrace"] = self.StackTrace;
            log4net.GlobalContext.Properties["CreatedBy"] = "";

            logger.Error(type + "-" + currentMethod.Name, self);

            logger = null;
            frame = null;
            stackTrace = null;
        }

        // List
        public static List<T> Clone<T>(this List<T> self) where T : ICloneable
        {
            return self.ConvertAll(x => (T)x.Clone());
        }
        public static List<string> ListAll(this Enum self)
        {
            List<string> result = new List<string>();
            foreach (Enum value in Enum.GetValues(self.GetType())) result.Add(value.ToEnumString());
            return result;
        }
    }
}
