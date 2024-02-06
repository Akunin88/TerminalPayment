using Client.Enums;
using Client.Management;
using System.Windows.Controls;

namespace Client.UI.Controls
{
    public partial class uStartMenu : UserControl
    {
        private ClientManager manager => this.DataContext as ClientManager;

        public uStartMenu()
        {
            InitializeComponent();
        }

        private void inflate_Click(object sender, System.Windows.RoutedEventArgs e) => manager?.SetMode(ControlEnum.InflateBalance);
        private void deflate_Click(object sender, System.Windows.RoutedEventArgs e) => manager?.SetMode(ControlEnum.DeflateBalance);
    }
}
