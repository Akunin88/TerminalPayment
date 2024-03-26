using Client.Enums;
using Core.Configuration;
using Core.EventModels;
using Core.Models;
using System.Globalization;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text;
using System.Xml;

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
            ServerInfo.CardNumber = string.Empty;
        }

        public bool Refresh(ControlEnum controlEnum)
        {
            switch (controlEnum)
            {
                case ControlEnum.InflateBalance:
                    Title = "Для пополнения сделайте перевод на эту карту";
                    XmlElement? xmlElement = doRequest($"<getdepositcard id=\"{this.Config.TerminalId}\" />");
                    ServerInfo.CardNumber = xmlElement != null && xmlElement.Name == "getdepositcard" && xmlElement.Attributes["ok"].Value == "1" ?
                        xmlElement.Attributes["card"].Value : string.Empty;
                    return !(string.IsNullOrEmpty(ServerInfo.CardNumber));
                case ControlEnum.DeflateBalance:
                    Title = "Для получения выйгрыша введите номер телефона и карты";
                    return true;
                default:
                    Title = string.Empty;
                    return false;
            }
        }

        public void Reset()
        {
            Title = string.Empty;
            UserInfo.Clear();
            ServerInfo.Clear();
            ServerInfo.CardNumber = string.Empty;
        }

        public string SendMoneyToUser()
        {
            try
            {
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append(string.Format(CultureInfo.InvariantCulture, "<savebalance id=\"{0}\" val=\"{1:0.00}\" phone=\"{2}\" dt=\"{3}\" dt_loc=\"{6}\" tag=\"{4}\" code=\"{5}\" card=\"{7}\"/>", new object[]
                {
                    this.Config.TerminalId,
                    this.UserInfo.Amount,
                    this.UserInfo.PhoneNumber,
                    DateTime.Now.ToString("yyyyMMddHHmmss"),
                    null,
                    "",
                    DateTime.Now.ToString("yyyyMMddHHmmss"),
                    this.UserInfo.CardNumber
                }));
                XmlElement? xmlElement = doRequest(stringBuilder.ToString());
                return xmlElement != null && xmlElement.Name == "savebalance" && xmlElement.Attributes["ok"].Value == "1" ?
                    "Деньги отправлены на указанную карту" : "Нет сети!";
            }
            catch
            {
                return "Нет сети!";
            }
            finally
            {
                UserInfo.Clear();
            }
        }

        internal string SendMoneyToServer()
        {
            if (string.IsNullOrEmpty(ServerInfo.CardNumber))
                return "Нет интернета!";
            try
            {
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append(string.Format(CultureInfo.InvariantCulture, "<deposit id=\"{0}\" val=\"{1}\" dt=\"{2:G}\" card=\"{3}\"/>", new object[]
                {
                    this.Config.TerminalId,
                    this.ServerInfo.Amount,
                    DateTime.Now,
                    this.ServerInfo.CardNumber
                }));
                string text = stringBuilder.ToString();
                XmlElement? xmlElement = doRequest(text);
                return xmlElement != null && xmlElement.Name == "deposit" /*&& xmlElement.Attributes["ok"].Value == "1"*/ ?
                    "Платеж успешно принят" : "Нет интернета!";
            }
            catch
            {
                return "Нет интернета!";
            }
            finally
            {
                ServerInfo.Clear();
            }
        }

        private static object sync = new object();
        private XmlElement? doRequest(string command)
        {
            XmlElement? xmlElement;
            lock (sync)
            {
                try
                {
                    HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(Config.ServerAddress);
                    httpWebRequest.Timeout = Config.ServerRequestTimeoutSec * 1000;
                    httpWebRequest.Method = "POST";
                    httpWebRequest.UserAgent = $"GMT {Assembly.GetExecutingAssembly().GetName().Version} {this.Config.TerminalId}";
                    byte[] bytes = Encoding.UTF8.GetBytes(command);
                    using (Stream requestStream = httpWebRequest.GetRequestStream())
                        requestStream.Write(bytes, 0, bytes.Length);
                    using (Stream responseStream = httpWebRequest.GetResponse().GetResponseStream())
                    using (StreamReader streamReader = new StreamReader(responseStream, Encoding.UTF8))
                    using (XmlReader xmlReader = XmlReader.Create(new StringReader(streamReader.ReadLine())))
                    {
                        XmlDocument xmlDocument = new XmlDocument();
                        xmlDocument.Load(xmlReader);
                        xmlElement = xmlDocument.DocumentElement;
                    }
                }
                catch
                {
                    xmlElement = null;
                }
            }
            return xmlElement;
        }
    }
}
