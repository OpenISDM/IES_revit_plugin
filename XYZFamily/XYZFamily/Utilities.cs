using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYZFamily
{
    public static class Utilities
    {
        /*
         * Converts feet to Meters
         */ 
        public static double feetToMeters(double ftValue)
        {
            return ftValue * Constants._feetToMeters;
        }
    }
}
