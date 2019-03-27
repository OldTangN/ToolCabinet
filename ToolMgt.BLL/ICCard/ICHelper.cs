﻿using System;
using System.Runtime.InteropServices;
using System.Text;

namespace ToolMgt.BLL
{
    public class ICHelper
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

        public static int icdev { set; get; }
        public static bool OpenPort()
        {
            icdev = dc_init(100, 115200);


            return icdev > 0;
        }

        public static void ClosePort()
        {
            if (icdev > 0)
            {
                dc_exit(icdev);
                icdev = -1;
            }
        }

        public static string ReadCard()
        {
            //ulong icCardNo = 0;
            //int st = dc_reset(icdev, 0);
            //dc_config_card(icdev, 'A');
            //dc_card(icdev, '0', ref icCardNo);
            //if (icCardNo != 0)
            //{
            //    dc_beep(icdev, 10);
            //}
            //return icCardNo.ToString();

            uint SnrLen = 0;
            string icCardNo = "0";
            byte[] _Snr = new byte[4];
            byte[] szSnr = new byte[4];
            int st = dc_reset(icdev, 0);
            dc_config_card(icdev, 'A');

            st = dc_card_n(icdev, 0x00, ref SnrLen, _Snr);

            //  dc_card(icdev, '0', ref icCardNo);
            if (st == 0)
            {
                dc_beep(icdev, 10);
                for (int i = 0; i < 4; i++)
                {
                    szSnr[i] = _Snr[3 - i];
                }

                 icCardNo = BitConverter.ToUInt32(szSnr, 0).ToString();
                //icCardNo = "";
                //for (int i = 0; i < 4; i++)
                //{
                //    icCardNo += Convert.ToString(_Snr[i], 16);
                //}

                //for (int i = 0; i < 4; i++)
                //{
                //    _Snr[i] = _Snr[3 - i];
                //}
                //hex_a(_Snr, szSnr, 4);
                //icCardNo = Encoding.Default.GetString(szSnr);

            }
            return icCardNo.ToUpper();
        }

    }
}