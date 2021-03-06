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
    /// SysConfigControl.xaml 的交互逻辑
    /// </summary>
    public partial class SysConfigControl : UserControl, IClose
    {
        SysConfigViewModel ViewModel;
        public SysConfigControl()
        {
            InitializeComponent();
            ViewModel = new SysConfigViewModel();
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
    }
}
