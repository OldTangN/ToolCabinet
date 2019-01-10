﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Text;

namespace ToolMgt.BLL.Tests
{
    [TestClass()]
    public class DeltaDataTests
    {
        [TestMethod()]
        public void GetLRCTest()
        {
            DeltaData aa = new DeltaData(0x03, new byte[] { 0x04, 0x01, 0x00, 0x01 });

            byte[] senddata = aa.ToSendData();
        }
        [TestMethod()]
        public void Getbyte()
        {
            short aa = 0x1cd;

            byte[] bb =   BitConverter.GetBytes(aa);

            short cc = BitConverter.ToInt16(bb,0);

            string dd = Convert.ToString(cc, 2);
        }
        [TestMethod()]
        public void SendTest()
        {
            PLCHelper pLCHelper = new PLCHelper();
            pLCHelper.GetStart(PLCHelper.PlcAdd.T0 ,1);
            //:010306000001F5\r\n  发送
            //:0103020000FA\r\n 接收
            pLCHelper.GetStart(PLCHelper.PlcAdd.Y0, 1);
            //:0183027A\r\n 接收 02无效的装置地址 07校验位错误
        }
        [TestMethod()]
        public void SetTest()
        {
            PLCHelper pLCHelper = new PLCHelper();
            pLCHelper.SetStart(PLCHelper.PlcAdd.Y20, 10, 0xcd1001);
        }
    }
}
