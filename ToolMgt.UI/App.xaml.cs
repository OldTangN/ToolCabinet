using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.ServiceModel;
using System.Threading.Tasks;
using System.Windows;
using ToolMgt.BLL;
using ToolMgt.Common;
using ToolMgt.UI.Common;
using ToolMgt.UI.Controls;
namespace ToolMgt.UI
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        public static ServiceHost host;
        public static PLCControl PLC;
        private App()
        {
            Application.Current.DispatcherUnhandledException += App_DispatcherUnhandledException;
            Application.Current.Exit += Current_Exit;
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
            PLC = new PLCControl(SysConfiguration.PLCCom);
            host = new ServiceHost(typeof(Service1));//, new Uri("net.tcp://localhost/MyService")
            try
            {
                host.Open();
            }
            catch (Exception ex)
            {
                MessageAlert.Alert("远程服务启动失败！");
                LogUtil.WriteLog("WCF服务绑定失败！", ex);
            }
            View.LogInWindow win = new View.LogInWindow(PLC);
            win.Show();
        }

        private void Current_Exit(object sender, ExitEventArgs e)
        {
            try
            {
                host.Close();
            }
            catch (Exception)
            {
            }
        }

        private void App_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            e.Handled = true;
            LogUtil.WriteLog("未处理异常！", e.Exception);
            MessageAlert.Alert(e.Exception.Message);
        }
    }
}
