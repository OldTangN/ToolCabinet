using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ToolMgt.UI.Models
{
    [DataContract]
    public class OpenDoorResut
    {
        [DataMember]
        public bool Success { get; set; } = false;
        [DataMember]
        public string Msg { get; set; } = "";
    }
}
