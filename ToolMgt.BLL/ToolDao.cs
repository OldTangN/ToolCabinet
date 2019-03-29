using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToolMgt.Common;
using ToolMgt.Model;

namespace ToolMgt.BLL
{
    public class ToolDao
    {
        private ToolCabinetEntities Db;
        public ToolDao()
        {
            Db = new ToolCabinetEntities("ToolCabinetEntities");
        }


        public List<Tool> GetTools(int userId)
        {
            List<Tool> tools = Db.Tools.OrderBy(p => p.Position).ToList();
            ToolRecord currRecord = Db.ToolRecords.FirstOrDefault(p => !p.IsReturn && p.UserId == userId);
            int currUserTool = -1;//当前用户未借出工具
            if (tools != null && tools.Count > 0)
            {
                foreach (var tool in tools)//获取借用信息
                {
                    tool.CanSelected = tool.Status;
                    tool.OriStatus = tool.Status;
                    ToolRecord record = tool.ToolRecords.FirstOrDefault(p => !p.IsReturn);
                    if (record != null)
                    {
                        tool.Text2 = "已领用：" + record.User.RealName;
                        if (userId == record.UserId)
                        {
                            currUserTool = tool.id;
                            tool.IsSelected = true;//默认选中  
                            tool.CanSelected = true;//只可以选自己的
                        }
                        else
                        {
                            tool.CanSelected = false;
                        }
                    }
                    else
                    {
                        if (currRecord != null)
                        {
                            tool.CanSelected = false;
                        }
                    }
                    if (tool.Position == 8 || tool.Position == 16)
                    {
                        tool.CanSelected = false;
                        tool.Text2 = "备用";
                    }

                    tool.Text3 = tool.RangeMin + "~" + tool.RangeMax + "N*m";
                }
            }
            return tools;
        }

        public List<Tool> GetTools()
        {

            return Db.Tools.Include("ToolType")?.ToList();
        }

        public bool DeleteTool(int toolId)
        {
            Tool tool = Db.Tools.FirstOrDefault(p => p.id == toolId);
            if (tool != null)
            {
                Db.Tools.Remove(tool);
                Db.SaveChanges();
            }
            return true;
        }

        public ToolRecord GetToolRecord(int uid)
        {
            ToolRecord record = Db.ToolRecords.Include("User").FirstOrDefault(p => p.UserId == uid);
            return record;
        }

        public bool AddTool(Tool tool)
        {
            Db.Tools.Add(tool);
            Db.SaveChanges();
            return true;
        }

        public bool UpdateTool(Tool tool)
        {
            Db.Entry(tool);
            Db.SaveChanges();
            return true;
        }

        public void ResetToolState(int id)
        {
            try
            {
                var rcd = Db.ToolRecords.FirstOrDefault(p => p.ToolId == id && p.IsReturn == false);
                if (rcd != null)
                {
                    rcd.IsReturn = true;
                    rcd.ReturnDate = DateTime.Now;
                }
                var d = Db.Entry(rcd);
                d.State = System.Data.Entity.EntityState.Modified;
                var tool = Db.Tools.FirstOrDefault(p => p.id == id && p.Status == false);
                if (tool != null)
                {
                    tool.Status = true;
                }
                var t = Db.Entry(tool);
                t.State = System.Data.Entity.EntityState.Modified;
                Db.SaveChanges();
            }
            catch (Exception ex)
            {
                LogUtil.WriteLog("重置状态失败！", ex);
            }
        }
    }
}
