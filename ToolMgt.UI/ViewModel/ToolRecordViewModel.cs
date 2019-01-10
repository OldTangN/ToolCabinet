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
    public class ToolRecordViewModel : ViewModelBase
    {
        private ToolRecordDao dao;
        public ToolRecordViewModel()
        {
            BorrowStart = DateTime.Now;
            BorrowEnd = DateTime.Now;
            dao = new ToolRecordDao();
        }

        public List<string> Status => new List<string> { "全部", "未领用", "已领用" };

        private RelayCommand searchCmd;

        public RelayCommand SearchCmd
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

        public DateTime BorrowStart { get => borrowStart; set => Set(ref borrowStart, value); }
        public DateTime BorrowEnd { get => borrowEnd; set => Set(ref borrowEnd, value); }
        public string NameOrCard { get => nameOrCard; set => Set(ref nameOrCard, value); }
        public string Barcode { get => barcode; set => Set(ref barcode, value); }
        public List<ToolRecord> Records { get => records; set => Set(ref records, value); }
        /// <summary>
        /// 全部、未归还、已归还
        /// </summary>
        public string SelectStatus { get => selectStatus; set => Set(ref selectStatus, value); }

        private DateTime borrowStart;
        private DateTime borrowEnd;
        private string nameOrCard = "";
        private string barcode = "";
        private string selectStatus = "全部";
        private List<ToolRecord> records;

        private void Search(object obj)
        {
            try
            {
                DateTime start = new DateTime(BorrowStart.Year, BorrowStart.Month, BorrowStart.Day);
                DateTime end = new DateTime(BorrowEnd.Year, BorrowEnd.Month, BorrowEnd.Day + 1);
                Records = dao.GetRecords(SelectStatus, Barcode, NameOrCard, start, end);
            }
            catch (Exception ex)
            {
                LogUtil.WriteLog("查询记录失败！", ex);
                MessageAlert.Alert("查询记录失败！");
            }
        }
    }
}
