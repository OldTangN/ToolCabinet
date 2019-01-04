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
        /// 显示文本
        /// </summary>
        public string Text2 { get => text2; set => Set(ref text2, value); }

        /// <summary>
        /// 被选中
        /// <para>当前用户已借出工具，在还工具时默认选中。</para>
        /// <para>当前用户借工具时选择工具。</para>
        /// </summary>
        public bool IsSelected { get => isSelected; set => Set(ref isSelected, value); }

        /// <summary>
        /// 能否被选择
        /// </summary>
        public bool CanSelected { get => canSelected; set => Set(ref canSelected, value); }

        private string text2 = "";
        private bool canSelected = true;
        private bool isSelected = false;
    }
}
