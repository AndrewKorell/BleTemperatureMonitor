using SQLite;


namespace BleTempMonitor.Models
{
    [Table("Log")]
    public class LogItem
    {
        public LogItem() { }

        public LogItem(double voltage, double temperature, int id)
        {
            DateTime = DateTime.Now;
            Temperature = temperature;
            Voltage = voltage;
            SensorId = id;
        }
        [PrimaryKey]
        [AutoIncrement]
        public int Id { get; set; }

        public DateTime DateTime { get; set; }

        public double Temperature { get; set; }

        public double Voltage { get; set; }   

        //normall I would assign this at foreign key
        //SQLite PCL does not support foreign keys
        public int SensorId { get; set; }
    }
}
