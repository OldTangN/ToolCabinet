using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToolMgt.Common;
using ToolMgt.Model;
using System.Linq.Expressions;

namespace ToolMgt.BLL
{
    public class UserDao
    {
        public bool DataSync()
        {
            ToolCabinetEntities DbSrv, Db;
            try
            {
                DbSrv = new ToolCabinetEntities("ToolCabinetEntitiesSrv");
                Db = new ToolCabinetEntities("ToolCabinetEntities");
            }
            catch (Exception ex)
            {
                LogUtil.WriteLog(ex);
                return false;
            }
            try
            {
                //用户数量不会很多，直接采用删除旧数据后全部重新导入的方法
                Db.Users.RemoveRange(Db.Users);
                Db.SaveChanges();
                Db.Users.AddRange(DbSrv.Users);
                Db.SaveChanges();
            }
            catch (Exception ex)
            {
                LogUtil.WriteLog(ex);
                return false;
            }
            return true;
        }


        public List<User> GetUsers(Expression<Func<User,bool>> expression)
        {
            ToolCabinetEntities Db;
            List<User> lstResult;
            try
            {
                Db = new ToolCabinetEntities("ToolCabinetEntities");
                lstResult = Db.Users.Where(expression)?.ToList();
            }
            catch (Exception ex)
            {
                LogUtil.WriteLog(ex);
                return null;
            }
            return lstResult;
        }
    }
}
