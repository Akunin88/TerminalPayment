using System.Windows;
using System.Windows.Controls;

namespace Client.UI.Controls.Buttons
{
    public partial class uButtonMain : UserControl
    {
        public static readonly DependencyProperty LabelProperty = DependencyProperty.Register("Label", typeof(string), typeof(uButtonMain));
        public string Label { get => (string)GetValue(LabelProperty); set => SetValue(LabelProperty, value); }

        public uButtonMain()
        {
            InitializeComponent();
            tTbx.DataContext = this;
        }
    }
}
