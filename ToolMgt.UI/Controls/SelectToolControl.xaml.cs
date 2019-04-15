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
using ToolMgt.Model;
using ToolMgt.UI.Common;
using ToolMgt.UI.ViewModel;

namespace ToolMgt.UI.Controls
{
    /// <summary>
    /// SelectToolControl.xaml 的交互逻辑
    /// </summary>
    public partial class SelectToolControl : UserControl, IClose
    {
        private List<Tool> Tools;
        public SelectToolControl(List<Tool> tools)
        {
            Tools = tools;
            InitializeComponent();
        }

        private SelectToolViewModel viewModel;

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            viewModel = new SelectToolViewModel(Tools);
            this.DataContext = viewModel;
        }

        public void CDispose()
        {
            try
            {
                this.viewModel.CDispose();
                this.viewModel = null;
            }
            catch (Exception)
            {
            }
        }
    }
}
