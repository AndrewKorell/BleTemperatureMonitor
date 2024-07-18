
using BleTempMonitor.Extensions;
using BleTempMonitor.Models;

namespace BleTempMonitor.Services
{
    public interface ISensorStorageService
    {
        Task<string> AddOrUpdate(Guid id, string alias);

        Task<string> GetAlias(Guid id);
    }

    public sealed class SensorStorageService : BaseModel, ISensorStorageService
    {
        private readonly SensorDatabaseService database = new SensorDatabaseService();

        public async Task<string> AddOrUpdate(Guid guid, string alias)
        {
            DebugMessage("Begin Database Add");
            var m = await database.GetItemAsync(guid);
            if(m != null)
            {
                if(m.Alias != alias && alias != string.Empty)
                {
                    m.Alias = alias;
                    await database.SaveItemAsync(m);
                    DebugMessage($"Updating Alias for {m.Guid} to {alias}");
                }
            }
            else
            {
                //We are saving a new value to the database
                var tempalias = guid.GetTempAlias();
                await database.SaveItemAsync(new Sensor { Alias = tempalias, Guid = guid, ID = 0 } );
                m = database.GetItemAsync(guid).Result;
                DebugMessage($"Adding {guid} to storage");
            }
            return m.Alias;
        }
        
        public Task<string> GetAlias(Guid id)
        {
            DebugMessage("Database Get Alias");
            return database.GetItemAliasAsync(id);
        }
    }
}
