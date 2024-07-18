using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BleTempMonitor.Models
{
    [Table("Sensor")]
    public class Sensor
    {
        [PrimaryKey]
        [AutoIncrement]
        public int ID { get; set; }

        public string Alias { get; set; } = "not set";

        public Guid Guid { get; set; }
    }
}
