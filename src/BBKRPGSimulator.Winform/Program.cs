using System;
using System.Text;
using System.Windows.Forms;

namespace BBKRPGSimulator.Winform
{
    internal static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        private static void Main()
        {
#if NETCOREAPP
            //Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
#endif
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}