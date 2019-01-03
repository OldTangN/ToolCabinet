using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToolMgt.BLL;
using ToolMgt.Common;
using ToolMgt.Model;
using ToolMgt.UI.Common;

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
    }
}
