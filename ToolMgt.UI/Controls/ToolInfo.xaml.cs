﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace ToolMgt.UI.Controls
{
    /// <summary>
    /// ToolInfo.xaml 的交互逻辑
    /// </summary>
    public partial class ToolInfo : UserControl//, INotifyPropertyChanged
    {
        public ToolInfo()
        {
            InitializeComponent();
        }

        #region DependencyProperty
        /// <summary>
        /// 依赖属性 Selected
        /// </summary>
        public static readonly DependencyProperty SelectedProperty = DependencyProperty.Register("Selected", typeof(bool), typeof(ToolInfo), new PropertyMetadata(false, new PropertyChangedCallback(SelectedPropertyChangedCallback)));
        private static void SelectedPropertyChangedCallback(object sender, DependencyPropertyChangedEventArgs e)
        {
            ToolInfo uc = sender as ToolInfo;
            uc?.OnSelectedPropertyChanged(e.OldValue, e.NewValue);
        }

        protected void OnSelectedPropertyChanged(object oldval, object newval)
        {
            this.Selected = (bool)newval;
        }

        public static readonly DependencyProperty Text1Property = DependencyProperty.Register("Text1", typeof(string), typeof(ToolInfo), new PropertyMetadata("", new PropertyChangedCallback(Test1PropertyChangeCallback)));
        private static void Test1PropertyChangeCallback(object sender, DependencyPropertyChangedEventArgs e)
        {
            ToolInfo uc = sender as ToolInfo;
            uc?.OnText1PropertyChange(e.OldValue, e.NewValue);
        }
        protected void OnText1PropertyChange(object oldval, object newval)
        {
            Text1 = newval?.ToString();
        }

        public static readonly DependencyProperty Text2Property = DependencyProperty.Register("Text2", typeof(string), typeof(ToolInfo), new PropertyMetadata("", new PropertyChangedCallback(Test2PropertyChangeCallback)));
        private static void Test2PropertyChangeCallback(object sender, DependencyPropertyChangedEventArgs e)
        {
            ToolInfo uc = sender as ToolInfo;
            uc?.OnText2PropertyChange(e.OldValue, e.NewValue);
        }
        protected void OnText2PropertyChange(object oldval, object newval)
        {
            Text2 = newval?.ToString();
        }


        public static readonly DependencyProperty Text3Property = DependencyProperty.Register("Text3", typeof(string), typeof(ToolInfo), new PropertyMetadata("", new PropertyChangedCallback(Test3PropertyChangeCallback)));
        private static void Test3PropertyChangeCallback(object sender, DependencyPropertyChangedEventArgs e)
        {
            ToolInfo uc = sender as ToolInfo;
            uc?.OnText3PropertyChange(e.OldValue, e.NewValue);
        }
        protected void OnText3PropertyChange(object oldval, object newval)
        {
            Text3 = newval?.ToString();
        }
        #endregion

        //public event PropertyChangedEventHandler PropertyChanged;


        #region Property
        private bool selected = false;

        public bool Selected
        {
            get
            {
                selected = (bool)this.GetValue(SelectedProperty);
                return selected;
            }
            set
            {
                if (selected != value)
                {
                    selected = value;
                    this.SetValue(SelectedProperty, value);
                }
                if (selected)
                {
                    border.Background = new SolidColorBrush(Colors.Yellow);
                }
                else
                {
                    border.Background = new SolidColorBrush(Colors.LightGreen);
                }
            }
        }

        public string Text1
        {
            get => text1;
            set
            {
                if (!string.Equals(text1, value))
                {
                    text1 = value;
                    tb1.Text = text1;
                    this.SetValue(Text1Property, value);
                }
            }
        }

        private string text1;

        public string Text2
        {
            get => text2;
            set
            {
                if (!string.Equals(text2, value))
                {
                    text2 = value;
                    tb2.Text = text2;
                    this.SetValue(Text2Property, value);
                }
            }
        }

        private string text2;


        public string Text3
        {
            get => text3;
            set
            {
                if (!string.Equals(text3, value))
                {
                    text3 = value;
                    tb3.Text = text3;
                    this.SetValue(Text3Property, value);
                }
            }
        }

        private string text3;


        private void UserControl_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.Selected = !this.Selected;
        }
        #endregion

        //private void RaisePropertyChanged(string propertyName)
        //{
        //    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName ?? ""));
        //}
    }
}
