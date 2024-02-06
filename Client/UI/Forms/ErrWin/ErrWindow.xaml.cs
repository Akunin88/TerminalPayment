using Core.Configuration;
using Core.Helpers;

namespace Client.UI.ErrWin
{
    public partial class ErrWindow
    {
        private bool NeedTerminate;

        private ErrWindowManager errWindowManager;

        public ErrWindow(Exception ex, bool needTerminate = true, bool needLog = true)
        {
            InitializeComponent();
            NeedTerminate = needTerminate;
            errWindowManager = new ErrWindowManager(ex);
            this.Cursor = System.Windows.Input.Cursors.None;
            DataContext = errWindowManager;
            if (needLog)
                Task.Run(async () => await LogCollector.WriteAsync($"[{DateTime.UtcNow.AddHours(3):HH:mm:ss dd.MM.yyyy}] {ExceptionHelper.GetFullExceptionMessage(ex)}", Config.TxtLogFileName));
        }

        private void Border_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (NeedTerminate)
                Environment.Exit(0);
            else
                Close();
        }
    }
}
