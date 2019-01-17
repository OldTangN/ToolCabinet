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
using ToolMgt.BLL;
using ToolMgt.Common;

namespace ToolMgt.UI.View
{
    /// <summary>
    /// PLCTestWindow.xaml 的交互逻辑
    /// </summary>
    public partial class PLCTestWindow : Window
    {
        public PLCTestWindow()
        {
            InitializeComponent();
            this.Closing += PLCTestWindow_Closing;
        }

        private void PLCTestWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            plcHelper.DisConnect();
        }

        private PLCHelper plcHelper;
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            List<PLCHelper.PlcAdd> datas = new List<PLCHelper.PlcAdd>()
            {
                PLCHelper.PlcAdd.Y0,
                PLCHelper.PlcAdd.Y1,
                PLCHelper.PlcAdd.Y2,
                PLCHelper.PlcAdd.Y3,
                PLCHelper.PlcAdd.Y20,
                PLCHelper.PlcAdd.Y21,
                PLCHelper.PlcAdd.Y22,
                PLCHelper.PlcAdd.Y23,
                PLCHelper.PlcAdd.Y30,
                PLCHelper.PlcAdd.Y31,
                PLCHelper.PlcAdd.Y32,
                PLCHelper.PlcAdd.Y33,
                PLCHelper.PlcAdd.Y40,
                PLCHelper.PlcAdd.Y41,
                PLCHelper.PlcAdd.Y42,
                PLCHelper.PlcAdd.Y43,

            };
            cmbAddr.ItemsSource = datas;
        }

        private void BtnConnect_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                plcHelper = new PLCHelper(txtCom.Text);
                if (plcHelper.Connected)
                {
                    MessageBox.Show("打开端口成功！");
                }
                else
                {
                    MessageBox.Show("打开端口失败！");
                }
            }
            catch (Exception ex)
            {
                LogUtil.WriteLog("连接PLC失败！", ex);
                MessageBox.Show("连接PLC失败！");
            }
        }

        private void BtnSend_Click(object sender, RoutedEventArgs e)
        {
            if (plcHelper == null)
            {
                MessageBox.Show("PLC未连接！");return;
            }
            plcHelper.SetStart((PLCHelper.PlcAdd)cmbAddr.SelectedItem, 1, Convert.ToInt32(txtValue.Text));
        }

        private void BtnCloseTool_Click(object sender, RoutedEventArgs e)
        {
            plcHelper.SetStart(PLCHelper.PlcAdd.Y20, 16, 0);
        }
    }
}
