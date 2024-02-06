using Client.Enums;
using Client.Management;
using Client.UI.Forms.WaitWindow;
using Core.Models;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Input;

namespace Client.UI.Controls
{
    public enum InflateMode : int
    {
        SetAmount = 0,
        ShowPayment = 1,
    }

    public partial class uInflate : UserControl, INotifyPropertyChanged
    {
        private ClientManager manager => this.DataContext as ClientManager;

        public InflateMode InflateMode { get; set; } = InflateMode.SetAmount;

        public uInflate()
        {
            InitializeComponent();
            tProxy.DataContext = this;
        }

        private void apply_Click(object sender, MouseButtonEventArgs e)
        {
            switch (InflateMode)
            {
                case InflateMode.SetAmount:
                    if (manager != null)
                        manager.DataManager.Title = "Для пополнения сделайте перевод на эту карту:";
                    InflateMode = InflateMode.ShowPayment;
                    raisePropertyChanged(nameof(InflateMode));
                    break;
                case InflateMode.ShowPayment:
                default:
                    fWaitWindow rw = new fWaitWindow(manager);
                    rw.ShowDialog();
                    break;
            }
        }

        private void cancel_Click(object sender, MouseButtonEventArgs e) => manager?.SetMode(ControlEnum.None);

        private void key_Pressed(object sender, char e)
        {
            if (manager == null)
                return;
            PaymentInfo serverInfo = manager.DataManager.ServerInfo;
            switch (e)
            {
                case 'C':
                    serverInfo.Amount = 0;
                    break;
                case '←':
                    if (serverInfo.Amount < 10)
                        serverInfo.Amount = 0;
                    else
                        serverInfo.Amount /= 10;
                    break;
                default:
                    if (int.TryParse($"{serverInfo.Amount}{e}", out int amount))
                        serverInfo.Amount = amount;
                    break;
            }
        }

        private void UserControl_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            InflateMode = InflateMode.SetAmount;
            raisePropertyChanged(nameof(InflateMode));
        }

        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        protected internal void raisePropertyChanged(params string[] properties)
        {
            if (PropertyChanged != null && properties != null)
                foreach (var p in properties)
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(p));
        }
    }
}
