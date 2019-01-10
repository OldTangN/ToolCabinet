using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading;

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

            byte[] bb = BitConverter.GetBytes(aa);

            short cc = BitConverter.ToInt16(bb, 0);

            string dd = Convert.ToString(cc, 2);
        }
        [TestMethod()]
        public void SendTest()
        {
            PLCHelper pLCHelper = new PLCHelper();
            //pLCHelper.GetStart(PLCHelper.PlcAdd.T0 ,1);
            //:010306000001F5\r\n  发送
            //:0103020000FA\r\n 接收
            pLCHelper.GetStart(PLCHelper.PlcAdd.X1, 1);
            pLCHelper.GetStart(PLCHelper.PlcAdd.Y1, 1);

            Thread.Sleep(1000);

            DeltaData deltaData = pLCHelper.GetRecive();


            //:0183027A\r\n 接收 02无效的装置地址 07校验位错误
        }
        [TestMethod()]
        public void SetTest()
        {
            PLCHelper pLCHelper = new PLCHelper();
            pLCHelper.SetStart(PLCHelper.PlcAdd.Y0, 8, 0xFF);
            pLCHelper.SetStart(PLCHelper.PlcAdd.Y20, 8, 0xFF);
            pLCHelper.SetStart(PLCHelper.PlcAdd.Y30, 8, 0xFF);
            pLCHelper.SetStart(PLCHelper.PlcAdd.Y40, 8, 0xFF);
        }
    }
}
