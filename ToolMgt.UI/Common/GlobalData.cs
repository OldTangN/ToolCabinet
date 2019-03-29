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
        /// 当前用户角色 roleid=1是管理员
        /// </summary>
        public static UserRole CurrUserRole = null;
    }
}
