using Microsoft.VisualStudio.TestTools.UnitTesting;
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
    }
}