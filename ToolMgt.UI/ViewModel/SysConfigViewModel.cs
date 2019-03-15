using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToolMgt.Common;
using ToolMgt.UI.Common;
using ToolMgt.UI.Controls;

namespace ToolMgt.UI.ViewModel
{
    public class SysConfigViewModel : ViewModelBase
    {
        public string ICReaderType { get; set; } = SysConfiguration.ICReaderType;
        public int ICReaderBaudRate { get; set; } = SysConfiguration.ICReaderBaudRate;
        public int ICReaderPort { get; set; } = SysConfiguration.ICReaderPort;
        public int DoorWaitTime { get; set; } = SysConfiguration.DoorWaitTime;
        public string PLCCom { get; set; } = SysConfiguration.PLCCom;

        public List<string> ICTypes { get; set; } = new List<string>() { "USB", "COM" };
        public SysConfigViewModel()
        {

        }

        public RelayCommand SaveCmd
        {
            get
            {
                if (_saveCmd == null)
                {
                    _saveCmd = new RelayCommand(Save);
                }
                return _saveCmd;
            }
        }

        private RelayCommand _saveCmd;

        private void Save(object obj)
        {
            try
            {
                SysConfiguration.SetConfiguration(nameof(SysConfiguration.ICReaderBaudRate), this.ICReaderBaudRate);
                SysConfiguration.SetConfiguration(nameof(SysConfiguration.ICReaderPort), this.ICReaderPort);
                SysConfiguration.SetConfiguration(nameof(SysConfiguration.ICReaderType), this.ICReaderType);
                SysConfiguration.SetConfiguration(nameof(SysConfiguration.PLCCom), this.PLCCom);
                SysConfiguration.SetConfiguration(nameof(SysConfiguration.DoorWaitTime), this.DoorWaitTime);
                MessageAlert.Alert("保存成功！");
            }
            catch (Exception ex)
            {
                LogUtil.WriteLog("保存配置失败！", ex);
                MessageAlert.Alert("保存配置失败！");
            }
        }
    }
}
