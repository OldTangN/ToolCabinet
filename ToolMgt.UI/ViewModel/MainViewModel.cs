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
            //plcControl.DoorClosed += RaiseDoorClosed;
            //plcControl.ToolStatusChanged += RaiseToolStatusChanged;
            plcControl.StatusReceived += OnStatusReceived;
        }

        private Status CurrStatus;

        private void OnStatusReceived(Status status)
        {
            CurrStatus = status;
            bool errorOperate = false;//是否有非法操作
            //判断工具状态变化
            for (int i = 0; i < Tools.Count; i++)
            {
                var tool = Tools[i];
                if (tool.IsSelected)//选中状态
                {
                    if (tool.OriStatus == CurrStatus.Tool[i])//状态未发生变化
                    {
                        //设置闪烁
                        plcControl.FlashToolLamp(i + 1, true);
                    }
                    else
                    {
                        //关闭闪烁
                        plcControl.FlashToolLamp(i + 1, false);
                        //根据状态设置长亮或长灭
                        plcControl.SetToolLamp(i + 1, CurrStatus.Tool[i]);
                    }
                }
                else//非选中状态
                {
                    if (tool.OriStatus != CurrStatus.Tool[i])//非法操作
                    {
                        errorOperate = true;
                    }
                }
            }
            if (errorOperate)
            {
                NoError = false;
                plcControl.OpenAlarm();
                plcControl.CloseGreen();
            }
            else
            {
                NoError = true;
                plcControl.CloseAlarm();
                plcControl.OpenGreen();
            }
            //判断柜门关闭状态
            if (CurrStatus.Lock[0] && CurrStatus.Lock[1])
            {
                SaveRecord();
                plcControl.CloseAll();
                OnDoorClose?.Invoke();
            }
        }

        /// <summary>
        /// 初始化完成回调
        /// </summary>
        public Action OnInitComplete { get; set; }

        /// <summary>
        /// 柜门全部关闭后的回调函数
        /// </summary>
        public Action OnDoorClose { get; set; }

        public User CurrUser => GlobalData.CurrUser;

        public List<Tool> Tools { get; set; }

        public void Init()
        {
            IsBusy = true;
            BackgroundWorker initWorker = new BackgroundWorker();
            initWorker.DoWork += InitWorker_DoWork;
            initWorker.RunWorkerCompleted += InitWorker_RunWorkerCompleted;
            initWorker.RunWorkerAsync();
        }

        private void InitWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            IsBusy = false;
            if (e.Error != null)
            {
                MessageAlert.Alert(e.Error.Message);
            }
            OnInitComplete?.Invoke();
        }

        private void InitWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            plcControl.OpenLight();
            BackgroundWorker worker = sender as BackgroundWorker;
            Tools = toolDao.GetTools(GlobalData.CurrUser.Id);
            foreach (var tool in Tools)
            {
                tool.PropertyChanged -= Tool_PropertyChanged;
                if (tool.IsSelected)//默认选择用户当前已借用工具
                {
                    plcControl.FlashToolLamp(tool.Position, true);//默认闪烁
                    SelectedTools.Add(tool);
                    OpenDoor(tool.Position);
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
            plcControl.SetToolLamp(status);
            PLCThread = new Thread(new ParameterizedThreadStart((p) =>
            {

                while (keep)
                {
                    Status currStatus = plcControl.GetStatus();
                    //if (currStatus != null)
                    {
                        CurrStatus = currStatus;
                    }
                    Thread.Sleep(200);
                }
            }));
            PLCThread.IsBackground = true;
            PLCThread.Start(status);
        }

        /// <summary>
        /// 持续读取PLC状态
        /// </summary>
        private bool keep = true;
        private bool[] OriToolStatus()
        {
            bool[] status = new bool[16];//初始状态
            for (int i = 0; i < Tools.Count; i++)
            {
                status[i] = Tools[i].OriStatus;
            }
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
                    if (!SelectedTools.Contains(tool))
                    {
                        SelectedTools.Add(tool);
                    }
                    IsBusy = true;
                    BackgroundWorker openDoorWorker = new BackgroundWorker();
                    openDoorWorker.RunWorkerCompleted += OpenDoorWorker_RunWorkerCompleted;
                    openDoorWorker.DoWork += OpenDoor_DoWork;
                    openDoorWorker.RunWorkerAsync(tool);
                }
                else
                {
                    if (SelectedTools.Contains(tool))
                    {
                        SelectedTools.Remove(tool);
                    }
                }
            }
        }

        private void OpenDoor(int toolPos)
        {
            int doorWaitTime = SysConfiguration.DoorWaitTime;
            if (toolPos <= 8)//根据选择的工具位置开门
            {
                plcControl.OpenDoor(1, doorWaitTime);
            }
            else
            {
                plcControl.OpenDoor(2, doorWaitTime);
            }
        }

        private void OpenDoor_DoWork(object sender, DoWorkEventArgs e)
        {
            Tool tool = e.Argument as Tool;
            if (CurrStatus == null || CurrStatus.Lock[tool.Position / 8] == false)
            {
                OpenDoor(tool.Position);
            }
        }

        private void OpenDoorWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            IsBusy = false;
        }
        
        private void RaiseDoorClosed()
        {
            keep = false;
            SaveRecord();
            plcControl.CloseAll();
            OnDoorClose?.Invoke();
        }

        public void SaveRecord()
        {
            if (!NoError)
            {
                MessageAlert.Alert("操作错误 或 未检测到工具状态，请重新领用！");
            }
            if (NoError && SelectedTools.Count > 0)
            {
                bool rlt = true;
                foreach (var tool in SelectedTools)
                {
                    //保存记录
                    if (tool.OriStatus)//空闲工具添加使用记录
                    {
                        rlt = recordDao.AddRecord(tool.id, GlobalData.CurrUser.Id);
                    }
                    else//已领用工具修改使用记录
                    {
                        rlt = recordDao.UpdateRecord(tool.id);
                    }
                }
                if (!rlt)
                {
                    MessageAlert.Error("更新工具使用记录失败！");
                }
            }
            SelectedTools.Clear();
        }

        private string pLCStatus;
        public string PLCStatus { get => pLCStatus; set => Set(ref pLCStatus, value); }

        public override void Dispose()
        {
            plcControl.CloseAll();
            plcControl.DisConnect();
            try { PLCThread.Abort(); } catch { }
            base.Dispose();
        }

        private bool NoError = false;

        /// <summary>
        /// 当前选择扳手
        /// </summary>
        private List<Tool> SelectedTools = new List<Tool>();
    }
}
