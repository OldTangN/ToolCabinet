using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;
using ToolMgt.BLL;
using ToolMgt.Common;
using ToolMgt.Model;

namespace ToolMgt.UI.ViewModel
{
    public class ToolMgtViewModel : ViewModelBase
    {
        public ToolMgtViewModel()
        {
            ToolDao dao = new ToolDao();
            Tools = dao.GetTools();
            ToolTypes = dao.GetToolTypes();
        }

        private Tool currTool;
        private List<Tool> _tools;
        private ToolType currType;
        private List<ToolType> toolTypes;

        public Tool CurrTool { get => currTool; set => Set(ref currTool, value); }

        public List<Tool> Tools { get => _tools; set => Set(ref _tools, value); }

        public ToolType CurrType { get => currType; set => Set(ref currType, value); }

        public List<ToolType> ToolTypes { get => toolTypes; set => Set(ref toolTypes, value); }

        public RelayCommand CancelCmd
        {
            get
            {
                if (cancelCmd == null)
                {
                    cancelCmd = new RelayCommand(OnCancel);
                }
                return cancelCmd;
            }
        }

        void OnCancel(object obj)
        {
            CurrTool = new Tool();
        }

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

        void OnCommit(object obj)
        {
            ToolDao dao = new ToolDao();
            currTool.ToolTypeId = CurrType.id;
            if (CurrTool.id == 0)
            {
                dao.AddTool(currTool);
            }
            else
            {
                dao.UpdateTool(currTool);
            }
        }

        private RelayCommand cancelCmd;
        private RelayCommand commitCmd;
    }
}
