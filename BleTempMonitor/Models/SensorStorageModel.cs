using BleTempMonitor.Extensions;

namespace BleTempMonitor.Models
{
    public class SensorStorageModel  : BaseModel
    {
        public SensorStorageModel(Guid id, string alias)
        {
            Id = id;
 
            Alias = string.IsNullOrEmpty(alias) ? id.GetTempAlias() : alias;
        }

        public string Alias { get; set; } = "not set";
        
        public Guid Id { get; set; }
    }
}
