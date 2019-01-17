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
            lock (lockobj)
            {
                try
                {
                    int val = Convert.ToInt32(strval, 2);
                    PLCHelper.PlcAdd startAddr = GetToolAddr(1, false);
                    plcHelper.SetStart(startAddr, 16, val);
                    Thread.Sleep(500);
                    plcHelper.GetRecive();
                }
                catch (Exception ex)
                {
                    LogUtil.WriteLog("设置工具指示灯失败！", ex);
                }
            }
        }

        /// <summary>
        /// 开指示灯1-16单独控制
        /// </summary>
        /// <param name="no">1-16</param>
        public void OpenToolLamp(int no)
        {
            lock (lockobj)
            {
                try
                {
                    PLCHelper.PlcAdd addr = GetToolAddr(no, false);
                    plcHelper.SetStart(addr, 1, 0x01);
                    Thread.Sleep(500);
                    plcHelper.GetRecive();
                }
                catch (Exception ex)
                {
                    LogUtil.WriteLog("打开工具指示灯失败！", ex);
                }
            }
        }

        /// <summary>
        /// 关指示灯1-16单独控制
        /// </summary>
        /// <param name="no"></param>
        public void CloseToolLamp(int no)
        {
            lock (lockobj)
            {
                try
                {
                    PLCHelper.PlcAdd addr = GetToolAddr(no, false);
                    plcHelper.SetStart(addr, 1, 0x00);
                    Thread.Sleep(500);
                    plcHelper.GetRecive();
                }
                catch (Exception ex)
                {
                    LogUtil.WriteLog("关闭工具指示灯失败！", ex);
                }
            }
        }

        private bool flashstatus = false;
        private int flashno = 0;
        public void FlashToolLamp(int no)
        {
            flashno = no;
        }

        #endregion

        #region 开门
        /// <summary>
        /// 开门1-2
        /// </summary>
        /// <param name="no">1-2</param>
        public void OpenDoor(int no)
        {
            lock (lockobj)
            {
                try
                {
                    PLCHelper.PlcAdd addr = GetLockAddr(no, false);
                    plcHelper.SetStart(addr, 1, 0x01);
                    Thread.Sleep(500);
                    plcHelper.GetRecive();

                    plcHelper.SetStart(addr, 1, 0x00);
                    Thread.Sleep(500);
                    plcHelper.GetRecive();
                }
                catch (Exception ex)
                {
                    LogUtil.WriteLog("开门失败！", ex);
                }
            }
        }
        #endregion

        #region 报警+蜂鸣
        /// <summary>
        /// 红灯报警+蜂鸣
        /// </summary>
        public void OpenAlarm()
        {
            lock (lockobj)
            {
                try
                {
                    plcHelper.ItemStart(AddrLightR1, 0xFF);
                    Thread.Sleep(500);
                    plcHelper.GetRecive();

                    plcHelper.ItemStart(AddrLightR2, 0xFF);
                    Thread.Sleep(500);
                    plcHelper.GetRecive();

                    plcHelper.ItemStart(AddrBuzzer, 0xFF);
                    Thread.Sleep(500);
                    plcHelper.GetRecive();
                }
                catch (Exception ex)
                {
                    LogUtil.WriteLog("打开报警失败！", ex);
                }
            }
        }

        /// <summary>
        /// 关闭报警
        /// </summary>
        public void CloseAlarm()
        {
            lock (lockobj)
            {
                try
                {
                    plcHelper.ItemStart(AddrLightR1, 0x00);
                    Thread.Sleep(500);
                    plcHelper.GetRecive();

                    plcHelper.ItemStart(AddrLightR2, 0x00);
                    Thread.Sleep(500);

                    plcHelper.GetRecive();
                    plcHelper.ItemStart(AddrBuzzer, 0x00);
                    Thread.Sleep(500);
                    plcHelper.GetRecive();
                }
                catch (Exception ex)
                {
                    LogUtil.WriteLog("关闭报警失败！", ex);
                }
            }
        }
        #endregion

        #region 开关日光灯
        public void OpenLight()
        {
            lock (lockobj)
            {
                try
                {
                    plcHelper.ItemStart(AddrLight, 0xFF);
                    Thread.Sleep(500);
                    plcHelper.GetRecive();
                }
                catch (Exception ex)
                {
                    LogUtil.WriteLog("打开日光灯失败！", ex);
                }
            }
        }

        public void CloseLight()
        {
            lock (lockobj)
            {
                try
                {
                    plcHelper.ItemStart(AddrLight, 0x00);
                    Thread.Sleep(500);
                    plcHelper.GetRecive();
                }
                catch (Exception ex)
                {
                    LogUtil.WriteLog("关闭日光灯失败！", ex);
                }
            }
        }
        #endregion

        public Status GetStatus(bool[] oriToolStatus)
        {
            lock (lockobj)
            {
                try
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
                        Thread.Sleep(500);
                        plcHelper.GetRecive();
                        flashstatus = !flashstatus;
                    }
                    //byte bytTools_3 = data.DATA[5];//备用扩展
                    //char[] arrTools_3 = Convert.ToString(bytTools_3, 2).PadLeft(8,'0').Reverse().ToArray();
                    if (!status.Lock[0] && !status.Lock[1])
                    {
                        DoorClosed?.Invoke();//关门改变回调
                    }
                    return status;
                }
                catch (Exception ex)
                {
                    LogUtil.WriteLog("获取PLC接入点状态失败！", ex);
                }
                return null;
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

        /// <summary>
        /// 柜门全部关闭 回调
        /// </summary>
        public Action DoorClosed { get; set; }

        /// <summary>
        /// 工具状态改变 回调 &lt;位置1-16,状态&gt;
        /// </summary>
        public Action<int, bool> ToolStatusChanged;

        public void DisConnect()
        {
            plcHelper.DisConnect();
        }
    }
}
