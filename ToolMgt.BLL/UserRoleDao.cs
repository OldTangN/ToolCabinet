using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToolMgt.Model;

namespace ToolMgt.BLL
{
    public class UserRoleDao
    {
        private ToolCabinetEntities Db;
        public UserRoleDao()
        {
            Db = new ToolCabinetEntities("ToolCabinetEntities");
        }
        public UserRole GetUserRole(int userid)
        {
            return Db.UserRoles.FirstOrDefault(p => p.UserId == userid);
        }

        public bool AddUserRole(UserRole ur)
        {
            Db.UserRoles.Add(ur);
            Db.SaveChanges();
            return true;
        }

        public bool UpdateType(UserRole toolType)
        {
            Db.SaveChanges();
            return true;
        }

        public bool DeleteType(int toolTypeId)
        {
            UserRole ur = Db.UserRoles.FirstOrDefault(p => p.Id == toolTypeId);
            if (ur != null)
            {
                Db.UserRoles.Remove(ur);
                Db.SaveChanges();
            }
            return true;
        }
    }
}
