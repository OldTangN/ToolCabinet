using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToolMgt.Common;
using ToolMgt.Model;

namespace ToolMgt.BLL
{
    public class ToolRecordDao
    {
        private ToolCabinetEntities Db;
        public ToolRecordDao()
        {
            Db = new ToolCabinetEntities("ToolCabinetEntities");
        }

        public List<ToolRecord> GetRecords(string selectStatus, string barcode, string nameOrCard, DateTime borrowStart, DateTime borrowEnd)
        {
            var dd = Db.ToolRecords.Include("Tool").Include("User").Where(p => false); ;

            try
            {
                if (selectStatus == "全部")
                {
                    return Db.ToolRecords.Include("Tool").Include("User").Where(
                           p => p.Tool.ToolBarCode.Contains(barcode)
                           && p.Tool.ToolRFIDCode.Contains(barcode)
                           && DateTime.Compare(p.BorrowDate, borrowEnd) <= 0
                           && DateTime.Compare(p.BorrowDate, borrowStart) >= 0
                       )?.ToList();
                }
                else if (selectStatus == "已领用")
                {
                    return Db.ToolRecords.Include("Tool").Include("User").Where(
                           p => p.Tool.ToolBarCode.Contains(barcode)
                           && p.Tool.ToolRFIDCode.Contains(barcode)
                           && DateTime.Compare(p.BorrowDate, borrowEnd) <= 0
                           && DateTime.Compare(p.BorrowDate, borrowStart) >= 0
                           && !p.Tool.Status
                       )?.ToList();
                }
                else
                {
                    return Db.ToolRecords.Include("Tool").Include("User").Where(
                           p => p.Tool.ToolBarCode.Contains(barcode)
                           && p.Tool.ToolRFIDCode.Contains(barcode)
                           && DateTime.Compare(p.BorrowDate, borrowEnd) <= 0
                           && DateTime.Compare(p.BorrowDate, borrowStart) >= 0
                           && p.Tool.Status
                       )?.ToList();
                }
            }
            catch (Exception ex)
            {
                LogUtil.WriteLog("查询工具领用记录失败！", ex);
                return null;
            }
        }

        /// <summary>
        /// 添加领用记录并修改工具状态
        /// </summary>
        /// <param name="toolid"></param>
        /// <param name="uid"></param>
        /// <returns></returns>
        public bool AddRecord(int toolid, int uid)
        {
            try
            {
                ToolRecord record = new ToolRecord()
                {
                    ToolId = toolid,
                    UserId = uid,
                    BorrowDate = DateTime.Now,
                    CreateDateTime = DateTime.Now,
                    IsReturn = false,
                    ReturnDate = null,
                };
                Db.ToolRecords.Add(record);
                Db.Tools.First(p => p.id == toolid).Status = false;
                Db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                LogUtil.WriteLog("添加领用记录失败！", ex);
                return false;
            }
        }

        /// <summary>
        /// 更新领用记录并修改工具状态
        /// </summary>
        /// <param name="toolid"></param>
        /// <returns></returns>
        public bool UpdateRecord(int toolid)
        {
            try
            {
                ToolRecord record = Db.ToolRecords.First(p => p.ToolId == toolid && !p.IsReturn);
                record.IsReturn = true;
                record.ReturnDate = DateTime.Now;
                Db.Tools.First(p => p.id == toolid).Status = true;
                Db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                LogUtil.WriteLog("修改领用记录失败！", ex);
                return false;
            }
        }
    }
}
