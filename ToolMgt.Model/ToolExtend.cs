using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToolMgt.Model
{
    public partial class Tool
    {
        
        /// <summary>
        /// 是否被借用
        /// </summary>
        public bool IsBorrowed { get => isBorrowed; set => Set(ref isBorrowed, value); }

        /// <summary>
        /// 显示文本
        /// </summary>
        public string Text2 { get => text2; set => Set(ref text2, value); }

        /// <summary>
        /// 被选中
        /// </summary>
        public bool IsSelected { get => isSelected; set => Set(ref isSelected, value); }

        private bool isBorrowed = false;

        private string text2 = "";

        private bool isSelected = false;
    }
}
