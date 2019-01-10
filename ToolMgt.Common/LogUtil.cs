using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
[assembly: log4net.Config.XmlConfigurator(Watch = true)]
namespace ToolMgt.Common
{

    public class LogUtil
    {
        public static void WriteLog(string errMsg,Exception ex)
        {
            log4net.ILog log = log4net.LogManager.GetLogger("");
            log.Error(errMsg, ex);
        }

        public static void WriteLog(string msg)
        {
            log4net.ILog log = log4net.LogManager.GetLogger("");
            log.Error(msg);
        }
    }
}
