using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ToolMgt.BLL
{
    public class Status
    {
        public Status()
        {
            Lock = new bool[2];
            Tool = new bool[16];
        }

        public bool[] Lock { get; set; }
        public bool[] Tool { get; set; }
    }
    public class PLCControl
    {
        private PLCHelper plcHelper;
        public bool Connected { get; set; }
        public PLCControl(string com)
        {
            plcHelper = new PLCHelper(com);
            Connected = plcHelper.Connected;
        }

        /// <summary>
        /// 开关指示灯 1-16
        /// </summary>
        /// <param name="lightValue">"1"=亮，"0"=灭；16个灯的状态</param>
        public void OperateLight(bool[] status)
        {
            byte[] lightValue = new byte[16];
            for (int i = 0; i < status.Length && i < lightValue.Length; i++)
            {
                lightValue[i] = (byte)(status[i] ? 1 : 0);
            }
            int val = BitConverter.ToInt32(lightValue, 0);
            PLCHelper.PlcAdd startAddr = GetToolAddr(1, false);
            plcHelper.SetStart(startAddr, 16, val);
            Thread.Sleep(500);
            plcHelper.GetRecive();
        }

        /// <summary>
        /// 开指示灯1-16单独控制
        /// </summary>
        /// <param name="no">1-16</param>
        public void OpenLight(int no)
        {
            PLCHelper.PlcAdd addr = GetToolAddr(no, false);
            plcHelper.SetStart(addr, 1, 0x01);
            Thread.Sleep(500);
            plcHelper.GetRecive();
        }

        /// <summary>
        /// 关指示灯1-16单独控制
        /// </summary>
        /// <param name="no"></param>
        public void CloseLight(int no)
        {
            PLCHelper.PlcAdd addr = GetToolAddr(no, false);
            plcHelper.SetStart(addr, 1, 0x00);
            Thread.Sleep(500);
            plcHelper.GetRecive();
        }

        private bool flashstatus = false;
        private int flashno = 0;
        public void FlashLight(int no)
        {
            flashno = no;
        }

        /// <summary>
        /// 开门1-2
        /// </summary>
        /// <param name="no">1-2</param>
        public void OpenDoor(int no)
        {
            PLCHelper.PlcAdd addr = GetLockAddr(no, false);
            plcHelper.SetStart(addr, 1, 0x01);
            Thread.Sleep(500);
            plcHelper.GetRecive();
        }

        /// <summary>
        /// 红灯报警+蜂鸣
        /// </summary>
        public void OpenAlarm()
        {
            plcHelper.ItemStart(AddrLightR, 0xFF);
            Thread.Sleep(500);
            plcHelper.GetRecive();
            plcHelper.ItemStart(AddrBuzzer, 0xFF);
            Thread.Sleep(500);
            plcHelper.GetRecive();
        }

        /// <summary>
        /// 关闭报警
        /// </summary>
        public void CloseAlarm()
        {
            plcHelper.ItemStart(AddrLightR, 0x00);
            Thread.Sleep(500);
            plcHelper.GetRecive();

            plcHelper.ItemStart(AddrBuzzer, 0x00);
            Thread.Sleep(500);
            plcHelper.GetRecive();
        }

        public Status GetStatus(bool[] oriToolStatus)
        {
            plcHelper.GetStart(PLCHelper.PlcAdd.X0, 40);
            Thread.Sleep(500);
            DeltaData data = plcHelper.GetRecive();
            if (data == null)
            {
                Thread.Sleep(500);
                data = plcHelper.GetRecive();
                return null;
            }

            if (data.CMD != 0x02)
            {
                return null;
            }
            Status status = new Status();

            byte bytLock = data.DATA[1];//锁状态
            char[] arrLock = Convert.ToString(bytLock, 2).PadLeft(8, '0').Reverse().ToArray();
            status.Lock[0] = arrLock[0] == '1';
            status.Lock[1] = arrLock[1] == '1';

            byte bytTools_1 = data.DATA[3];//第一组扩展，扳手1-8
            char[] arrTools_1 = Convert.ToString(bytTools_1, 2).PadLeft(8, '0').Reverse().ToArray();

            byte bytTools_2 = data.DATA[4];//第二组扩展，扳手9-16
            char[] arrTools_2 = Convert.ToString(bytTools_2, 2).PadLeft(8, '0').Reverse().ToArray();

            List<char> toolsStatus = new List<char>();
            toolsStatus.AddRange(arrTools_1);
            toolsStatus.AddRange(arrTools_2);
            for (int i = 0; i < status.Tool.Length; i++)
            {
                status.Tool[i] = toolsStatus[i] == '1';
            }
            for (int i = 0; i < status.Tool.Length && i < oriToolStatus.Length; i++)
            {
                if (status.Tool[i] != oriToolStatus[i])
                {
                    ToolStatusChanged?.Invoke(i + 1, status.Tool[i]);//状态改变回调
                }
            }
            if (flashno > 0)
            {
                plcHelper.ItemStart(GetToolAddr(flashno, false), (short)(flashstatus ? 0xFF : 0x00));
                //plcHelper.ItemStart(PLCHelper.PlcAdd.Y27, 0xFF);
                Thread.Sleep(500);
                //var data2 = plcHelper.GetRecive();
                flashstatus = !flashstatus;
            }

            //byte bytTools_3 = data.DATA[5];//备用扩展
            //char[] arrTools_3 = Convert.ToString(bytTools_3, 2).PadLeft(8,'0').Reverse().ToArray();
            if (status.Lock[0] && status.Lock[1])
            {
                DoorClosed?.Invoke();//关门改变回调
            }

            return status;
        }

        /// <summary>
        /// 获取工具指示灯地址 或 传感器地址
        /// </summary>
        /// <param name="no">1-16</param>
        /// <param name="input">true=传感器地址；false=指示灯地址</param>
        /// <returns></returns>
        private PLCHelper.PlcAdd GetToolAddr(int no, bool input)
        {
            int iStart, iAddr;
            if (input)
                iStart = (int)PLCHelper.PlcAdd.X20;
            else
                iStart = (int)PLCHelper.PlcAdd.Y20;
            iAddr = iStart + (no - 1);
            return (PLCHelper.PlcAdd)iAddr;
        }

        /// <summary>
        /// 获取门锁地址 或 门锁反馈地址
        /// </summary>
        /// <param name="no">1-2</param>
        /// <param name="input">true=门锁；false=反馈</param>
        /// <returns></returns>
        private PLCHelper.PlcAdd GetLockAddr(int no, bool input)
        {
            int iStart, iAddr;
            if (input)
                iStart = (int)PLCHelper.PlcAdd.X0;
            else
                iStart = (int)PLCHelper.PlcAdd.Y0;
            iAddr = iStart + (no - 1);
            return (PLCHelper.PlcAdd)iAddr;
        }

        /// <summary>
        /// 绿色报警灯
        /// </summary>
        private PLCHelper.PlcAdd AddrLightG = PLCHelper.PlcAdd.Y2;
        /// <summary>
        /// 红色报警灯
        /// </summary>
        private PLCHelper.PlcAdd AddrLightR = PLCHelper.PlcAdd.Y3;
        /// <summary>
        /// 黄色报警灯
        /// </summary>
        private PLCHelper.PlcAdd AddrLightY = PLCHelper.PlcAdd.Y4;
        /// <summary>
        /// 蜂鸣器
        /// </summary>
        private PLCHelper.PlcAdd AddrBuzzer = PLCHelper.PlcAdd.Y5;

        /// <summary>
        /// 柜门全部关闭 回调
        /// </summary>
        public Action DoorClosed { get; set; }

        /// <summary>
        /// 工具状态改变 回调 &lt;位置1-16,状态&gt;
        /// </summary>
        public Action<int, bool> ToolStatusChanged;
    }
}
