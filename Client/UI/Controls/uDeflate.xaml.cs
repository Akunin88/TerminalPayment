using Client.Enums;
using Client.Management;
using Client.UI.Forms.WaitWindow;
using Core.Models;
using System.Windows.Controls;
using System.Windows.Input;

namespace Client.UI.Controls
{
    public partial class uDeflate : UserControl
    {
        private bool isPhoneInputMode = true;
        private ClientManager manager => this.DataContext as ClientManager;

        public uDeflate()
        {
            InitializeComponent();
        }

        private void apply_Click(object sender, MouseButtonEventArgs e)
        {
            fWaitWindow rw = new fWaitWindow(manager);
            rw.ShowDialog();
        }

        private void cancel_Click(object sender, MouseButtonEventArgs e) => manager?.SetMode(ControlEnum.None);

        private void tbMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (sender is TextBox tb)
                isPhoneInputMode = tb.Name == "tbPhone";
        }

        private void key_Pressed(object sender, char e)
        {
            if (manager == null)
                return;
            PaymentInfo userInfo = manager.DataManager.UserInfo;
            switch (e)
            {
                case 'C':
                    if (isPhoneInputMode)
                        userInfo.PhoneNumber = string.Empty;
                    else
                        userInfo.CardNumber = string.Empty;
                    break;
                case '←':
                    if (isPhoneInputMode)
                    {
                        if (userInfo.PhoneNumber.Length > 0)
                            userInfo.PhoneNumber = userInfo.PhoneNumber.Substring(0, userInfo.PhoneNumber.Length - 1);
                    }
                    else
                        if (userInfo.CardNumber.Length > 0)
                        userInfo.CardNumber = userInfo.CardNumber.Substring(0, userInfo.CardNumber.Length - 1);
                    break;
                default:
                    if (isPhoneInputMode)
                    {
                        if (userInfo.PhoneNumber.Length < 12)
                            userInfo.PhoneNumber += e;
                    }
                    else
                        if (userInfo.CardNumber.Length < 16)
                        userInfo.CardNumber += e;
                    break;
            }
        }
    }
}
