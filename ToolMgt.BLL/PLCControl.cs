using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ToolMgt.Common;

namespace ToolMgt.BLL
{
    public class Status
    {
        public Status()
        {
            Lock = new bool[2];
            Tool = new bool[16];
        }

        /// <summary>
        /// true=开锁
        /// </summary>
        public bool[] Lock { get; set; }
        public bool[] Tool { get; set; }
    }
    public class PLCControl
    {
        /// <summary>
        /// 单线程锁，否则开锁后，锁状态可能读的原始数据
        /// </summary>
        private readonly object lockobj = new object { };
        private PLCHelper plcHelper;
        public bool Connected { get; set; }
        public PLCControl(string com)
        {
            plcHelper = new PLCHelper(com);
            Connected = plcHelper.Connected;
        }

        #region 工具指示灯控制
        /// <summary>
        /// 开关指示灯 1-16
        /// </summary>
        /// <param name="lightValue">"1"=亮，"0"=灭；16个灯的状态</param>
        public void SetToolLamp(bool[] status = null)
        {
            string strval = "";
            if (status == null || status.Length == 0)
            {
                strval = "0".PadLeft(16, '0');
            }
            else
            {
                for (int i = 0; i < status.Length; i++)
                {
                    strval = (status[i] ? "1" : "0") + strval;
                }
            }
            //lock (lockobj)
            {
                try
                {
                    int val = Convert.ToInt32(strval, 2);
                    PLCHelper.PlcAdd startAddr = GetToolAddr(1, false);
                    plcHelper.SetStart(startAddr, 16, val);
                    Thread.Sleep(50);
                    //plcHelper.GetRecive();
                }
                catch (Exception ex)
                {
                    LogUtil.WriteLog("设置工具指示灯失败！", ex);
                }
            }
        }

        /// <summary>
        /// 设置指示灯1-16单独控制
        /// </summary>
        /// <param name="no"></param>
        /// <param name="open"></param>
        public void SetToolLamp(int no, bool open)
        {
            //lock (lockobj)
            {
                try
                {
                    PLCHelper.PlcAdd addr = GetToolAddr(no, false);
                    plcHelper.SetStart(addr, 1, open ? 0x01 : 0x00);
                    Thread.Sleep(50);
                    //plcHelper.GetRecive();
                }
                catch (Exception ex)
                {
                    LogUtil.WriteLog("控制工具指示灯失败！", ex);
                }
            }
        }

        private bool flashstatus = false;
        private List<int> flashno = new List<int>();
        public void FlashToolLamp(int no, bool flash)
        {
            if (flash)
            {
                if (!flashno.Contains(no))
                {
                    flashno.Add(no);
                }
            }
            else
            {
                if (flashno.Contains(no))
                {
                    flashno.Remove(no);
                }
            }
        }

        #endregion

        #region 开门
        /// <summary>
        /// 开门1-2
        /// </summary>
        /// <param name="no">1-2</param>
        /// <param name="waitTime">开门等待时间 单位：秒(s)</param>
        public void OpenDoor(int no, int waitTime)
        {
            //LogUtil.WriteLog("开门：" + no);
            PLCHelper.PlcAdd addr = GetLockAddr(no, false);
            //lock (lockobj)
            {
                try
                {
                    plcHelper.SetStart(addr, 1, 0x01);
                    Thread.Sleep(50);
                    //plcHelper.GetRecive();
                }
                catch (Exception ex)
                {
                    LogUtil.WriteLog("开门失败！", ex);
                }
            }
            Thread lockThread = new Thread(() =>
            {
                Thread.Sleep(waitTime * 1000);
                //lock (lockobj)
                {
                    try
                    {
                        plcHelper.SetStart(addr, 1, 0x00);
                        Thread.Sleep(50);
                        //plcHelper.GetRecive();
                    }
                    catch (Exception ex)
                    {
                        LogUtil.WriteLog("锁门失败！", ex);
                    }
                }
            });
            lockThread.Start();
        }
        #endregion

