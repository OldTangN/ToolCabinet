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
               
        public override void Dispose()
        {
            base.Dispose();
        }
    }
}
