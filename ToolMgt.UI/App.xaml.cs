using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using ToolMgt.UI.Controls;
namespace ToolMgt.UI
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        App()
        {
            this.DispatcherUnhandledException += App_DispatcherUnhandledException;
            ToolMgt.UI.View.LogInWindow win = new View.LogInWindow();
            win.Show();
        }

        private void App_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            e.Handled = true;
            MessageAlert.Alert(e.Exception.Message);
        }
    }
}
