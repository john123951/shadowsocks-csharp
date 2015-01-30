using Shadowsocks.Controller;
using Shadowsocks.Properties;
using Shadowsocks.View;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace Shadowsocks
{
    internal static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        private static void Main()
        {
            Util.Utils.ReleaseMemory();
            I18N.Register(Resources.cn);

            using (Mutex mutex = new Mutex(false, "Global\\" + "71981632-A427-497F-AB91-241CD227EC1F"))
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);

                if (!mutex.WaitOne(0, false))
                {
                    Process[] oldProcesses = Process.GetProcessesByName("Shadowsocks");
                    if (oldProcesses.Length > 0)
                    {
                        Process oldProcess = oldProcesses[0];
                    }
                    MessageBox.Show("Shadowsocks is already running.\n\nFind Shadowsocks icon in your notify tray.");
                    return;
                }
                Directory.SetCurrentDirectory(Application.StartupPath);
#if !DEBUG
                Logging.OpenLogFile();
#endif
                ShadowsocksController controller = new ShadowsocksController();

                MenuViewController viewController = new MenuViewController(controller);

                //开始运行
                controller.Start();

                Application.Run();
            }
        }
    }
}