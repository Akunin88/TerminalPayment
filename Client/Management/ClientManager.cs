using Client.Enums;
using Client.UI.Controls;
using Core.EventModels;
using System.Windows.Controls;
using System.Windows.Threading;

namespace Client.Management
{
    public class ClientManager : PropertyObject
    {
        private Dispatcher dispatcher;
        private UserControl element, uStartMenu, uInflate, uDeflate;

        public DataManager DataManager { get; private set; }
        public ControlEnum WorkMode { get; private set; }
        public UserControl Element { get => element; set { if (element != value) { element = value; raisePropertyChanged(nameof(Element)); } } }

        public ClientManager(Dispatcher dispatcher)
        {
            DataManager = new DataManager();
            SetMode(ControlEnum.StartMenu);
            this.dispatcher = dispatcher;
        }

        internal void SetMode(ControlEnum mode)
        {
            WorkMode = mode;
            DataManager.Refresh(mode);
            switch (WorkMode)
            {
                case ControlEnum.StartMenu:
                    DataManager.Reset();
                    uStartMenu = setUComponent(uStartMenu, typeof(uStartMenu));
                    break;
                case ControlEnum.InflateBalance:
                    uInflate = setUComponent(uInflate, typeof(uInflate));
                    break;
                case ControlEnum.DeflateBalance:
                    uDeflate = setUComponent(uDeflate, typeof(uDeflate));
                    break;
                default:
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
