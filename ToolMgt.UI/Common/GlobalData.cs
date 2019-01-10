using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToolMgt.Model;

namespace ToolMgt.UI.Common
{
    public class GlobalData
    {
        /// <summary>
        /// 当前用户
        /// </summary>
        public static User CurrUser = null;

        /// <summary>
        /// 当前选择扳手
        /// </summary>
        public static Tool CurrTool = null;

        /// <summary>
        /// 被选工具是否取出/放入
        /// </summary>
        public static bool SelectToolCorrect = false;
    }
}
