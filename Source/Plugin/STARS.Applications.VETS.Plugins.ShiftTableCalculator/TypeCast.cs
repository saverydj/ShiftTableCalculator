using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace STARS.Applications.VETS.Plugins.ShiftTableCalculator
{
    static class TypeCast
    {
        public static bool ToBool(string inputString, bool defaultValue = false)
        {
            bool val;
            if (!bool.TryParse(inputString, out val)) return defaultValue;
            return val;
        }

        public static double ToDouble(string inputString, double defaultValue = 0)
        {
            double val;
            if (!double.TryParse(inputString, out val)) return defaultValue;
            return val;
        }

        public static int ToInt(string inputString, int defaultValue = 0)
        {
            int val;
            if (!int.TryParse(inputString, out val)) return defaultValue;
            return val;
        }

        public static bool IsBool(string inputString)
        {
            bool val;
            return bool.TryParse(inputString, out val);
        }

        public static bool IsDouble(string inputString)
        {
            double val;
            return double.TryParse(inputString, out val);
        }

        public static bool IsInt(string inputString)
        {
            int val;
            return int.TryParse(inputString, out val); 
        }

    }
}
