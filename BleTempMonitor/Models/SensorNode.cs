using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BleTempMonitor.Models
{
    public class SensorNode  : BaseModel
    {
        public string FriendlyName { get; set; } = "";
        
        public int TemperatureIndex { get; set; }

        public bool IsConnected { get; set; } 

        public bool LastConnection {  get; set; }

    }
}
