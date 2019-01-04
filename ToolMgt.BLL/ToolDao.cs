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
        public List<Tool> GetTools(int userId)
        {
            ToolCabinetEntities Db = DBContextFactory.GetContext();
            List<Tool> tools = Db.Tools?.OrderBy(p => p.Position).ToList();
            bool notborrowed = true;//当前用户未借出工具
            if (tools != null && tools.Count > 0)
            {
                foreach (var tool in tools)//获取借用信息
                {
                    tool.CanSelected = tool.Status;
                    ToolRecord record = tool.ToolRecords.FirstOrDefault(p => !p.IsReturn);
                    if (record != null)
                    {
                        tool.Text2 = record.User.RealName;
                        if (userId == record.UserId)
                        {
                            notborrowed = false;
                            tool.IsSelected = true;
                        }
                    }
                }
            }
            if (!notborrowed)
            {
                foreach (var tool in tools)//当前用户已借出工具，不允许再次选择工具
                {
                    tool.CanSelected = false;
                }
            }
            return tools;
        }

        public List<Tool> GetTools()
        {
            ToolCabinetEntities Db = DBContextFactory.GetContext();
            return Db.Tools.Include("ToolType")?.ToList();
        }

        public bool DeleteTool(int toolId)
        {
            ToolCabinetEntities Db = DBContextFactory.GetContext();
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
            ToolCabinetEntities Db = DBContextFactory.GetContext();
            ToolRecord record = Db.ToolRecords.Include("User").FirstOrDefault(p => p.UserId == uid);
            return record;
        }

        public bool AddTool(Tool tool)
        {
            ToolCabinetEntities Db = DBContextFactory.GetContext();
            Db.Tools.Add(tool);
            Db.SaveChanges();
            return true;
        }

        public bool UpdateTool(Tool tool)
        {
            ToolCabinetEntities Db = DBContextFactory.GetContext();
            Db.Entry(tool);
            Db.SaveChanges();
            return true;
        }
    }
}
