﻿using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ToolMgt.Common;

namespace ToolMgt.BLL.ICCard
{
    public class ComICCard : ICardHelper
    {
        private SerialPort port = new SerialPort();
        private const long priCDelay = 3000000;
        public bool isread { set; get; } = false;
        private bool _keepreading = false;
        private string readstring = "";
        private int baudrate { set; get; } = 115200;

        public event EventHandler<CardEventArgs> HandDataBack;

        public ComICCard(int _port, int _baudrate)
        {
            try
            {
                port = new SerialPort();
                port.PortName = "COM" + _port;
                port.BaudRate = _baudrate;
                port.ReceivedBytesThreshold = 1000;
                port.Open();
                _keepreading = true;
            }
            catch (Exception e)
            {
                LogUtil.WriteLog("读卡器端口" + port.PortName + "打开失败！" + e);
            }
        }
        public bool Openport()
        {
            try
            {
                if (!port.IsOpen)
                    port.Open();
                return true;
            }
            catch { return false; }
        }

        public void Read()
        {
            try
            {
                while (_keepreading)
                {
                    string strData;
                    byte[] r_byte = new byte[20];
                    if (port.IsOpen)
                    {
                        try
                        {
                            port.Read(r_byte, 0, 8);

                            if (r_byte[0] != 0xBB)
                            {
                                strData = "";
                                for (int i = 2; i < 6; i++)
                                {
                                    strData = strData + funBtoHex(r_byte[i]);
                                }
                                HandDataBack?.Invoke(this, new CardEventArgs(strData));
                                isread = false;
                            }
                        }
                        catch (Exception ex) { }
                    }
                    else
                    {
                        Thread.Sleep(50);
                    }
                }
            }
            catch { }
        }

        #region 
        private string funBtoHex(byte num)
        {
            string strhex;
            strhex = num.ToString("X");
            if (strhex.Length == 1)
                strhex = " 0" + strhex;
            else
                strhex = " " + strhex;
            return strhex;

        }
        #endregion


        public string BackString()
        {
            return readstring;
        }
        public void setreturnstr(string str)
        {
            isread = false;
            readstring = str;
        }
        public string returnreadstr()
        {
            if (isread)
            {
                return "";
            }
            return readstring;
        }

        public bool IsOpen()
        {
            return port.IsOpen;
        }

        public void Close()
        {
            try
            {
                port.Close();
            }
            catch (Exception )
            {
            }
        }
    }
}
