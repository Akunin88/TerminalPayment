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
        }

        private void inflate_Click(object sender, RoutedEventArgs e)
        {
            mainWindow.ClientManager.SetMode(Enums.ControlEnum.InflateBalance);
        }

        private void deflate_Click(object sender, RoutedEventArgs e)
        {
            mainWindow.ClientManager.SetMode(Enums.ControlEnum.DeflateBalance);
        }
    }
}
