using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using ToolMgt.Common;

namespace ToolMgt.BLL.ICCard
{
    public class UsbICCard : ICardHelper
    {
        #region
        [DllImport("dcrf32.dll")]
        public static extern int dc_init(Int16 port, Int32 baud);  //初试化
        [DllImport("dcrf32.dll")]
        public static extern short dc_exit(int icdev);
        [DllImport("dcrf32.dll")]
        public static extern short dc_beep(int icdev, uint _Msec);
        [DllImport("dcrf32.dll")]
        public static extern short dc_card_double_hex(int icdev, char _Mode, [MarshalAs(UnmanagedType.LPStr)] StringBuilder Snr);  //从卡中读数据(转换为16进制)

        [DllImport("dcrf32.dll")]
        public static extern short dc_read(int icdev, int adr, [Out] byte[] sdata);  //从卡中读数据
        [DllImport("dcrf32.dll")]
        public static extern short dc_read(int icdev, int adr, [MarshalAs(UnmanagedType.LPStr)] StringBuilder sdata);  //从卡中读数据

        [DllImport("dcrf32.dll")]
        public static extern int dc_reset(int icdev, uint sec);
        [DllImport("dcrf32.dll")]
        public static extern short dc_config_card(int icdev, char cardtype);

        [DllImport("dcrf32.dll")]
        public static extern short dc_card(int icdev, char _Mode, ref ulong Snr);

        [DllImport("dcrf32.dll")]
        public static extern short dc_card_n(int icdev, byte _Mode, ref uint SnrLen, [Out]byte[] _Snr);
        [DllImport("dcrf32.dll")]
        public static extern short hex_a([In] byte[] hex, [Out] byte[] a, short length);  //普通字符转换成十六进制字符

        [DllImport("dcrf32.dll")]
        public static extern short dc_read_hex(int icdev, int adr, ref byte sdata);  //从卡中读数据(转换为16进制)
        [DllImport("dcrf32.dll")]
        public static extern short dc_read_hex(int icdev, int adr, [MarshalAs(UnmanagedType.LPStr)] StringBuilder sdata);  //从卡中读数

        [DllImport("dcrf32.dll")]
        public static extern short dc_write(int icdev, int adr, [In] string sdata);  //向卡中写入数据
        [DllImport("dcrf32.dll")]
        public static extern short dc_write_hex(int icdev, int adr, [In] string sdata);  //向卡中写入数据(转换为16进制)
        #endregion

        private int Icdev { set; get; } = -1;

        public string Port { set; get; }

        /// <summary>
        /// 是否连接成功
        /// </summary>
        public bool IsConnected { set; get; } = false;


        private bool _keepreading = false;

        /// <summary>
        /// 处理数据事件
        /// </summary>
        public event EventHandler<CardEventArgs> HandDataBack;

        public UsbICCard(int _port, int _baudRate)
        {
            try
            {
                Icdev = dc_init((short)_port, _baudRate);
                if (Icdev > 0)
                {
                    IsConnected = true;
                    _keepreading = true;
                }
                else
                {
                    LogUtil.WriteLog("读卡器连接失败！");
                }
            }
            catch (Exception ex)
            {
                LogUtil.WriteLog("读卡器端口" + _port + "打开失败！" + ex.Message);
            }
        }
       
        public void Read()
        {
            try
            {
                while (_keepreading)
                {
                    uint SnrLen = 0;
                    string icCardNo = "";
                    byte[] _Snr = new byte[4];
                    byte[] szSnr = new byte[4];
                    int st = dc_reset(Icdev, 0);
                    dc_config_card(Icdev, 'A');

                    st = dc_card_n(Icdev, 0x00, ref SnrLen, _Snr);

                    //  dc_card(icdev, '0', ref icCardNo);
                    if (st == 0)
                    {
                        dc_beep(Icdev, 10);
                        for (int i = 0; i < 4; i++)
                        {
                            szSnr[i] = _Snr[3 - i];
                        }

                        icCardNo = BitConverter.ToUInt32(szSnr, 0).ToString();
                    }
                    if (!string.IsNullOrEmpty(icCardNo))
                    {
                        HandDataBack?.Invoke(this, new CardEventArgs(icCardNo.ToString()));
                    }
                    Thread.Sleep(2000);
                }
            }
            catch { }
        }

        public bool IsOpen()
        {
            return IsConnected;
        }

        public void Close()
        {
            if (Icdev > 0)
            {
                dc_exit(Icdev);
                Icdev = -1;
            }
        }

        ~UsbICCard()
        {
            try
            {
                Close();
            }
            catch (Exception)
            {
            }
        }
    }
}
