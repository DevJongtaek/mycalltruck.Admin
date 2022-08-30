using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace mycalltruck.Admin.Class.Extensions
{
    public static class IntNull
    {
        public static int? Parse(string s)
        {
            int t = 0;
            if (int.TryParse(s, out t))
                return t;
            else
                return null;
        }
    }
}
