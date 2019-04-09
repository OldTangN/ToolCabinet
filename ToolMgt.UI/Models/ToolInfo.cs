using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace ToolMgt.UI.Models
{
    [DataContract]
    public class ToolInfo
    {
        public ToolInfo()
        {
            
        }
        [DataMember]
        public int id { get; set; }

        //public string ToolName { get; set; } = "";
        [DataMember]
        public string ToolType{ get; set; } = "";
        [DataMember]
        public string ToolBarCode { get; set; } = "";

        //public string ToolRFIDCode { get; set; } = "";
        [DataMember]
        public int Position { get; set; }
        [DataMember]
        public string Status { get; set; } = "";
        [DataMember]
        public string BorrowerName { get; set; } = "";
        [DataMember]
        public decimal RangeMin { get; set; }
        [DataMember]
        public decimal RangeMax { get; set; }
        [DataMember]
        public string Factory { get; set; } = "";
        [DataMember]
        public string Note { get; set; } = "";


    }
}