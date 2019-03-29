using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using ToolCabinet.Web.Models;
using ToolMgt.BLL;
using ToolMgt.Common;
using ToolMgt.Model;

namespace ToolCabinet.Web
{
    /// <summary>
    /// WebService1 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消注释以下行。 
    // [System.Web.Script.Services.ScriptService]
    public class WebService1 : System.Web.Services.WebService
    {

        [WebMethod]
        public string HelloWorld()
        {
            return "Hello World";
        }

        [WebMethod]
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

        [WebMethod]
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
    }
}
