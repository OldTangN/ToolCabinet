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
        /// 显示文本--空闲或领用人姓名
        /// </summary>
        public string Text2 { get => text2; set => Set(ref text2, value); }

        /// <summary>
        /// 显示文本--量程
        /// </summary>
        public string Text3 { get => text3; set => Set(ref text3, value); }

        /// <summary>
        /// 被选中
        /// <para>当前用户已借出工具，在还工具时默认选中。</para>
        /// <para>当前用户借工具时选择工具。</para>
        /// </summary>
        public bool IsSelected { get => isSelected; set => Set(ref isSelected, value); }


        private bool _OriStatus;
        /// <summary>
        /// 初始工具状态
        /// </summary>
        public bool OriStatus { get => _OriStatus; set => Set(ref _OriStatus, value); }

        /// <summary>
        /// 能否被选择
        /// </summary>
        public bool CanSelected { get => canSelected; set => Set(ref canSelected, value); }

        private string text2 = "";
        private string text3 = "";
        private bool canSelected = true;
        private bool isSelected = false;
    }
}
