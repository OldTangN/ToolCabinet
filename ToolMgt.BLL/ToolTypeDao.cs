using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToolMgt.Model;

namespace ToolMgt.BLL
{
    public class ToolTypeDao
    {
        public List<ToolType> GetToolTypes()
        {
            ToolCabinetEntities Db = DBContextFactory.GetContext();
            return Db.ToolTypes?.ToList();
        }

        public bool AddType(ToolType toolType)
        {
            ToolCabinetEntities Db = DBContextFactory.GetContext();
            Db.ToolTypes.Add(toolType);
            Db.SaveChanges();
            return true;
        }

        public bool UpdateType(ToolType toolType)
        {
            ToolCabinetEntities Db = DBContextFactory.GetContext();
            Db.SaveChanges();
            return true;
        }

        public bool DeleteType(int toolTypeId)
        {
            ToolCabinetEntities Db = DBContextFactory.GetContext();
            ToolType toolType = Db.ToolTypes.FirstOrDefault(p => p.id == toolTypeId);
            if (toolType != null)
            {
                Db.ToolTypes.Remove(toolType);
                Db.SaveChanges();
            }
            return true;
        }
    }
}
