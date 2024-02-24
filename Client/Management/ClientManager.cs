using Client.Enums;
using Client.UI.Controls;
using Core.EventModels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace Client.Management
{
    public class ClientManager : PropertyObject
    {
        private Dispatcher dispatcher;
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
            this.dispatcher = dispatcher;
        }

        internal void SetMode(ControlEnum mode)
        {
            WorkMode = mode;
            DataManager.Refresh(mode);
            DataManager.Reset();
            switch (WorkMode)
            {
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
            raisePropertyChanged(nameof(WorkMode));
        }

        private UserControl setUComponent(UserControl uc, Type T)
        {
            if (uc == null)
            {
                var ctr = T.GetConstructor(Type.EmptyTypes);
                if (dispatcher == null)
                {
                    uc = ctr.Invoke(null) as UserControl;
                    uc.DataContext = this;
                }
                else
                {
                    dispatcher.Invoke(() =>
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
