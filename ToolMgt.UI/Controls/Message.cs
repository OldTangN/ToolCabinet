using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ToolMgt.UI.Controls
{
    public static class MessageAlert
    {
        /// <summary>
        /// 错误
        /// </summary>
        /// <param name="str"></param>
        public static void Error(string str)
        {
            MessageBox.Show(str, "错误！");
        }

       /// <summary>
       /// 警告
       /// </summary>
       /// <param name="str"></param>
        public static void Warning(string str)
        {
            MessageBox.Show(str, "警告！");
        }

        /// <summary>
        /// 提示
        /// </summary>
        /// <param name="str"></param>
        public static void Alert(string str)
        {
            MessageBox.Show(str, "提示！");
        }

        /// <summary>
        /// 确认
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool Confirm(string str)
        {
            if (MessageBox.Show(str, "提示", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                return true;
            }
            return false;
        }

    }
}
