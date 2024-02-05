using Client.Enums;
using Core.Configuration;
using Core.EventModels;
using Core.Models;

namespace Client.Management
{
    public class DataManager : PropertyObject
    {
        private string title;

        public string Title { get => title; set { if (title != value) { title = value; raisePropertyChanged(nameof(Title)); } } }
        public PaymentInfo UserInfo { get; set; } = new PaymentInfo();
        public PaymentInfo ServerInfo { get; set; } = new PaymentInfo();

        public Config Config { get; set; }


        public DataManager()
        {
            Config = Config.Read();

            setReceiver();
        }

        public void Refresh(ControlEnum controlEnum)
        {
            switch (controlEnum)
            {
                case ControlEnum.InflateBalance:
                    Title = "Для пополнения сделайте перевод на эту карту";
                    break;
                case ControlEnum.DeflateBalance:
                    Title = "Для получения выйгрыша введите номер телефона и карты";
                    break;
                default:
                    Title = string.Empty;
                    break;
            }
        }

        public void Reset()
        {
            Title = string.Empty;
            UserInfo.Clear();
            ServerInfo.Clear();
            setReceiver();
        }

        [Obsolete("TODO receive from server")]
        private void setReceiver()
        {
            ServerInfo.CardNumber = "5555";
        }

        public string SendMoneyToUser()
        {

            //TODO some communication with server + send money to user
            //Config.Read().TerminalId
            Thread.Sleep(3000);

            return "Деньги отправлены на указанную карту";
        }

        internal string SendMoneyToServer()
        {
            //TODO some communication with server + receive money from user
            //Config.Read().TerminalId
            Thread.Sleep(3000);

            return "Платеж успешно принят";
        }
    }
}
