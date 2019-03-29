using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ToolCabinet.Web.Models
{
    public class ToolRecordInfo
    {
        public int Id { get; set; }

        public int ToolId { get; set; }

        //public string ToolName { get; set; } = "";

        public string ToolType { get; set; } = "";

        public string ToolBarCode { get; set; } = "";

        //public string ToolRFIDCode { get; set; } = "";

        public int UserId { get; set; }

        public string UserName { get; set; } = "";

        public System.DateTime BorrowDate { get; set; }

        public Nullable<System.DateTime> ReturnDate { get; set; }

        public bool IsReturn { get; set; }

        public DateTime CreateDateTime { get; set; }
    }
}