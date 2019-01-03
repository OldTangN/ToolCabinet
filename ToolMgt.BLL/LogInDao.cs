using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToolMgt.Common;
using ToolMgt.Model;

namespace ToolMgt.BLL
{
    public class LogInDao
    {
        public User LogIn(LogInModel model)
        {
            User u;
            try
            {
                ToolCabinetEntities Db = DBContextFactory.GetContext();
                u = Db.Users.FirstOrDefault(p => p.LoginName == model.NameOrCard || p.UserID == model.NameOrCard);
                if (u == null)
                {
                    u = new User() { ErrMsg = "用户名或卡号不存在！" };
                }
                else
                {
                    string md5 = MD5Encrypt.GetMD5(model.Pwd);
                    if (!model.IsCard && !string.Equals(md5, u.Password))//非卡号登录时验证密码
                    {
                        u = new User() { ErrMsg = "用户密码错误！" };
                    }
                }
            }
            catch (Exception ex)
            {
                u = new User() { ErrMsg = ex.Message };
            }
            return u;
        }
    }
}
