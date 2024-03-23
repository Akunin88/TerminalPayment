using Core.Configuration;
using System.Windows;

namespace Client.UI.Forms
{
    public partial class fPanel
    {
        private fMain mainWindow;

        public fPanel()
        {
            InitializeComponent();
            this.Cursor = System.Windows.Input.Cursors.None;
            Config config = Config.Read();
            Left = config.PanelLeft;
            Top = config.PanelTop;
            mainWindow = new fMain();
            DataContext = mainWindow.DataContext;
        }

        private void inflate_Click(object sender, RoutedEventArgs e)
        {
            mainWindow.ClientManager.SetMode(Enums.ControlEnum.RequestBeforeInflate);
        }

        private void deflate_Click(object sender, RoutedEventArgs e)
        {
            mainWindow.ClientManager.SetMode(Enums.ControlEnum.DeflateBalance);
        }
    }
}
