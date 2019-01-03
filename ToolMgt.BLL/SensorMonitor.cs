using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToolMgt.BLL
{
    /// <summary>
    /// 传感器监控
    /// </summary>
    public class SensorMonitor
    {
        /// <summary>
        /// 16个传感器状态 true=有扳手，false=无扳手
        /// </summary>
        public bool[] Status = new bool[16];

        public void Start(bool[] normalStatus)
        {
            while (true)
            {

            }
        }

        public void Stop()
        {

        }
    }
}
