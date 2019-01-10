using System;
using System.Collections.Generic;
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
        private PLCHelper PLC;
        private Thread PLCThread;

        private ToolDao toolDao;
        private ToolRecordDao recordDao;
        public MainViewModel(PLCHelper plc)
        {
            toolDao = new ToolDao();
            recordDao = new ToolRecordDao();
            PLC = plc;
            //PLC.OnReceive += PLCReceive;
            //PLC.DoorClosed += DoorClosed;
            //PLC.ToolStatusChanged += ToolStatusChanged;
            Init();
        }

        /// <summary>
        /// 柜门全部关闭后的回调函数
        /// </summary>
        public Action OnDoorClose;

        public User CurrUser => GlobalData.CurrUser;

        public List<Tool> Tools { get; set; }

        private void Init()
        {
            Tools = toolDao.GetTools(GlobalData.CurrUser.Id);
            foreach (var tool in Tools)
            {
                tool.PropertyChanged -= Tool_PropertyChanged;
                if (tool.IsSelected)//默认选择用户当前已借用工具
                {
                    //PLC.SetGreen(LightStatus.Flash, tool.Position);//默认闪烁
                    GlobalData.CurrTool = tool;
                }
                else
                {
                    //根据工具设置开关指示灯
                    //PLC.SetGreen(tool.Status ? LightStatus.Open : LightStatus.Close, tool.Position);
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

            bool[] status = new bool[16];//初始状态
            for (int i = 0; i < Tools.Count; i++)
            {
                status[i] = Tools[i].Status;
            }
            PLCThread = new Thread(new ParameterizedThreadStart((p) =>
            {
                //PLC.StartMonitor(p as bool[]);
            }));
            PLCThread.Start(status);
        }

        private void Tool_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "IsSelected")
            {
                Tool tool = sender as Tool;
                if (tool.IsSelected)
                {
                    GlobalData.CurrTool = tool;
                    //设置当前选择
                    //PLC.SetGreen(LightStatus.Flash, GlobalData.CurrTool.Position);
                    //把其他工具选中状态改成false
                    foreach (var other in Tools)
                    {
                        if (other != tool)
                        {
                            other.IsSelected = false;
                            if (other.Status)
                            {
                                //PLC.SetGreen(LightStatus.Open, other.Position);
                            }
                            else
                            {
                                //PLC.SetGreen(LightStatus.Open, other.Position);
                            }
                        }
                    }
                }
            }
        }

        private void ToolStatusChanged(int pos, bool status)
        {
            //错拿、错放报警
            if (pos != GlobalData.CurrTool.Position)
            {
                GlobalData.SelectToolCorrect = false;
                //PLC.SetAlarm(true);
            }
            else
            {
                GlobalData.SelectToolCorrect = true;
                //PLC.SetAlarm(false);
            }
        }

        private void DoorClosed()
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
            ToolRecord record = new ToolRecord();
            record.ToolId = GlobalData.CurrTool.id;
            record.UserId = GlobalData.CurrUser.Id;
            record.CreateDateTime = DateTime.Now;
            record.BorrowDate = DateTime.Now;
            record.IsReturn = false;

            OnDoorClose?.Invoke();
        }

        private void PLCReceive(object sender, DataEventArgs e)
        {

        }
    }
}
