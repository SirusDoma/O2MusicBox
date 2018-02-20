using System;
using System.Collections.Generic;
using System.Text;

namespace CXO2
{
    public static class Extensions
    {
        public static string PadNull(this string value, int length)
        {
            return value.PadRight(length, '\0').Substring(0, length);
        }
    }
}
