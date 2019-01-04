using System;

namespace ToolMgt.BLL
{
    public interface IPLC
    {
        event EventHandler<DataEventArgs> ReciveHandler;

        void Open();

        void Close();

        void SendData(byte[] data);



    }
}
