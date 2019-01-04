using System;
using System.Collections.Generic;
using System.IO.Ports;

namespace ToolMgt.BLL
{
    public class ComParmater
    {
        public ComParmater()
        { }

        public ComParmater(string _PortName, int _BaudRate, Parity _Parity, int _DataBits, StopBits _StopBits)
        {
            this.PortName = _PortName;
            this.BaudRate = _BaudRate;
            this.Parity = _Parity;
            this.DataBits = _DataBits;
            this.StopBits = _StopBits;
        }
        /// <summary>
        /// COM1,COM2 etc ---1表示RS485-1，2表示RS485-2，3表示红外 ，4表示模块通信
        /// </summary>
        public string PortName { set; get; } = "";

        /// <summary>
        /// 波特率
        /// </summary>
        public int BaudRate { set; get; } = 2400;

        /// <summary>
        /// 校验位
        /// </summary>
        public Parity Parity { set; get; } = Parity.Even;

        /// <summary>
        /// 比特位
        /// </summary>
        public int DataBits { set; get; } = 8;

        /// <summary>
        /// 停止位
        /// </summary>
        public StopBits StopBits { set; get; } = StopBits.One;

        ///// <summary>
        ///// 发送数据
        ///// </summary>
        //public List<byte> SendData { set; get; } = new List<byte>();

        ///// <summary>
        ///// 接收数据
        ///// </summary>
        //public List<byte> RecData { set; get; } = new List<byte>();

        ///// <summary>
        ///// 接收次数
        ///// </summary>
        //public int RcvCount { set; get; } = 3;
    }

    public class DataEventArgs : EventArgs
    {
        public byte[] data { set; get; }
         
        public DataEventArgs(byte[] _data) : base()
        {
            this.data = _data; 
        }
    }
}
