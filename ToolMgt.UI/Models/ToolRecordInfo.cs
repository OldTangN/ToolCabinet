using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace ToolMgt.UI.Models
{
    [DataContract]
    public class ToolRecordInfo
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public int ToolId { get; set; }

        //public string ToolName { get; set; } = "";
        [DataMember]
        public string ToolType { get; set; } = "";
        [DataMember]
        public string ToolBarCode { get; set; } = "";

        //public string ToolRFIDCode { get; set; } = "";
        [DataMember]
        public int UserId { get; set; }
        [DataMember]
        public string UserName { get; set; } = "";
        [DataMember]
        public string BorrowDate { get; set; }
        [DataMember]
        public string ReturnDate { get; set; }
        [DataMember]
        public bool IsReturn { get; set; }
        [DataMember]
        public string CreateDateTime { get; set; }
    }
}