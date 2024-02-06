using Client.UI.ErrWin;
using Client.UI.Forms;
using System.Diagnostics;
using System.Windows;

namespace Client
{
    public partial class App : Application
    {
        public App()
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            if (Process.GetProcessesByName(Process.GetCurrentProcess().ProcessName).Length > 1)
            {
                //Process.GetProcessesByName(Process.GetCurrentProcess().ProcessName).First().Kill();
                throw new Exception("Другой процесс уже активен. Закройте его и повторите.");
            }
            fPanel common = new fPanel();
            common.ShowDialog();
            Environment.Exit(0);
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
            => new ErrWindow((Exception)e.ExceptionObject).ShowDialog();
    }
}
