using Client.Enums;
using Client.UI.Controls;
using Client.UI.Forms.WaitWindow;
using Core.EventModels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace Client.Management
{
    public class ClientManager : PropertyObject
    {
        public Dispatcher Dispatcher;
        private Visibility isVisible = Visibility.Hidden;
        private UserControl element, uInflate, uDeflate;

        public DataManager DataManager { get; private set; }
        public ControlEnum WorkMode { get; private set; }
        public Visibility IsVisible { get => isVisible; set { if (isVisible != value) { isVisible = value; raisePropertyChanged(nameof(IsVisible)); } } }
        public UserControl Element { get => element; set { if (element != value) { element = value; raisePropertyChanged(nameof(Element)); } } }

        public ClientManager(Dispatcher dispatcher)
        {
            DataManager = new DataManager();
            SetMode(ControlEnum.None);
            this.Dispatcher = dispatcher;
        }

        internal void SetMode(ControlEnum mode)
        {
            bool raise = WorkMode != mode;
            WorkMode = mode;
            switch (WorkMode)
            {
                case ControlEnum.RequestBeforeInflate:
                    fWaitWindow rw = new fWaitWindow(this);
                    rw.ShowDialog();
                    if (!string.IsNullOrEmpty(DataManager.ServerInfo.CardNumber))
                        SetMode(ControlEnum.InflateBalance);
                    break;
                case ControlEnum.InflateBalance:
                    uInflate = setUComponent(uInflate, typeof(uInflate));
                    (uInflate as uInflate).InflateMode = InflateMode.SetAmount;
                    IsVisible = Visibility.Visible;
                    break;
                case ControlEnum.DeflateBalance:
                    uDeflate = setUComponent(uDeflate, typeof(uDeflate));
                    IsVisible = Visibility.Visible;
                    break;
                default:
                    IsVisible = Visibility.Hidden;
                    break;
            }
            if (raise)
                raisePropertyChanged(nameof(WorkMode));
        }

        private UserControl setUComponent(UserControl uc, Type T)
        {
            if (uc == null)
            {
                var ctr = T.GetConstructor(Type.EmptyTypes);
                if (Dispatcher == null)
                {
                    uc = ctr.Invoke(null) as UserControl;
                    uc.DataContext = this;
                }
                else
                {
                    Dispatcher.Invoke(() =>
                    {
                        uc = ctr.Invoke(null) as UserControl;
                        uc.DataContext = this;
                    });
                }
            }
            Element = uc;
            return uc;
        }
    }
}
