using Client.Enums;
using Client.Management;
using MahApps.Metro.Controls;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace Client.UI.Forms.WaitWindow
{
    public partial class fWaitWindow : MetroWindow, INotifyPropertyChanged
    {
        private ClientManager manager;

        public string Text { get; set; } = "Пожалуйста, ждите...";
        public Visibility ProgressVisible { get; set; } = Visibility.Visible;

        public fWaitWindow(ClientManager manager)
        {
            InitializeComponent();
            this.Cursor = System.Windows.Input.Cursors.None;
            this.manager = manager;
            mainCtrl.DataContext = this;
        }

        private void close_Click(object sender, MouseButtonEventArgs e)
        {
            Close();
            manager?.SetMode(ControlEnum.None);
        }

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if (manager == null)
                return;
            switch (manager.WorkMode)
            {
                case ControlEnum.RequestBeforeInflate:
                    Task.Run(() =>
                    {
                        Thread.Sleep(1000);
                        if (manager.DataManager.Refresh(ControlEnum.InflateBalance))
                            manager.Dispatcher.Invoke(() => this.Close());
                        else
                        {
                            Text = "Нет сети!";
                            ProgressVisible = Visibility.Hidden;
                            raisePropertyChanged(nameof(Text), nameof(ProgressVisible));
                        }
                    });
                    break;
                case ControlEnum.InflateBalance:
                    Task.Run(() =>
                    {
                        Thread.Sleep(1000);
                        Text = manager.DataManager.SendMoneyToServer();
                        ProgressVisible = Visibility.Hidden;
                        raisePropertyChanged(nameof(Text), nameof(ProgressVisible));
                    });
                    break;
                case ControlEnum.DeflateBalance:
                    Task.Run(() =>
                    {
                        Thread.Sleep(1000);
                        Text = manager.DataManager.SendMoneyToUser();
                        ProgressVisible = Visibility.Hidden;
                        raisePropertyChanged(nameof(Text), nameof(ProgressVisible));
                    });
                    break;
                default:
                    break;
            }
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
