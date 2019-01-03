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
            ToolCabinetEntities Db = new ToolCabinetEntities("ToolCabinetEntities");
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
            ToolCabinetEntities Db = new ToolCabinetEntities("ToolCabinetEntities");
            return Db.Tools.Include("ToolTypes")?.ToList();
        }

        public ToolRecord GetToolRecord(int uid)
        {
            ToolCabinetEntities Db = new ToolCabinetEntities("ToolCabinetEntities");
            ToolRecord record = Db.ToolRecords.Include("Users").FirstOrDefault(p => p.UserId == uid);
            return record;
        }

        public List<ToolType> GetToolTypes()
        {
            ToolCabinetEntities Db = new ToolCabinetEntities("ToolCabinetEntities");
            return Db.ToolTypes?.ToList();
        }

        public bool AddTool(Tool tool)
        {
            ToolCabinetEntities Db = new ToolCabinetEntities("ToolCabinetEntities");
            Db.Tools.Add(tool);
            Db.SaveChanges();
            return true;
        }

        public bool UpdateTool(Tool tool)
        {
            ToolCabinetEntities Db = new ToolCabinetEntities("ToolCabinetEntities");
            Db.Entry(tool);
            Db.SaveChanges();
            return true;
        }
    }
}
