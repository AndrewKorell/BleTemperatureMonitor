
using BleTempMonitor.Models;

namespace BleTempMonitor.Services
{
    public interface ISensorStorageService
    {
        string AddOrUpdate(Guid id, string? alias=null);

        bool IsStored(Guid id);

        string Get(Guid id);
    }

    public sealed class SensorStorageService : BaseModel, ISensorStorageService
    {
        private readonly List<SensorStorageModel> archive = new List<SensorStorageModel>();

        public string AddOrUpdate(Guid id, string alias)
        {
            var m = archive.FirstOrDefault(x => x.Id == id);
            if(m != null)
            {
                if(m.Alias != alias && alias != null)
                {
                    m.Alias = alias;
                    DebugMessage($"Updating Alias for {m.Id.ToString()} to {alias}");
                }
            }
            else
            {
                m = new SensorStorageModel(id, alias);
                archive.Add(m);
                DebugMessage($"Adding {id.ToString()} to storage");
            }
            return m.Alias;
        }
        
        public bool IsStored(Guid id)
        {
            return archive.Any(x => x.Id == id);
        }
        
        public string Get(Guid id)
        {
            var s = archive.FirstOrDefault(x => x.Id == id);
            return s == null ? "error" : s.Alias;
        }
    }
}
