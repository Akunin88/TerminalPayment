using Client.Management;
using System.Windows;

namespace Client
{
    public partial class fMain
    {
        private ClientManager clientManager;
        private int time;
        private Timer mouseTimer;

        public fMain()
        {
            InitializeComponent();
            //this.Cursor = System.Windows.Input.Cursors.None;
            clientManager = new ClientManager(Dispatcher);
            DataContext = clientManager;
            time = clientManager.DataManager.Config.DelayForMainMenuReturnSec * 1000;
            mouseTimer = new Timer(x => noAction(), null, time, 1000);
        }

        private void noAction() => clientManager.SetMode(Enums.ControlEnum.StartMenu);

        private void Window_MouseMove(object sender, System.Windows.Input.MouseEventArgs e) => mouseTimer.Change(time, time);
    }
}