        #region 报警+蜂鸣
        private bool IsOpenAlarm = false;
        private bool AlarmStatus = false;
        /// <summary>
        /// 红灯报警+蜂鸣
        /// </summary>
        public void OpenAlarm()
        {
            IsOpenAlarm = true;
            AlarmStatus = true;
        }

        /// <summary>
        /// 关闭报警
        /// </summary>
        public void CloseAlarm()
        {
            IsOpenAlarm = false;
            AlarmStatus = false;
        }

        private void SetAlarm(bool open)
        {
            short val = open ? (short)0xFF : (short)0x00;
            //lock (lockobj)
            {
                try
                {
                    plcHelper.ItemStart(AddrLightR1, val);
                    Thread.Sleep(50);
                    //plcHelper.GetRecive();

                    plcHelper.ItemStart(AddrLightR2, val);
                    Thread.Sleep(50);
                    //plcHelper.GetRecive();

                    plcHelper.ItemStart(AddrBuzzer, val);
                    Thread.Sleep(50);
                    //plcHelper.GetRecive();
                }
                catch (Exception ex)
                {
                    LogUtil.WriteLog("设置报警失败！", ex);
                }
            }
        }

        #endregion

        #region 开关日光灯
        public void OpenLight()
        {
            //lock (lockobj)
            {
                try
                {
                    plcHelper.ItemStart(AddrLight, 0xFF);
                    Thread.Sleep(50);
                    //plcHelper.GetRecive();
                }
                catch (Exception ex)
                {
                    LogUtil.WriteLog("打开日光灯失败！", ex);
                }
            }
        }

        public void CloseLight()
        {
            //lock (lockobj)
            {
                try
                {
                    plcHelper.ItemStart(AddrLight, 0x00);
                    Thread.Sleep(50);
                    //plcHelper.GetRecive();
                }
                catch (Exception ex)
                {
                    LogUtil.WriteLog("关闭日光灯失败！", ex);
                }
            }
        }
        #endregion

        public void CloseAll()
        {
            plcHelper.SetStart(PLCHelper.PlcAdd.Y0, 8, 0);
            Thread.Sleep(50);
            plcHelper.SetStart(PLCHelper.PlcAdd.Y20, 8, 0);
            Thread.Sleep(50);
            plcHelper.SetStart(PLCHelper.PlcAdd.Y30, 8, 0);
            Thread.Sleep(50);
            plcHelper.SetStart(PLCHelper.PlcAdd.Y40, 8, 0);
            Thread.Sleep(50);
        }

        public void GetStatus()
        {
            //lock (lockobj)
            {
                try
                {                    
                    plcHelper.GetStart(PLCHelper.PlcAdd.X0, 40);
                    Thread.Sleep(200);
                    DeltaData data = plcHelper.GetRecive0x02();
                    if (data == null)
                    {
                        Thread.Sleep(300);
                        data = plcHelper.GetRecive0x02();
                        return;
                    }

                    if (data.CMD != 0x02)
                    {
                        return;
                    }
                    Status status = new Status();
                    byte bytLock = data.DATA[1];//锁状态
                    char[] arrLock = Convert.ToString(bytLock, 2).PadLeft(8, '0').Reverse().ToArray();
                    status.Lock[0] = arrLock[1] == '0';
                    status.Lock[1] = arrLock[3] == '0';

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
                    StatusReceived?.Invoke(status);
                    if (flashno.Count > 0)
                    {
                        #region 方案一：同时设置16个扳手，减少重复通信 TODO:数据拼接错误
                        //string strval = "";
                        //for (int i = 0; i < 16; i++)
                        //{
                        //    strval += (flashno.Contains(i) && flashstatus) ? "0" : "1";
                        //}
                        //int val = Convert.ToInt32(strval, 2);
                        //plcHelper.SetStart(GetToolAddr(1, false), 16, val);
                        #endregion
                        string strNo = "";
                        #region 方案二：循环单独控制
                        foreach (int no in flashno)
                        {
                            strNo += no + ",";
                            plcHelper.ItemStart(GetToolAddr(no, false), (short)(flashstatus ? 0xFF : 0x00));
                            Thread.Sleep(50);
                        }
                        #endregion
                        //Thread.Sleep(300);
                        //plcHelper.GetRecive();
                        LogUtil.WriteLog("闪烁：" + strNo + flashstatus);
                        flashstatus = !flashstatus;
                    }

                    SetAlarm(IsOpenAlarm && AlarmStatus);//红灯+蜂鸣控制
                    AlarmStatus = !AlarmStatus;
                    //byte bytTools_3 = data.DATA[5];//备用扩展
                    //char[] arrTools_3 = Convert.ToString(bytTools_3, 2).PadLeft(8,'0').Reverse().ToArray();                   

                }
                catch (Exception ex)
                {
                    LogUtil.WriteLog("获取PLC接入点状态失败！", ex);
                }
            }
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
            if (input)
            {
                if (no == 1)
                {
                    return PLCHelper.PlcAdd.X1;
                }
                else
                {
                    return PLCHelper.PlcAdd.X3;
                }
            }
            else
            {
                int iStart = (int)PLCHelper.PlcAdd.Y0, iAddr = 0;
                iAddr = iStart + (no - 1);
                return (PLCHelper.PlcAdd)iAddr;
            }
        }

