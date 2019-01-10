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
using ToolMgt.UI.Common;
using ToolMgt.UI.Controls;

namespace ToolMgt.UI.ViewModel
{
    public class ToolMgtViewModel : ViewModelBase
    {
        private ToolDao dao = new ToolDao();
        private ToolTypeDao typedao = new ToolTypeDao();
        public ToolMgtViewModel()
        {
            ShowTools();
        }

        private Tool currTool;
        private Tool selectTool;
        private List<Tool> _tools;
        private ToolType currType;
        private List<ToolType> toolTypes;

        /// <summary>
        /// 当前编辑项
        /// </summary>
        public Tool CurrTool { get => currTool; set => Set(ref currTool, value); }

        /// <summary>
        /// 列表选择项
        /// </summary>
        public Tool SelectTool { get => selectTool; set => Set(ref selectTool, value); }

        public List<Tool> Tools { get => _tools; set => Set(ref _tools, value); }

        /// <summary>
        /// 当前选择工具类型
        /// </summary>
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

        private void OnCancel(object obj)
        {
            CurrTool = new Tool() { Status = true };
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

        private void OnCommit(object obj)
        {
            if (CurrType == null)
            {
                MessageAlert.Alert("请选择工具类型！");
                return;
            }
            currTool.ToolTypeId = CurrType.id;
            if (CurrTool.id == 0)
            {
                currTool.CreateDateTime = DateTime.Now;
                dao.AddTool(currTool);
            }
            else
            {
                dao.UpdateTool(currTool);
            }
            ShowTools();
        }

        public RelayCommand DeleteCmd
        {
            get
            {
                if (deleteCmd == null)
                {
                    deleteCmd = new RelayCommand(OnDelete);
                }
                return deleteCmd;
            }
        }

        private void OnDelete(object obj)
        {
            try
            {
                if (SelectTool == null)
                {
                    return;
                }
                dao.DeleteTool(SelectTool.id);
            }
            catch (Exception ex)
            {
                MessageAlert.Error(ex.Message);
                return;
            }
            ShowTools();
        }

        private void ShowTools()
        {
            try
            {
                Tools = dao.GetTools();
                ToolTypes = typedao.GetToolTypes();
                CurrTool = new Tool() { Status = true };
            }
            catch (Exception ex)
            {
                MessageAlert.Error(ex.Message);
            }
        }

        private RelayCommand deleteCmd;
        private RelayCommand cancelCmd;
        private RelayCommand commitCmd;
    }
}
