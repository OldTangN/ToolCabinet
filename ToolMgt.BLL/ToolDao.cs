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
            if (tools != null && tools.Count > 0)
            {
                foreach (var tool in tools)//获取借用信息
                {
                    ToolRecord record = tool.ToolRecords.FirstOrDefault(p => !p.IsReturn);
                    if (record != null)
                    {
                        tool.Text2 = record.User.RealName;
                        tool.IsBorrowed = true;
                        if (userId == record.UserId)
                        {
                            tool.IsSelected = true;
                        }
                    }
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
