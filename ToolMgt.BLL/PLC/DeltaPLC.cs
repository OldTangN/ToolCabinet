using System;
using System.Collections.Generic;
using System.Text;

namespace ToolMgt.BLL
{
    public class DeltaPLC
    {
        private SerialPortService portService { set; get; }

        public event EventHandler<DataEventArgs> ReciveHandler;


        public DeltaPLC(ComParmater comParmater)
        {
            portService = new SerialPortService(comParmater);
            portService.ReciveHandler += PortService_ReciveHandler;

        }

        private void PortService_ReciveHandler(object sender, DataEventArgs e)
        {
            ReciveHandler?.Invoke(sender, e);
        }

        public void Close()
        {
            portService.ClosePort();
        }

        public bool Open()
        {
            return portService.CreateAndOpenPort() != null;
        }

        public void SendData(byte[] data)
        {
            portService.SendData(data);
        }


    }


    public class DeltaData
    {
        /// <summary>
        /// 起始位 固定值
        /// </summary>
        private byte STX { set; get; } = 0x3A;

        /// <summary>
        /// 地址 固定值
        /// </summary>
        public byte ADR { set; get; } = 0x01;
        /// <summary>
        /// 命令
        /// 01, 读节点状态(不可读输入点状态) 读取 T0-TFF 0600~06FF 
        /// 02, 读节点状态(可读输入节点状态) 读取 Y0-YFF 0500~05FF 
        /// 03, 读出寄存器内容值            可读寄存器：T, C, D 
        /// 05, 强制单独节点状              强制 Y0 节点为 On:FF00,Off:0000
        /// 06, 预设单独寄存器的值          设置寄存器T0 的值为 12 34（16 进制），T0的通讯地址为0600
        /// 15, 强制多个节点                设置节点 Y007…Y000 = 1100 1101, Y011…Y010 = 01.
        /// 16, 预设多个寄存器的值          设置 T0 的值为 000A
        /// </summary>
        public byte CMD { set; get; }

        /// <summary>
        /// 数据帧
        /// </summary>
        public byte[] DATA { set; get; }
        /// <summary>
        /// 校验位
        /// </summary>
        public byte CHK { set; get; }

        /// <summary>
        /// 终止位 固定值
        /// </summary>
        public byte[] END { set; get; } = { 0x0D, 0x0A };

        public DeltaData()
        {
        }
        public DeltaData(byte _CMD, byte[] _Data)
        {
            CMD = _CMD;
            DATA = _Data;
        }

        public byte[] ToSendData()
        {
            //发送ASCII码数据
            List<byte> SendData = new List<byte>();
            //校验数据
            List<byte> chkData = new List<byte>();

            string adr = Convert.ToString(ADR, 16).PadLeft(2, '0').ToUpper();
            SendData.AddRange(Encoding.ASCII.GetBytes(adr));


            chkData.Add(ADR);
            string cmd = Convert.ToString(CMD, 16).PadLeft(2, '0').ToUpper();
            SendData.AddRange(Encoding.ASCII.GetBytes(cmd));
            chkData.Add(CMD);


            string sData = "";
            for (int i = 0; i < DATA.Length; i++)
            {
                sData += Convert.ToString(DATA[i], 16).PadLeft(2, '0').ToUpper();
            }
            SendData.AddRange(Encoding.ASCII.GetBytes(sData));


            chkData.AddRange(DATA);
            CHK = GetLRC(chkData);
            string chk = Convert.ToString(CHK, 16).PadLeft(2, '0').ToUpper();
            SendData.AddRange(Encoding.ASCII.GetBytes(chk));

            SendData.Insert(0, STX);
            SendData.AddRange(END);
            return SendData.ToArray();
        }

        private byte GetLRC(List<byte> data)
        {
            byte lrc = 0x00;
            foreach (var item in data)
            {
                lrc += item;
            }
            lrc = (byte)(~lrc + 0x01);
            return lrc;
        }
    }
}
