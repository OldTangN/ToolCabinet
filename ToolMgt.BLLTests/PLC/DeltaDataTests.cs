using System;
using System.Text;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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
            pLCHelper.GetStart(PLCHelper.PlcAdd.X0, 30);//X10~X17没有，但是有数据返回
            Thread.Sleep(500);
            try
            {
                DeltaData deltaData = pLCHelper.GetRecive();

                //pLCHelper.GetStart(PLCHelper.PlcAdd.Y0, 40);//Y10~Y17没有，但是有数据返回
            }
            catch (Exception ex)
            {

                throw;
            }
            //:0183027A\r\n 接收 02无效的装置地址 07校验位错误
        }
        [TestMethod()]
        public void SetTest()
        {
            PLCHelper pLCHelper = new PLCHelper();
            pLCHelper.SetStart(PLCHelper.PlcAdd.Y0, 8, 0xFF);//10~17没有
            pLCHelper.SetStart(PLCHelper.PlcAdd.Y20, 8, 0xFF);
            pLCHelper.SetStart(PLCHelper.PlcAdd.Y30, 8, 0xFF);
            pLCHelper.SetStart(PLCHelper.PlcAdd.Y40, 8, 0xFF);
        }
        [TestMethod()]
        public void LightControlTest()
        {
            PLCControl ctl = new PLCControl("");
            ctl.OpenAlarm();

            ctl.CloseAlarm();

            ctl.OpenLight(2);
            ctl.CloseLight(2);

            ctl.GetStatus();

            ctl.OperateLight(new byte[] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 });

            for (int i = 1; i <= 16; i++)
            {
                ctl.CloseLight(i);
                Thread.Sleep(2000);
            }
        }
    }
}
