using System.Windows;
using System.Windows.Controls;

namespace Client.UI.Controls.Buttons
{
    public partial class uButton : UserControl
    {
        public static readonly DependencyProperty LabelProperty = DependencyProperty.Register("Label", typeof(string), typeof(uButton));
        public string Label { get => (string)GetValue(LabelProperty); set => SetValue(LabelProperty, value); }

        public static readonly DependencyProperty Label2Property = DependencyProperty.Register("Label2", typeof(string), typeof(uButton));
        public string Label2 { get => (string)GetValue(Label2Property); set => SetValue(Label2Property, value); }

        public uButton()
        {
            InitializeComponent();
            tTbx.DataContext = this;
            tTbx2.DataContext = this;
        }
    }
}
