using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        public SelectToolViewModel()
        {
            Init();
        }

        private void Init()
        {
            ToolDao dao = new ToolDao();
            Tools = dao.GetTools(GlobalData.CurrUser.Id);
            foreach (var tool in Tools)
            {
                tool.PropertyChanged += Tool_PropertyChanged;
                if (tool.IsSelected)
                {
                    GlobalData.CurrTool = tool;
                }
            }
        }

        private void Tool_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "IsSelected")
            {
                Tool tool = sender as Tool;
                if (tool.IsSelected)
                {
                    GlobalData.CurrTool = tool;
                    //把其他改成false
                    foreach (var other in Tools)
                    {
                        if (other != tool)
                        {
                            other.IsSelected = false;
                        }
                    }
                }
            }
        }

        public List<Tool> Tools { get; set; }

        private RelayCommand commitCmd;
        public RelayCommand CommitCmd
        {
            get
            {
                if (commitCmd == null)
                {
                    commitCmd = new RelayCommand();
                }
                return commitCmd;
            }
        }

        private void OnCommit(object obj)
        {

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

            ToolRecord record = new ToolRecord();
            record.ToolId = GlobalData.CurrTool.id;
            record.UserId = GlobalData.CurrUser.Id;
            record.CreateDateTime = DateTime.Now;
            record.BorrowDate = DateTime.Now;
            record.IsReturn = false;
        }
    }
}
