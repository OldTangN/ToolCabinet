using MahApps.Metro.Controls;
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
using ToolMgt.UI.Controls;
using ToolMgt.UI.View;
using ToolMgt.UI.ViewModel;
using MahApps.Metro.Controls.Dialogs;

namespace ToolMgt.UI
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : MetroWindow, IDisposable
    {
        private MainViewModel viewModel;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            viewModel = new MainViewModel();
            this.DataContext = viewModel; 
        }

        private void ChangeView(UserControl control)
        {
            foreach (UserControl item in gridContainer.Children)
            {
                item.Dispatcher.InvokeShutdown();
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
            SelectToolControl control = new SelectToolControl();
            ChangeView(control);
        }

        private void btnReturn_Click(object sender, RoutedEventArgs e)
        {

        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
