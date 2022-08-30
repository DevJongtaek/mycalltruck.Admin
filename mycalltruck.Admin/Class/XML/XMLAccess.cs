using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace mycalltruck.Admin.Class.XML
{
    class XMLAccess
    {
        public static string RootFolder = string.Empty;
        static XMLAccess()
        {
            string _RootFolder = Path.Combine(
                //Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), @"SSCF\");
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), @"mycalltruck.Admin");
            DirectoryInfo _Dir = new DirectoryInfo(_RootFolder);
            if (_Dir.Exists == false) _Dir.Create();
            RootFolder = _Dir.FullName;
        }
    }
}
