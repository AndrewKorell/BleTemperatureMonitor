using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BleTempMonitor.Models
{
    public class AdverstisementModel
    {    
        public byte[]? Flags { set; get; }

        public byte[]? ServiceData { set; get; } 

        public byte[]? TxPowerLevel { set; get; }

        public byte[]? CompleteLocalNameData { set; get; }

        public byte[]? ManufacturerSpecificData { get; set; }

        public string CompleteLocalName()
        {
            return CompleteLocalNameData == null ? "not set" : System.Text.Encoding.Default.GetString(CompleteLocalNameData);
        }
    }
}
