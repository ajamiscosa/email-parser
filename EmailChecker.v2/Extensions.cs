using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailChecker.v2
{
    public static class Extensions
    {
        public static bool IsNullOrEmptyOrWhiteSpace(this String str)
        {
            return String.IsNullOrEmpty(str) || String.IsNullOrWhiteSpace(str) || str.Length == 0;
        }
    }
}
