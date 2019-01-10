using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ToolMgt.Common;

namespace ToolMgt.BLL
{
    public class DataEventArgs : EventArgs
    {
        private byte[] _data;
        public DataEventArgs(byte[] data)
        {
            this.Data = data;
        }

        public byte[] Data { get => _data; set => _data = value; }
    }

    public enum LightStatus
    {
        /// <summary>
        /// 关灯
        /// </summary>
        Close = 0,

        /// <summary>
        /// 开灯
        /// </summary>
        Open = 1,

        /// <summary>
        /// 闪灯
        /// </summary>
        Flash = 2
    }

    /// <summary>
    /// 传感器监控
    /// </summary>
    public class PLCHelper
    {
        /// <summary>
        /// 16个指示灯地址
        /// </summary>
        private string[] Addr_Light = new string[]
        {
            "0500","0501","0502","0503","0504","0505","0506","0507",
            "0508","0509","050A","050B","050C","050D","050E","050F"
        };
        /// <summary>
        /// 2把锁地址
        /// </summary>
        private string[] Addr_Lock = new string[] { "0510", "0511" };

        /// <summary>
        /// 绿、红、黄 报警灯地址
        /// </summary>
        private string[] Addr_AlarmLight = new string[] { "0512", "0513", "0514" };

        /// <summary>
        /// 打开、关闭 报警蜂鸣器地址
        /// </summary>
        private string[] Addr_AlarmBuzzer = new string[] { "0518", "0519" };

        /// <summary>
        /// 16个工具传感器地址
        /// </summary>
        private string[] Addr_ToolSensor = new string[]
        {
            "0400","0401","0402","0403","0404","0405","0406","0407",
            "0408","0409","040A","040B","040C","040D","040E","040F"
        };

        /// <summary>
        /// 2把锁传感器地址
        /// </summary>
        private string[] Addr_LockSensor = new string[] { "0410", "0411" };

        private string LgtOpen = "01";
        private string LgtClose = "02";
        private string LgtFlash = "03";

        /// <summary>
        /// PLC通讯地址，出厂设置为0x01
        /// </summary>
        public string ADR { get; set; } = "01";

        public PLCHelper(string adr = "01")
        {
            ADR = adr;
        }

        public EventHandler<DataEventArgs> OnReceive;

        /// <summary>
        /// 工具状态
        /// <para>参数：工位号（1-16），工位状态（true=有工具，false=无工具）。</para>
        /// </summary>
        public Action<int, bool> ToolStatusChanged;

        /// <summary>
        /// 柜门状态
        /// <para>true=开门；false=关门。</para>
        /// </summary>
        public Action DoorClosed;

        /// <summary>
        /// 16个传感器状态 true=有工具，false=无工具
        /// </summary>
        public bool[] Status = new bool[16];

        /// <summary>
        /// 暂停监控
        /// </summary>
        private bool Pause = false;

        /// <summary>
        /// 停止监控
        /// </summary>
        private bool Stop = false;

        /// <summary>
        /// 当前状态
        /// </summary>
        /// <param name="currToolStatus"></param>
        public void StartMonitor(bool[] currToolStatus)
        {
            while (true)
            {
                if (Stop)
                {
                    break;
                }
                if (Pause && !Stop)
                {
                    Thread.Sleep(100);
                    continue;
                }
                try
                {
                    //TODO:监控工具状态、柜门状态
                    //1、发送读取工具状态、柜门状态指令
                    //2、接收指令并得到16个工具结果、柜门结果
                    //3、处理结果，调用相应的回调函数
                }
                catch (Exception ex)
                {
                    LogUtil.WriteLog("StartMonitor Error", ex);
                }
            }
        }

        public void StopMonitor()
        {
            Stop = true;
        }

        public void PauseMonitor()
        {
            Pause = true;
        }

        public void SetRed(LightStatus status, int pos)
        {
            //TODO 发送红灯命令
        }
        public void SetGreen(LightStatus status, int pos)
        {
            //TODO 发送绿灯命令
        }

        public void SetAlarm(bool open)
        {
            //TODO 发送蜂鸣命令
        }

        private byte[] GetProtocol(string plcADR, string funcCode, string adr, string cmd)
        {
            return null;
        }
    }
}
