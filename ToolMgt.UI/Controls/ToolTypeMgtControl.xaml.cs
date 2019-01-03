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
using ToolMgt.UI.ViewModel;

namespace ToolMgt.UI.Controls
{
    /// <summary>
    /// ToolTypeMgtControl.xaml 的交互逻辑
    /// </summary>
    public partial class ToolTypeMgtControl : UserControl, IDisposable
    {
        ToolTypeMgtViewModel ViewModel;
        public ToolTypeMgtControl()
        {
            InitializeComponent();
        }

        private void editButtonClick(object sender, RoutedEventArgs e)
        {
            ViewModel.CurrType = ViewModel.SelectType;
        }

        private void delButtonClick(object sender, RoutedEventArgs e)
        {
            ViewModel.DeleteCmd.Execute(null);
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            ViewModel = new ToolTypeMgtViewModel();
            this.DataContext = ViewModel;
        }

        public void Dispose()
        {
        }
    }
}
