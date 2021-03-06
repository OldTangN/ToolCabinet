﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using ToolMgt.BLL;
using ToolMgt.BLL.ICCard;
using ToolMgt.Common;
using ToolMgt.Model;
using ToolMgt.UI.Common;
using ToolMgt.UI.Controls;

namespace ToolMgt.UI.ViewModel
{
    public class LogInViewModel : ViewModelBase
    {
        private Thread ThreadICReader;
        private ICardHelper card;
        public LogInViewModel()
        {
            CurrLogIn = new LogInModel();
            GlobalData.CurrUser = null;
            string readerType = SysConfiguration.ICReaderType;
            if (string.IsNullOrEmpty(readerType))
            {
                return;
            }

            int readerPort = SysConfiguration.ICReaderPort;
            int readerBaud = SysConfiguration.ICReaderBaudRate;
            if (readerBaud == -1 || readerPort == -1)
            {
                ICReaderStatus = "读卡器配置错误！";
                MessageAlert.Error("读卡器配置错误！");
                return;
            }


            if (readerType == "USB")
            {
                card = new UsbICCard(readerPort, readerBaud);
            }
            else
            {
                card = new ComICCard(readerPort, readerBaud);
            }
            if (card.IsOpen())
            {
                card.HandDataBack += Card_HandDataBack;
                ThreadICReader = new Thread(new ParameterizedThreadStart(ICReadThread));
                ThreadICReader.IsBackground = true;
                ThreadICReader.Start(card);
            }
            else
            {
                ICReaderStatus = "读卡器连接失败！";
            }
        }

        private string iCReaderStatus = "";
        public string ICReaderStatus { get => iCReaderStatus; set => Set(ref iCReaderStatus, value); }
        /// <summary>
        /// 登录后处理
        /// </summary>
        public Action OnLogIn;

        /// <summary>
        /// 当前登录Model
        /// </summary>
        public LogInModel CurrLogIn { get; set; }

        #region IC卡登录
        private void Card_HandDataBack(object sender, CardEventArgs e)
        {
            CurrLogIn.NameOrCard = e.data;
            CurrLogIn.IsCard = true;
            LogInCmd.Execute(null);
        }

        private void ICReadThread(object obj)
        {
            ICardHelper card = obj as ICardHelper;
            card.Read();
        }
        #endregion

        #region 登录按钮
        private ICommand _logInCmd;
        private BackgroundWorker loginWorker;
        /// <summary>
        /// 登录命令
        /// </summary>
        public ICommand LogInCmd => _logInCmd ?? (_logInCmd = new RelayCommand(LogIn, CanLogIn));

        private void LogIn(object obj)
        {
            IsBusy = true;
            loginWorker = new BackgroundWorker();
            loginWorker.DoWork += LoginWorker_DoWork;
            loginWorker.RunWorkerCompleted += LoginWorker_RunWorkerCompleted;
            loginWorker.RunWorkerAsync(obj);
        }

        private void LoginWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            IsBusy = false;
            if (e.Error != null)
            {
                MessageAlert.Error(e.Error.Message);
            }
            User u = e.Result as User;
            if (u.IsError)
            {
                MessageAlert.Error(u.ErrMsg);
            }
            else
            {
                GlobalData.CurrUser = u;
                //获取角色
                UserRoleDao dao = new UserRoleDao();
                GlobalData.CurrUserRole = dao.GetUserRole(u.Id);
                OnLogIn?.Invoke();
            }
        }

        private void LoginWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            LogInDao dao = new LogInDao();
            User u = dao.LogIn(CurrLogIn);
            e.Result = u;
        }

        private bool CanLogIn(object obj)
        {
            return !string.IsNullOrEmpty(CurrLogIn.NameOrCard);
        }

        public override void CDispose()
        {
            card?.Close();
            base.CDispose();
        }
        #endregion
    }
}
