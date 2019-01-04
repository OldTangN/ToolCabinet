using System;

namespace ToolMgt.BLL
{
    public class DeltaPLC : IPLC
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

        public void Open()
        {
            portService.CreateAndOpenPort();
        }

        public void SendData(byte[] data)
        {
            portService.SendData(data);
        }
    }
}
