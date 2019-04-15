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
using ToolMgt.UI.Common;
using ToolMgt.UI.ViewModel;

namespace ToolMgt.UI.Controls
{
    /// <summary>
    /// UserMgtControl.xaml 的交互逻辑
    /// </summary>
    public partial class UserMgtControl : UserControl, IClose
    {
        public UserMgtControl()
        {
            InitializeComponent();
        }

        private void btn_Search_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btn_Sync_Click(object sender, RoutedEventArgs e)
        {

        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            ViewModel = new UserMgtViewModel();
            this.DataContext = ViewModel;
        }

        public void CDispose()
        {
            try
            {
                ViewModel.CDispose();
                ViewModel = null;
            }
            catch (Exception)
            {
            }
        }

        UserMgtViewModel ViewModel;
    }
}
