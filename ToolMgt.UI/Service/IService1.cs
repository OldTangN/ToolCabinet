using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using ToolMgt.UI.Models;

namespace ToolMgt.UI
{
    // 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码和配置文件中的接口名“IService1”。
    [ServiceContract]
    public interface IService1
    {
        [OperationContract]
        List<ToolRecordInfo> GetToolRecords(string barcode, DateTime startTime, DateTime endTime);

        [OperationContract]
        List<ToolInfo> GetTools();

        [OperationContract]
        OpenDoorResut OpenDoor();
    }
}
