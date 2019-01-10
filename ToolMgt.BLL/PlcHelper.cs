using System;
using System.Collections.Generic;
using System.IO.Ports;
namespace ToolMgt.BLL
{

    /// <summary>
    /// 传感器监控
    /// </summary>
    public class PLCHelper
    {
        private DeltaPLC deltaPLC { set; get; }
        public enum PlcAdd : ushort
        {
            Y0 = 0x0500, Y1 = 0x0501, Y2 = 0x0502, Y3 = 0x0503, Y4 = 0x0504, Y5 = 0x0505, Y6 = 0x0506, Y7 = 0x0507,
            Y20 = 0x0510, Y21 = 0x0511, Y22 = 0x0512, Y23 = 0x0513, Y24 = 0x0514, Y25 = 0x0515, Y26 = 0x0516, Y27 = 0x0517,
            Y30 = 0x0518, Y31 = 0x0519, Y32 = 0x051A, Y33 = 0x051B, Y34 = 0x051C, Y35 = 0x051D, Y36 = 0x051E, Y37 = 0x051F,
            Y40 = 0x0520, Y41 = 0x0521, Y42 = 0x0522, Y43 = 0x0523, Y44 = 0x0524, Y45 = 0x0525, Y46 = 0x0526, Y47 = 0x0527,
            X0 = 0x0400, X1 = 0x0401, X2 = 0x0402, X3 = 0x0403, X4 = 0x0404, X5 = 0x0405, X6 = 0x0406, X7 = 0x0407,
            X20 = 0x0410, X21 = 0x0411, X22 = 0x0412, X23 = 0x0413, X24 = 0x0414, X25 = 0x0415, X26 = 0x0416, X27 = 0x0417,
            X30 = 0x0418, X31 = 0x0419, X32 = 0x041A, X33 = 0x041B, X34 = 0x041C, X35 = 0x041D, X36 = 0x041E, X37 = 0x041F,
            X40 = 0x0420, X41 = 0x0421, X42 = 0x0422, X43 = 0x0423, X44 = 0x0424, X45 = 0x0425, X46 = 0x0426, X47 = 0x0427,
            T20 = 0x0614,
        }

        public PLCHelper()
        {
            ComParmater comParmater = new ComParmater("COM12", 9600, Parity.Even, 7, StopBits.One);
            deltaPLC = new DeltaPLC(comParmater);
            deltaPLC.Open();
            deltaPLC.ReciveHandler += DeltaPLC_ReciveHandler;

        }

        private void DeltaPLC_ReciveHandler(object sender, DataEventArgs e)
        {
            byte[] reciveData = e.data;
            string str = System.Text.Encoding.ASCII.GetString(reciveData);

        }
        /// <summary>
        /// 获取各个工具位置的状态 参考协议4.5.3
        /// </summary>
        /// <param name="plcAdd">起始地址</param>
        /// <param name="count">读取数量</param>
        /// <returns></returns>
        public void GetStart(PlcAdd plcAdd, short count)
        {
            List<byte> cmd = new List<byte>();
            byte[] padd = (BitConverter.GetBytes((short)plcAdd));
            Array.Reverse(padd);//高低位反转，padd为低位在前高位在后，需转换为高位在前低位在后
            cmd.AddRange(padd);
            byte[] ct = BitConverter.GetBytes(count);
            Array.Reverse(ct);//高低位反转
            cmd.AddRange(ct);

            DeltaData deltaData = new DeltaData(0x03, cmd.ToArray());
            byte[] data = deltaData.ToSendData();

            deltaPLC.SendData(data);
        }

        /// <summary>
        /// 设置批量状态 参考协议4.5.6
        /// </summary>
        /// <param name="plcAdd"></param>
        /// <param name="count"></param>
        /// <param name="value"></param>
        public void SetStart(PlcAdd plcAdd, short count, int value)
        {
            List<byte> cmd = new List<byte>();
            byte[] padd = (BitConverter.GetBytes((short)plcAdd));
            Array.Reverse(padd);//高低位反转
            cmd.AddRange(padd);
            byte[] ct = BitConverter.GetBytes(count);
            Array.Reverse(ct);//高低位反转
            cmd.AddRange(ct);
            int valueInt = (int)Math.Ceiling(count / 8.0);
            byte valueByte = (byte)valueInt;
            cmd.Add(valueByte);//字节数目

            byte[] vbyte = (BitConverter.GetBytes(value));

            List<byte> lbyte = new List<byte>();
            for (int i = 0; i < valueInt; i++)
            {
                lbyte.Add(vbyte[i]);
            }
            byte[] newbyte = lbyte.ToArray();
            Array.Reverse(newbyte);//高低位反转
            cmd.AddRange(newbyte);

            DeltaData deltaData = new DeltaData(0x0F, cmd.ToArray());
            byte[] data = deltaData.ToSendData();
            deltaPLC.SendData(data);
        }

    }
}
