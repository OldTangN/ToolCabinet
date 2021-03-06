﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using ToolMgt.Common;
using ToolMgt.Model;

namespace ToolMgt.BLL
{
    public class UserDao
    {
        private ToolCabinetEntities Db;
        private ToolCabinetEntities DbSrv;
        public UserDao()
        {
            Db = new ToolCabinetEntities("ToolCabinetEntities");
        }
        public bool DataSync()
        {
            try
            {
                DbSrv = new ToolCabinetEntities("ToolCabinetEntitiesSrv");
                if (!DbSrv.Database.Exists())
                {
                    LogUtil.WriteLog("服务器数据库不存在！");
                    return false;
                }
            }
            catch (Exception ex)
            {
                LogUtil.WriteLog("连接数据库失败！", ex);
                return false;
            }
            try
            {
                if (DbSrv.Users.Count() == 0 || DbSrv.UserRoles.Count() == 0)
                {
                    return true;
                }
                //用户数量不会很多，直接采用删除旧数据后全部重新导入的方法
                Db.Users.RemoveRange(Db.Users);
                Db.UserRoles.RemoveRange(Db.UserRoles);
                Db.SaveChanges();

                foreach (User srvu in DbSrv.Users)
                {
                    User u = new User()
                    {
                        Id = srvu.Id,
                        CompanyID = srvu.CompanyID,
                        CreateDateTime = srvu.CreateDateTime,
                        Face = srvu.Face,
                        IsDeleted = srvu.IsDeleted,
                        LoginName = srvu.LoginName,
                        Nfc = srvu.Nfc,
                        Password = srvu.Password,
                        RealName = srvu.RealName,
                        Status = srvu.Status,
                        Token = srvu.Token,
                        UserID = srvu.UserID,
                        UserImg = srvu.UserImg
                    };
                    Db.Users.Add(u);
                }
                foreach (UserRole srvur in DbSrv.UserRoles)
                {
                    UserRole ur = new UserRole()
                    {
                        Id = srvur.Id,
                        UserId = srvur.UserId,
                        RoleId = srvur.RoleId,
                        CreateDateTime = srvur.CreateDateTime,
                        IsDeleted = srvur.IsDeleted
                    };
                    Db.UserRoles.Add(ur);
                }
                Db.SaveChanges();
            }
            catch (Exception ex)
            {
                LogUtil.WriteLog("DataSync失败！", ex);
                return false;
            }
            return true;
        }


        public List<User> GetUsers(Expression<Func<User, bool>> expression)
        {
            List<User> lstResult;
            try
            {
                lstResult = Db.Users.Where(expression)?.ToList();
            }
            catch (Exception ex)
            {
                LogUtil.WriteLog("查询用户失败！", ex);
                return null;
            }
            return lstResult;
        }
    }
}
