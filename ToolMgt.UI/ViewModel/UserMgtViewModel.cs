using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ToolMgt.BLL;
using ToolMgt.Common;
using ToolMgt.Model;
using ToolMgt.UI.Controls;

namespace ToolMgt.UI.ViewModel
{
    public class UserMgtViewModel : ViewModelBase
    {
        public UserMgtViewModel()
        {

        }

        private ObservableCollection<User> _users;
        public ObservableCollection<User> UsersCollection { get => _users; set => Set(ref _users, value); }

        #region 搜索
        public ICommand SearchCmd
        {
            get
            {
                if (searchCmd == null)
                {
                    searchCmd = new RelayCommand(Search);
                }
                return searchCmd;
            }
        }

        private ICommand searchCmd;

        private void Search(object obj)
        {
            IsBusy = true;
            UserDao dao = new UserDao();
            List<User> lstRlt = dao.GetUsers(p => p.LoginName.Contains(LoginName)
                             && p.RealName.Contains(RealName) && p.UserID.Contains(CardId));
            UsersCollection = new ObservableCollection<User>(lstRlt);
            IsBusy = false;
        }
        #endregion

        #region 同步

        private ICommand userSyncCmd;
        public ICommand UserSyncCmd
        {
            get
            {
                if (userSyncCmd == null)
                {
                    userSyncCmd = new RelayCommand(UserSync);
                }
                return userSyncCmd;
            }
        }
        private void UserSync(object obj)
        {
            IsBusy = true;
            UserDao dao = new UserDao();
            bool rlt = false;
            try
            {
                rlt = dao.DataSync();
            }
            catch (Exception ex)
            {
                LogUtil.WriteLog(ex);
                MessageAlert.Alert(ex.Message);
            }
            IsBusy = false;
            if (rlt)
            {
                MessageAlert.Alert("同步成功！");
            }
            else
            {
                MessageAlert.Alert("同步失败！");
            }
        }
        #endregion

        public string LoginName { get => loginName; set => Set(ref loginName, value); }
        public string RealName { get => realName; set => Set(ref realName, value); }
        public string CardId { get => cardId; set => Set(ref cardId, value); }


        private string loginName = "";
        private string realName = "";
        private string cardId = "";
    }
}