        public void OpenGreen()
        {
            SetLightBelt(this.AddrLightG1, 0xFF);
            SetLightBelt(this.AddrLightG2, 0xFF);
        }

        public void CloseGreen()
        {
            SetLightBelt(this.AddrLightG1, 0x00);
            SetLightBelt(this.AddrLightG2, 0x00);
        }

        private void SetLightBelt(PLCHelper.PlcAdd addr, short val)
        {
            //lock (lockobj)
            {
                try
                {
                    plcHelper.ItemStart(addr, val);
                    Thread.Sleep(50);
                    //plcHelper.GetRecive();
                }
                catch (Exception ex)
                {
                    LogUtil.WriteLog("设置灯带失败！", ex);
                }
            }
        }

        /// <summary>
        /// 红色报警灯
        /// </summary>
        private readonly PLCHelper.PlcAdd AddrLightR1 = PLCHelper.PlcAdd.Y40;
        /// <summary>
        /// 红色报警灯 地址
        /// </summary>
        private readonly PLCHelper.PlcAdd AddrLightR2 = PLCHelper.PlcAdd.Y43;
        /// <summary>
        /// 绿色报警灯 地址
        /// </summary>
        private readonly PLCHelper.PlcAdd AddrLightG1 = PLCHelper.PlcAdd.Y41;
        /// <summary>
        /// 绿色报警灯 地址
        /// </summary>
        private readonly PLCHelper.PlcAdd AddrLightG2 = PLCHelper.PlcAdd.Y44;

        /// <summary>
        /// 蓝色报警灯 地址
        /// </summary>
        private readonly PLCHelper.PlcAdd AddrLightB1 = PLCHelper.PlcAdd.Y42;
        /// <summary>
        /// 蓝色报警灯 地址
        /// </summary>
        private readonly PLCHelper.PlcAdd AddrLightB2 = PLCHelper.PlcAdd.Y45;
        /// <summary>
        /// 蜂鸣器地址
        /// </summary>
        private readonly PLCHelper.PlcAdd AddrBuzzer = PLCHelper.PlcAdd.Y3;

        /// <summary>
        /// 日光灯地址
        /// </summary>
        private readonly PLCHelper.PlcAdd AddrLight = PLCHelper.PlcAdd.Y2;

        ///// <summary>
        ///// 柜门全部关闭 回调
        ///// </summary>
        //public Action DoorClosed { get; set; }

        ///// <summary>
        ///// 工具状态改变 回调 &lt;位置1-16,状态&gt;
        ///// </summary>
        //public Action<int, bool> ToolStatusChanged;

        public Action<Status> StatusReceived;

        public void DisConnect()
        {
            plcHelper.DisConnect();
        }
    }
}
