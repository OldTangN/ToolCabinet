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
                    plcControl.FlashToolLamp(tool.Position);//默认闪烁
                    GlobalData.CurrTool = tool;
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
                bool[] lastStatus = p as bool[];
                while (keep)
                {
                    Status currStatus = plcControl.GetStatus(lastStatus);
                    if (currStatus != null)
                    {
                        lastStatus = currStatus.Tool;
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
                    IsBusy = true;
                    BackgroundWorker lightWorker = new BackgroundWorker();
                    lightWorker.RunWorkerCompleted += LightWorker_RunWorkerCompleted;
                    lightWorker.DoWork += LightWorker_DoWork;
                    lightWorker.RunWorkerAsync(tool);
                }
                else
                {
                    if (GlobalData.CurrTool == tool)
                    {
                        GlobalData.CurrTool = null;
                    }
                }
            }
        }

        private void OpenDoor(int toolPos)
        {
            Thread thread = new Thread(() =>
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
            });
            thread.IsBackground = true;
            thread.Start();
        }

        private void LightWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker lightWorker = sender as BackgroundWorker;
            Tool tool = e.Argument as Tool;
            OpenDoor(tool.Position);
            //设置当前选择 闪烁
            bool[] status = OriToolStatus();
            plcControl.FlashToolLamp(tool.Position);
            plcControl.SetToolLamp(status);
            GlobalData.CurrTool = tool;
            //把其他工具选中状态改成false
            foreach (var othertool in Tools)
            {
                if (othertool != tool && othertool.IsSelected)
                {
                    othertool.IsSelected = false;
                }
            }
        }

        private void LightWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            IsBusy = false;
        }

        private void RaiseToolStatusChanged(int pos, bool status)
        {
            if (status)
            {
                plcControl.OpenToolLamp(pos);
            }
            else
            {
                plcControl.CloseToolLamp(pos);
            }

            if (GlobalData.CurrTool == null)
            {
                if (Tools[pos - 1].Status != status)
                {
                    plcControl.OpenAlarm();
                }
                else
                {
                    plcControl.CloseAlarm();
                }
                GlobalData.SelectToolCorrect = false;
            }
            else
            {
                //错拿、错放报警
                if (pos != GlobalData.CurrTool.Position)
                {
                    GlobalData.SelectToolCorrect = false;
                    plcControl.OpenAlarm();
                    plcControl.CloseGreen();
                }
                else
                {
                    GlobalData.SelectToolCorrect = true;
                    plcControl.CloseAlarm();
                    plcControl.OpenGreen();
                }
            }
        }

        private void RaiseDoorClosed()
        {
            if (GlobalData.CurrUser == null || GlobalData.CurrTool == null)
            {
                return;
            }

            keep = false;

            //正常取、放，则关灯
            SaveRecord();
            //TODO:异常取、放，则报警
            //关
            plcControl.CloseAll();
            OnDoorClose?.Invoke();
        }

        public void SaveRecord()
        {
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
            GlobalData.CurrTool = null;
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
    }
}
