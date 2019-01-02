using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToolMgt.BLL;
using ToolMgt.Common;
using ToolMgt.Model;

namespace ToolMgt.UI.ViewModel
{
    public class ViewModelBase : ObservableObject
    {
        public ViewModelBase()
        {

        }

        private bool _isBusy;

        /// <summary>
        /// 忙碌状态
        /// </summary>
        public bool IsBusy { get => _isBusy; set => Set(ref _isBusy, value); }
    }
}
