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
    }

}
