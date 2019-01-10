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
        private ToolCabinetEntities Db;
        public ToolTypeDao()
        {
            Db = new ToolCabinetEntities("ToolCabinetEntities");
        }
        public List<ToolType> GetToolTypes()
        {
            return Db.ToolTypes?.ToList();
        }

        public bool AddType(ToolType toolType)
        {
            Db.ToolTypes.Add(toolType);
            Db.SaveChanges();
            return true;
        }

        public bool UpdateType(ToolType toolType)
        {
            Db.SaveChanges();
            return true;
        }

        public bool DeleteType(int toolTypeId)
        {
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
