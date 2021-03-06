﻿using System;
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
    /// ToolMgtControl.xaml 的交互逻辑
    /// </summary>
    public partial class ToolMgtControl : UserControl, IClose
    {
        private ToolMgtViewModel ViewModel;
        public ToolMgtControl()
        {
            InitializeComponent();
        }

        private void tb_wrenchbarcode_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void tb_min_KeyUp(object sender, KeyEventArgs e)
        {

        }

        private void tb_max_KeyUp(object sender, KeyEventArgs e)
        {

        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            ViewModel = new ToolMgtViewModel();
            this.DataContext = ViewModel;
            if (GlobalData.CurrUserRole?.RoleId == 1)
            {
                btnReset.Visibility = Visibility.Visible;
            }
        }

        private void editButtonClick(object sender, RoutedEventArgs e)
        {
            ViewModel.CurrTool = ViewModel.SelectTool;
            ViewModel.CurrType = ViewModel.ToolTypes.FirstOrDefault(p => p.id == ViewModel.CurrTool.ToolTypeId);
        }

        private void delButtonClick(object sender, RoutedEventArgs e)
        {
            ViewModel.DeleteCmd.Execute(null);
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
    }
}
