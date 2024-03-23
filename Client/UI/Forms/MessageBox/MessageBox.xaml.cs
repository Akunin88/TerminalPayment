using ControlzEx.Theming;
using Core.Configuration;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.ComponentModel;
using System.Windows;

namespace Client.UI.Forms
{
    public partial class MessageBoxW : INotifyPropertyChanged
    {
        private string message;
        private string captionMessage;
        private System.Windows.Media.Brush selectedColor = System.Windows.Media.Brushes.LightBlue;

        public AffNegButtonsStyle ANStyle { get; private set; }
        public string Message { get => message; set { message = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Message")); } }
        public string CaptionMessage { get => captionMessage; set { captionMessage = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("CaptionMessage")); } }

        public MessageBoxW(string Message = "", MessageDialogStyle Style = MessageDialogStyle.Affirmative, string CaptionMessage = "", string WindowTitle = "Внимание", AffNegButtonsStyle anStyle = AffNegButtonsStyle.OkCancel)
        {
            InitializeComponent();
            ANStyle = anStyle;
            DataContext = this;
            mForm.Title = WindowTitle;
            this.CaptionMessage = CaptionMessage;
            this.Message = Message;
            borderCancel.Visibility = (Style == MessageDialogStyle.Affirmative) ? Visibility.Collapsed :
                (Style == MessageDialogStyle.AffirmativeAndNegative) ? Visibility.Visible : throw new Exception($"Неверный режим работы [{Style.ToString()}]!");
        }

        public void SetBtnFocus() => borderOk.Background = selectedColor;

        private void closeWindow(object sender)
        {
            var bd = sender as System.Windows.Controls.Border;
            DialogResult = (bd == null || bd.Name != "borderOk") ? false : true;
            Close();
        }

        private void Border_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e) => 
            closeWindow(sender);

        private void borderOk_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key != System.Windows.Input.Key.Enter || borderOk.Background != selectedColor)
                return;
            DialogResult = true;
            Close();
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }

    public enum AffNegButtonsStyle { OkCancel, YesNo }
}
