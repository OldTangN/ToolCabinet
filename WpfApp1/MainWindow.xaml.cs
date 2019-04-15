using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.ServiceModel;
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

namespace WpfApp1
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            //ServiceHost host = new ServiceHost(typeof(Service1));
            //host.Open();
        }

        private void BtnGet_Click(object sender, RoutedEventArgs e)
        {
            txtRlt.Text = HttpConnectToServer(txtUrl.Text, "");
        }
        //发送消息到服务器
        private string HttpConnectToServer(string ServerPage, string strXml)
        {
            byte[] dataArray = Encoding.UTF8.GetBytes(strXml);
            //创建请求
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(ServerPage);
            request.Method = "GET";
            request.ContentType = "text/json; charset=utf-8";
            //读取返回消息
            string res = string.Empty;
            try
            {
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                res = reader.ReadToEnd();
                reader.Close();
            }
            catch (Exception ex)
            {
                throw ex;//连接服务器失败
            }
            return res;
        }
    }
}
