using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using ToolMgt.BLL;
using ToolMgt.Common;
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
            Application.Current.DispatcherUnhandledException += App_DispatcherUnhandledException;
            bool rlt = false;
            try
            {
                UserDao dao = new UserDao();
                rlt = dao.DataSync();
            }
            catch (Exception ex)
            {
                LogUtil.WriteLog("同步失败！", ex);
                MessageBox.Show("用户信息失败！");
            }
            if (!rlt)
            {
                MessageBox.Show("从服务器同步用户信息失败，采用本地缓存数据登录！");
            }
            View.LogInWindow win = new View.LogInWindow();
            win.Show();
        }

        private void App_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            e.Handled = true;
            MessageAlert.Alert(e.Exception.Message);
        }
    }
}
