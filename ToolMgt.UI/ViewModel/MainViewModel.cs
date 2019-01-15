using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using ToolMgt.BLL;
using ToolMgt.Common;
using ToolMgt.Model;
using ToolMgt.UI.Common;
using ToolMgt.UI.Controls;

namespace ToolMgt.UI.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private PLCControl plcControl;
        private Thread PLCThread;

        private ToolDao toolDao;
        private ToolRecordDao recordDao;
        public MainViewModel(PLCControl plc)
        {
            toolDao = new ToolDao();
            recordDao = new ToolRecordDao();
            plcControl = plc;
            if (!plcControl.Connected)
            {
                PLCStatus = "PLC连接失败！";
            }
            plcControl.DoorClosed += RaiseDoorClosed;
            plcControl.ToolStatusChanged += RaiseToolStatusChanged;
            //Init();
        }

        /// <summary>
        /// 柜门全部关闭后的回调函数
        /// </summary>
        public Action OnDoorClose { get; set; }

        public User CurrUser => GlobalData.CurrUser;

        public List<Tool> Tools { get; set; }


        public void Init()
        {
            BackgroundWorker worker = new BackgroundWorker();
            worker.DoWork += Worker_DoWork;
            worker.RunWorkerCompleted += Worker_RunWorkerCompleted;
            worker.ProgressChanged += Worker_ProgressChanged;
            worker.WorkerReportsProgress = true;
            worker.RunWorkerAsync();
        }

        private void Worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (e.ProgressPercentage == 0)
            {
                IsBusy = true;
            }
            else
            {
                IsBusy = false;
            }
        }

        private void Worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                MessageAlert.Alert(e.Error.Message);
            }
        }

        private void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            worker.ReportProgress(0);
            Tools = toolDao.GetTools(GlobalData.CurrUser.Id);
            foreach (var tool in Tools)
            {
                tool.PropertyChanged -= Tool_PropertyChanged;
                if (tool.IsSelected)//默认选择用户当前已借用工具
                {
                    plcControl.FlashLight(tool.Position);//默认闪烁
                    GlobalData.CurrTool = tool;
                }
                if (tool.CanSelected)//可选择的工具添加事件
                {
                    tool.PropertyChanged += Tool_PropertyChanged;
                }
            }
            if (PLCThread != null && PLCThread.IsAlive)
            {
                try { PLCThread.Abort(); } catch { }
            }
            bool[] status = OriToolStatus();
            plcControl.OperateLight(status);
            PLCThread = new Thread(new ParameterizedThreadStart((p) =>
            {
                while (true)
                {
                    plcControl.GetStatus(p as bool[]);//new bool[] { false, true }
                    Thread.Sleep(1000);
                }
            }));
            PLCThread.IsBackground = true;
            PLCThread.Start(status);

            worker.ReportProgress(100);
        }

        private bool[] OriToolStatus()
        {
            bool[] status = new bool[16];//初始状态
            for (int i = 0; i < Tools.Count; i++)
            {
                status[i] = Tools[i].Status;
                //status[i] = false;//TODO:测试代码
            }
            //status[0] = true;//TODO:测试代码
            //设置灯光
            return status;
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
                    foreach (var othertool in Tools)
                    {
                        if (othertool != tool && othertool.IsSelected)
                        {
                            othertool.IsSelected = false;
                        }
                    }
                    IsBusy = true;
                    BackgroundWorker lightWorker = new BackgroundWorker();
                    lightWorker.RunWorkerCompleted += LightWorker_RunWorkerCompleted;
                    lightWorker.DoWork += LightWorker_DoWork;
                    lightWorker.RunWorkerAsync();
                }
            }
        }

        private void LightWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker lightWorker = sender as BackgroundWorker;
            //设置当前选择 闪烁
            bool[] status = OriToolStatus();
            plcControl.FlashLight(GlobalData.CurrTool.Position);
            plcControl.OperateLight(status);
        }

        private void LightWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            IsBusy = false;
        }


        private void RaiseToolStatusChanged(int pos, bool status)
        {
            if (GlobalData.CurrTool == null)
            {
                return;
            }
            //错拿、错放报警
            if (pos != GlobalData.CurrTool.Position)
            {
                GlobalData.SelectToolCorrect = false;
                plcControl.OpenAlarm();
            }
            else
            {
                GlobalData.SelectToolCorrect = true;
                plcControl.CloseAlarm();
            }
            if (status)
            {
                plcControl.OpenLight(pos);
            }
            else
            {
                plcControl.CloseLight(pos);
            }
        }

        private void RaiseDoorClosed()
        {
            if (GlobalData.CurrTool == null)
            {
                return;
            }

            if (GlobalData.CurrUser == null || GlobalData.CurrUser.Id == 0)
            {
                return;
            }

            //正常取、放，则关灯
            if (GlobalData.SelectToolCorrect)
            {
                bool rlt;
                //保存记录
                if (GlobalData.CurrTool.Status)//空闲工具添加使用记录
                {
                    rlt = recordDao.AddRecord(GlobalData.CurrTool.id, GlobalData.CurrUser.Id);
                }
                else//已领用工具修改使用记录
                {
                    rlt = recordDao.UpdateRecord(GlobalData.CurrTool.id);
                }
                if (!rlt)
                {
                    MessageAlert.Error("更新工具使用记录失败！");
                }
            }

            //异常取、放，则报警
            //TODO:关灯
            OnDoorClose?.Invoke();
        }

        private string pLCStatus;
        public string PLCStatus { get => pLCStatus; set => Set(ref pLCStatus, value); }
    }
}
