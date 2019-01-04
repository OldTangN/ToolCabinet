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
        private PLCHelper PLC;
        private Thread PLCThread;
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
            PLC = new PLCHelper();
        }

        private void Tool_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "IsSelected")
            {
                Tool tool = sender as Tool;
                if (tool.IsSelected)
                {
                    GlobalData.CurrTool = tool;
                    //把其他工具选中状态改成false
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

            if (PLCThread != null || PLCThread.IsAlive)
            {
                try { PLCThread.Abort(); } catch { }
            }

            bool[] status = new bool[16];//初始状态
            for (int i = 0; i < Tools.Count; i++)
            {
                status[i] = Tools[i].Status;
            }

            PLC.OpenYellow(GlobalData.CurrTool.Position);

            PLCThread = new Thread(new ParameterizedThreadStart((p) => { PLC.StartMonitor(p as bool[]); }));
            PLC.OnReceive -= PLCReceive;
            PLC.OnReceive += PLCReceive;
            PLC.DoorStatusChanged -= DoorStatusChanged;
            PLC.DoorStatusChanged += DoorStatusChanged;
            PLC.ToolStatusChanged -= ToolStatusChanged;
            PLC.ToolStatusChanged += ToolStatusChanged;
        }

        /// <summary>
        /// 被选工具是否取出/放入
        /// </summary>
        public bool SelectToolStatusChanged = false;

        private void ToolStatusChanged(int pos, bool status)
        {
            if (pos != GlobalData.CurrTool.Position)
            {
                SelectToolStatusChanged = false;
                PLC.OpenAlarm(pos);
                PLC.OpenRed(pos);
            }
            else
            {
                if (!SelectToolStatusChanged)
                {
                    SelectToolStatusChanged = true;
                }
                
                PLC.CloseAlarm();
                for (int i = 1; i <= 16; i++)
                {
                    PLC.CloseRed(i);
                }
            }
        }

        private void DoorStatusChanged(bool status)
        {
            if (status == false)
            {
                //正常取、放，则关灯
                if (SelectToolStatusChanged)
                {

                }
                //异常取、放，则报警
                //TODO:关灯
                ToolRecord record = new ToolRecord();
                record.ToolId = GlobalData.CurrTool.id;
                record.UserId = GlobalData.CurrUser.Id;
                record.CreateDateTime = DateTime.Now;
                record.BorrowDate = DateTime.Now;
                record.IsReturn = false;
            }
        }

        private void PLCReceive(object sender, DataEventArgs e)
        {

        }
    }
}
