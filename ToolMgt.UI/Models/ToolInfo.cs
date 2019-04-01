using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ToolMgt.UI.Models
{
    public class ToolInfo
    {
        public ToolInfo()
        {
            
        }

        public int id { get; set; }

        //public string ToolName { get; set; } = "";

        public string ToolType{ get; set; } = "";

        public string ToolBarCode { get; set; } = "";

        //public string ToolRFIDCode { get; set; } = "";

        public int Position { get; set; }

        public string Status { get; set; } = "";

        public string BorrowerName { get; set; } = "";

        public decimal RangeMin { get; set; }

        public decimal RangeMax { get; set; }

        public string Factory { get; set; } = "";

        public string Note { get; set; } = "";


    }
}