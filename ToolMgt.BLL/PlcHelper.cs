using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
namespace ToolMgt.BLL
{
 
    /// <summary>
    /// 传感器监控
    /// </summary>
    public class PLCHelper
    {
        private string[] Addr_Light = new string[]
        {
            "0500","0501","0502","0503","0504","0505","0506","0507",
            "0508","0509","050A","050B","050C","050D","050E","050F"
        };
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
        public Action<bool> DoorStatusChanged;

        /// <summary>
        /// 16个传感器状态 true=有工具，false=无工具
        /// </summary>
        public bool[] Status = new bool[16];

        public bool Pause { get; set; }

        private bool IsBusy = false;

        /// <summary>
        /// 当前状态
        /// </summary>
        /// <param name="currStatus"></param>
        public void StartMonitor(bool[] currStatus)
        {
            while (true)
            {
                if (Pause)
                {
                    Thread.Sleep(100);
                    continue;
                }
                //TODO:监控工具状态、柜门状态
                //1、发送读取工具状态、柜门状态指令
                //2、接收指令并得到16个工具结果、柜门结果
                //3、处理结果，调用相应的回调函数
            }
        }

        public void Stop()
        {

        }

        public void OpenRed(int pos)
        {

        }
        public void CloseRed(int pos)
        {

        }
        public void OpenYellow(int pos)
        {

        }
        public void CloseYellow(int pos)
        {

        }
        public void OpenGreen(int pos)
        {

        }
        public void CloseGreen(int pos)
        {

        }
        public void OpenAlarm(int pos)
        {

        }
        public void CloseAlarm()
        {

        }

        private byte[] GetProtocol(string plcADR,string funcCode,string adr,string cmd)
        {
            return null;
        }
    }
}
