using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace mycalltruck.Admin
{
    static class Program
    {
        /// <summary>
        /// 해당 응용 프로그램의 주 진입점입니다.
        /// </summary>
        [STAThread]
        static void Main()
        {
            OperatingSystem os = System.Environment.OSVersion;
            if (os.Version.Major >= 6)
            {
                if (IsAdministrator() == false)
                {
                    try
                    {
                        ProcessStartInfo procInfo = new ProcessStartInfo();
                        procInfo.UseShellExecute = true;
                        procInfo.FileName = Assembly.GetExecutingAssembly().Location;
                        procInfo.WorkingDirectory = Environment.CurrentDirectory;
                        procInfo.Verb = "runas";
                        Process.Start(procInfo);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message.ToString());
                    }
                    return;
                }
            }
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FrmMDI());
        }

        static bool IsAdministrator()
        {
            WindowsIdentity identity = WindowsIdentity.GetCurrent();

            if (null != identity)
            {
                WindowsPrincipal principal = new WindowsPrincipal(identity);
                return principal.IsInRole(WindowsBuiltInRole.Administrator);
            }

            return false;
        }

    }
}
