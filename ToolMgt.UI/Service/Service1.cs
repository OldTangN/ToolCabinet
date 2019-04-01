using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using ToolMgt.BLL;
using ToolMgt.Common;
using ToolMgt.UI.Common;
using ToolMgt.UI.Models;

namespace ToolMgt.UI
{
    // 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码和配置文件中的类名“Service1”。
    public class Service1 : IService1
    {
        public List<ToolRecordInfo> GetToolRecords(string barcode, DateTime startTime, DateTime endTime)
        {
            ToolRecordDao dao = new ToolRecordDao();
            var rlt = dao.GetRecords("全部", barcode, "", startTime, endTime);
            List<ToolRecordInfo> infos = new List<ToolRecordInfo>();
            if (rlt != null && rlt.Count > 0)
            {
                foreach (var rcd in rlt)
                {
                    ToolRecordInfo info = new ToolRecordInfo()
                    {
                        BorrowDate = rcd.BorrowDate,
                        CreateDateTime = rcd.CreateDateTime,
                        Id = rcd.Id,
                        IsReturn = rcd.IsReturn,
                        ReturnDate = rcd.ReturnDate,
                        ToolBarCode = rcd.Tool.ToolBarCode,
                        ToolId = rcd.ToolId,
                        //ToolName = rcd.Tool.ToolName,
                        //ToolRFIDCode = rcd.Tool.ToolRFIDCode,
                        ToolType = rcd.Tool.ToolType.TypeName,
                        UserId = rcd.UserId,
                        UserName = rcd.User.RealName
                    };
                    infos.Add(info);
                }
            }
            return infos;
        }

        public List<ToolInfo> GetTools()
        {
            ToolDao dao = new ToolDao();
            List<ToolInfo> infos = new List<ToolInfo>();
            var rlt = dao.GetTools();
            if (rlt != null && rlt.Count > 0)
            {
                foreach (var t in rlt)
                {
                    var rcd = t.ToolRecords.FirstOrDefault(p => p.ToolId == t.id && p.IsReturn == false);
                    ToolInfo info = new ToolInfo()
                    {
                        id = t.id,
                        BorrowerName = rcd == null ? "" : rcd.User.RealName,
                        Factory = t.Factory,
                        Note = t.Note,
                        Position = t.Position,
                        RangeMax = t.RangeMax,
                        RangeMin = t.RangeMin,
                        Status = t.Status ? "空闲" : "已领用",
                        ToolBarCode = t.ToolBarCode,
                        ToolType = t.ToolType.TypeName
                    };
                    infos.Add(info);
                }
            }
            return infos;
        }

        public OpenDoorResut OpenDoor()
        {
            OpenDoorResut rlt = new OpenDoorResut();
            PLCControl plcCtl = App.PLC;

            if (!plcCtl.Connected)
            {
                rlt.Msg = "PLC通信失败！";
                return rlt;
            }
            try
            {
                plcCtl.OpenDoor(1, SysConfiguration.DoorWaitTime);
                plcCtl.OpenDoor(2, SysConfiguration.DoorWaitTime);
                rlt.Success = true;
                rlt.Msg = "成功";
            }
            catch (Exception ex)
            {
                LogUtil.WriteLog("打开柜门异常！", ex);
                rlt.Msg = "打开柜门失败！";
            }
            return rlt;
        }
    }
}
