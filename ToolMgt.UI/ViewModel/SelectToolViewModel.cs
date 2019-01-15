using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ToolMgt.BLL;
using ToolMgt.Common;
using ToolMgt.Model;
using ToolMgt.UI.Common;
using ToolMgt.UI.Controls;

namespace ToolMgt.UI.ViewModel
{
    public class SelectToolViewModel : ViewModelBase
    {
      
        private ToolDao dao;
        public SelectToolViewModel(List<Tool> tools)
        {
            dao = new ToolDao();
            Tools = tools;
        }
      
        public List<Tool> Tools { get; set; }

        private RelayCommand commitCmd;
        public RelayCommand CommitCmd
        {
            get
            {
                if (commitCmd == null)
                {
                    commitCmd = new RelayCommand(OnCommit);
                }
                return commitCmd;
            }
        }

        private void OnCommit(object obj)
        {
            Tools[0].IsSelected = !Tools[0].IsSelected;
            return;
            if (GlobalData.CurrTool == null)
            {
                MessageAlert.Alert("请选择工具！");
                return;
            }

            if (GlobalData.CurrUser == null || GlobalData.CurrUser.Id == 0)
            {
                MessageAlert.Alert("请登录！");
                return;
            }
        }

        public override void Dispose()
        {
            base.Dispose();
        }
    }
}
