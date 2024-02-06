using Client.Management;
using System.Windows;

namespace Client
{
    public partial class fMain
    {
        public ClientManager ClientManager;
        private int time;
        private Timer mouseTimer;

        public fMain()
        {
            InitializeComponent();
            this.Cursor = System.Windows.Input.Cursors.None;
            ClientManager = new ClientManager(Dispatcher);
            DataContext = ClientManager;
            time = ClientManager.DataManager.Config.DelayForMainMenuReturnSec * 1000;
            mouseTimer = new Timer(x => noAction(), null, time, 1000);
        }

        private void noAction() => ClientManager.SetMode(Enums.ControlEnum.None);

        private void Window_MouseMove(object sender, System.Windows.Input.MouseEventArgs e) => mouseTimer.Change(time, time);
    }
}