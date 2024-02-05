using Client.UI.Controls.Buttons;
using Core.Models;
using System.Windows;
using System.Windows.Controls;

namespace Client.UI.Controls
{
    public partial class uKeyboard : UserControl
    {
        public uKeyboard()
        {
            InitializeComponent();
        }

        private void keyClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (sender is uButton ubtn && !string.IsNullOrEmpty(ubtn.Label))
                KeyPressed?.Invoke(this, ubtn.Label[0]);
        }

        public event EventHandler<char> KeyPressed = delegate { };
    }
}
