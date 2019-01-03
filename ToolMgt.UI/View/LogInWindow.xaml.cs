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
using System.Windows.Shapes;
using ToolMgt.UI.ViewModel;

namespace ToolMgt.UI.View
{
    /// <summary>
    /// LogInWindow.xaml 的交互逻辑
    /// </summary>
    public partial class LogInWindow : MetroWindow, IDisposable
    {
        private LogInViewModel viewModel;
        public LogInWindow()
        {
            InitializeComponent();
        }

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            viewModel = new LogInViewModel();
            viewModel.OnLogIn += LogIn;
            this.DataContext = viewModel;
        }

        private void LogIn()
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }

        private void txtName_GotFocus(object sender, RoutedEventArgs e)
        {
            txtName.Text = "";
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void pbPwd_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                viewModel.LogInCmd.Execute(this);
            }
        }

        private void pbPwd_GotFocus(object sender, RoutedEventArgs e)
        {
            pbPwd.Password = "";
        }

        private void pbPwd_PasswordChanged(object sender, RoutedEventArgs e)
        {
            txtPwd.Text = pbPwd.Password;
        }

        private void txtName_KeyDown(object sender, KeyEventArgs e)
        {
            txtPwd.Focus();
            txtPwd.SelectAll();
        }

        public void Dispose()
        {
        }
    }
}
