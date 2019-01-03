using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToolMgt.Model
{
    public class DBContextFactory
    {
        private static ToolCabinetEntities context;

        private static readonly object lockobj =new { };
        public static ToolCabinetEntities GetContext()
        {
            if (context == null)
            {
                lock (lockobj)
                {
                    if (context == null)
                    {
                        context = new ToolCabinetEntities("ToolCabinetEntities");
                    }
                }
            }
            return context;
        }
    }
}
