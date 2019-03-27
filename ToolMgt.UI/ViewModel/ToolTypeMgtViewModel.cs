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
    public class ToolTypeMgtViewModel : ViewModelBase
    {
        private ToolTypeDao typeDao = new ToolTypeDao();
        public ToolTypeMgtViewModel()
        {
            ShowToolTypes();
        }

        private ToolType currType;
        private List<ToolType> toolTypes;

        public List<ToolType> ToolTypes { get => toolTypes; set => Set(ref toolTypes, value); }

        /// <summary>
        /// 当前编辑项
        /// </summary>
        public ToolType CurrType { get => currType; set => Set(ref currType, value); }

        /// <summary>
        /// 当前列表选择项
        /// </summary>
        public ToolType SelectType { get => selectType; set => Set(ref selectType, value); }

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
            if (CurrType.id == 0)
            {
                CurrType.CreateDateTime = DateTime.Now;
                typeDao.AddType(CurrType);
            }
            else
            {
                typeDao.UpdateType(CurrType);
            }
            ShowToolTypes();
        }

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
            ShowToolTypes();
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
            if (selectType == null)
            {
                return;
            }
            if (GlobalData.CurrUserRole.RoleId == 1)
            {
                MessageAlert.Alert("只有系统管理员可以进行此操作！");
                return;
            }
            typeDao.DeleteType(selectType.id);
            ShowToolTypes();
        }

        private void ShowToolTypes()
        {
            ToolTypes = typeDao.GetToolTypes();
            CurrType = new ToolType();
        }

        private ToolType selectType;
        private RelayCommand deleteCmd;
        private RelayCommand commitCmd;
        private RelayCommand cancelCmd;
    }
}
