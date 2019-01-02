using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ToolMgt.BLL.ICCard
{
    public interface ICardHelper
    {
        /// <summary>
        /// 处理数据事件
        /// </summary>
        event EventHandler<CardEventArgs> HandDataBack;

        /// <summary>
        /// 打开状态
        /// </summary>
        /// <returns></returns>
        bool IsOpen();

        /// <summary>
        /// 开始循环读取
        /// </summary>
        void Read();

        /// <summary>
        /// 关闭
        /// </summary>
        void Close();

    }
}
