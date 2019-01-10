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
using ToolMgt.BLL;
using ToolMgt.UI.Common;
using ToolMgt.UI.ViewModel;

namespace ToolMgt.UI.Controls
{
    /// <summary>
    /// ToolRecordControl.xaml 的交互逻辑
    /// </summary>
    public partial class ToolRecordControl : UserControl, IDisposable
    {
        private ToolRecordViewModel viewModel;
        public ToolRecordControl()
        {
            InitializeComponent();
        }

        public void Dispose()
        {
            this.viewModel.Dispose();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            viewModel = new ToolRecordViewModel();
            this.DataContext = viewModel;
        }
    }
}
