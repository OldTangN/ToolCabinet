using System;
using System.IO.Ports;
using System.Text;
using ToolMgt.Common;

namespace ToolMgt.BLL
{
    public class SerialPortService
    {
        private SerialPort port { set; get; }
        public event EventHandler<DataEventArgs> ReciveHandler;

        private ComParmater comParmater { set; get; }
        public SerialPortService(ComParmater _comParmater)
        {
            this.comParmater = _comParmater;
        }
        /// <summary>
        /// 创建并打开串口，或返回已创建的串口
        /// </summary>
        /// <param name="portName">串口名称</param>
        /// <param name="baudRate"></param>
        /// <param name="parity"></param>
        /// <param name="dataBits"></param>
        /// <param name="stopBits"></param>
        /// <returns></returns>
        public SerialPort CreateAndOpenPort()
        {
            if (string.IsNullOrEmpty(comParmater.PortName))
            {
                return null;
            }
            try
            {
                port = new SerialPort(comParmater.PortName, comParmater.BaudRate, comParmater.Parity, comParmater.DataBits, comParmater.StopBits);
                port.RtsEnable = false;
                port.DataReceived += Port_DataReceived;
                port.Open();
            }
            catch (Exception e)
            {
                LogUtil.WriteLog("创建串口失败" + port.PortName + "失败！" + e);
                return null;
            }
            return port;
        }

        private void Port_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                SerialPort sp = (SerialPort)sender;
                int len = sp.BytesToRead;
                if (len > 0)
                {
                    byte[] receiveMsg = new byte[len];
                    sp.Read(receiveMsg, 0, len);
                    ReciveHandler?.Invoke(sender, new DataEventArgs(receiveMsg));
                }
            }
            catch (Exception ex)
            {
                LogUtil.WriteLog(ex.Message, ex);
            }
        }

        public void ClosePort()
        {
            try
            {
                port.Close();
            }
            catch (Exception ex)
            {
                LogUtil.WriteLog("关闭串口失败!{0}", ex);
            }
        }
        /// <summary>
        /// 向串口发送数据
        /// </summary> 
        /// <returns></returns>
        public void SendData(string data, int ascii = 0)
        {
            try
            {
                Encoding encoding;
                if (ascii == 0)
                {
                    encoding = Encoding.ASCII;
                }
                else
                {
                    encoding = Encoding.UTF8;
                }
                byte[] bdata = encoding.GetBytes(data);

                SendData(bdata);
            }
            catch (Exception ex)
            {
                LogUtil.WriteLog(ex.Message, ex);
            }           
        }

        public void SendData(byte[] data)
        {
            try
            {
                port.Write(data, 0, data.Length);
            }
            catch (Exception ex)
            {
                LogUtil.WriteLog(ex.Message, ex);
            }
        }

        ///// <summary>
        ///// 发送命令，并获取接收信息，
        ///// </summary>
        ///// <param name="serialPort"></param>
        ///// <param name="SendData">发送数据</param>
        ///// <param name="ReceiveData">接收信息</param> 
        ///// <param name="receiveCount">接收信息尝试次数：每50毫秒接收一次信息</param> 
        ///// <returns>接收数据的长度，串口未打开返回-1</returns>
        //public static List<byte> SendByte(SerialPort serialPort, List<byte> SendData)
        //{
        //    List<byte> ReceiveData = new List<byte>();
        //    if (!serialPort.IsOpen)
        //    {
        //        return ReceiveData;
        //    }
        //    try
        //    {
        //        serialPort.DiscardInBuffer();//清空接收缓冲区
        //        serialPort.DiscardOutBuffer();
        //        serialPort.Write(SendData.ToArray(), 0, SendData.Count);
        //        if (serialPort.BytesToRead > 0)
        //        {
        //            byte[] buffer = new byte[serialPort.BytesToRead];
        //            serialPort.Read(buffer, 0, buffer.Length);
        //            ReceiveData.AddRange(buffer);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        LogUtil.WriteLog(string.Format("发送数据失败!{0}", ex.ToString()));
        //    }
        //    return ReceiveData;
        //}
    }


}
