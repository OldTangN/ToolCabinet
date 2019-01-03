using System;
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

        public static readonly DependencyProperty Text2Property = DependencyProperty.Register("Text2", typeof(string), typeof(ToolInfo), new PropertyMetadata("", new PropertyChangedCallback(Test1PropertyChangeCallback)));
        private static void Test2PropertyChangeCallback(object sender, DependencyPropertyChangedEventArgs e)
        {
            ToolInfo uc = sender as ToolInfo;
            uc?.OnText2PropertyChange(e.OldValue, e.NewValue);
        }
        protected void OnText2PropertyChange(object oldval, object newval)
        {
            Text2 = newval?.ToString();
        }
        #endregion

        //public event PropertyChangedEventHandler PropertyChanged;

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.Selected = !this.Selected;
        }

        #region Property
        private bool selected = false;

        public bool Selected
        {
            get => selected; set
            {
                if (selected != value)
                {
                    selected = value;
                    //RaisePropertyChanged("Selected");
                }
                if (selected)
                {
                    this.Background = new SolidColorBrush(Colors.Yellow);
                }
                else
                {
                    this.Background = new SolidColorBrush(Colors.White);
                }
            }
        }

        public string Text1
        {
            get => text1; set
            {
                if (!string.Equals(text1, value))
                {
                    text1 = value;
                    tb1.Text = text1;
                }
            }
        }

        private string text1;

        public string Text2
        {
            get => text2; set
            {
                if (!string.Equals(text2, value))
                {
                    text2 = value;
                    tb2.Text = text2;
                }
            }
        }

        private string text2;
        #endregion

        //private void RaisePropertyChanged(string propertyName)
        //{
        //    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName ?? ""));
        //}
    }
}
