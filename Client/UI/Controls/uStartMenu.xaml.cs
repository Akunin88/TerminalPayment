using Client.Enums;
using Client.Management;
using System.Windows.Controls;
using System.Windows.Input;

namespace Client.UI.Controls
{
    public partial class uStartMenu : UserControl
    {
        private ClientManager manager => this.DataContext as ClientManager;

        public uStartMenu()
        {
            InitializeComponent();
        }

        private void deflate_Click(object sender, MouseButtonEventArgs e) => manager?.SetMode(ControlEnum.DeflateBalance);
        private void inflate_Click(object sender, MouseButtonEventArgs e) => manager?.SetMode(ControlEnum.InflateBalance);
    }
}
