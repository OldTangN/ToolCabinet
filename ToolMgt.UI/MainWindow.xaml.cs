using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using ToolMgt.BLL;
using ToolMgt.UI.Common;
using ToolMgt.UI.Controls;
using ToolMgt.UI.View;
using ToolMgt.UI.ViewModel;
namespace ToolMgt.UI
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : MetroWindow, IDisposable
    {
        private MainViewModel viewModel;
        private PLCControl PLC;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (GlobalData.CurrUserRole.RoleId == 1)
            {
                btnUserSearch.Visibility = Visibility.Visible;
                btnToolSearch.Visibility = Visibility.Visible;
                btnToolType.Visibility = Visibility.Visible;
                btnSysConfig.Visibility = Visibility.Visible;
            }
            PLC = new PLCControl(SysConfiguration.PLCCom);
            viewModel = new MainViewModel(PLC);
            viewModel.OnDoorClose += DoorClose;
            viewModel.OnInitComplete += InitComplete;
            this.DataContext = viewModel;
            viewModel.Init();
        }

        private void InitComplete()
        {
            SelectToolControl control = new SelectToolControl(viewModel.Tools);
            ChangeView(control);
        }

        private void DoorClose()
        {
            this.Dispatcher.Invoke(() => { this.Close(); });
        }

        private void ChangeView(UserControl control)
        {
            foreach (IDisposable item in gridContainer.Children)
            {
                item?.Dispose();
            }
            gridContainer.Children.Clear();
            gridContainer.Children.Add(control);
        }

        private void btnUserSearch_Click(object sender, RoutedEventArgs e)
        {
            UserMgtControl control = new UserMgtControl();
            ChangeView(control);
        }

        private void btnToolSearch_Click(object sender, RoutedEventArgs e)
        {
            ToolMgtControl control = new ToolMgtControl();
            ChangeView(control);
        }

        private void btnToolType_Click(object sender, RoutedEventArgs e)
        {
            ToolTypeMgtControl control = new ToolTypeMgtControl();
            ChangeView(control);
        }

        private void btnSysConfig_Click(object sender, RoutedEventArgs e)
        {
            SysConfigControl control = new SysConfigControl();
            ChangeView(control);
        }

        private void btnBorrow_Click(object sender, RoutedEventArgs e)
        {
            SelectToolControl control = new SelectToolControl(viewModel.Tools);
            ChangeView(control);
        }

        public void Dispose()
        {
            this.viewModel.Dispose();
        }

        private void MetroWindow_Closed(object sender, EventArgs e)
        {

        }

        private void MetroWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            viewModel.Dispose();
            LogInWindow win = new LogInWindow();
            win.Show();
        }

        private void btnRecord_Click(object sender, RoutedEventArgs e)
        {
            ToolRecordControl control = new ToolRecordControl();
            ChangeView(control);
        }

        private void BtnExit_Click(object sender, RoutedEventArgs e)
        {
            viewModel.SaveRecord();
            this.Close();
        }
    }
}
