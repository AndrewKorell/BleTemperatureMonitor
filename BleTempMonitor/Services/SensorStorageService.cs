
using BleTempMonitor.Extensions;
using BleTempMonitor.Helpers;
using BleTempMonitor.Models;

namespace BleTempMonitor.Services
{
    public interface ISensorStorageService
    {
        Task<string> AddOrUpdate(Guid id, string alias);

        Task<string> GetAlias(Guid id);

        Task LogSensorData(int sensorId, double voltage, double temperature);
    }

    public sealed class SensorStorageService : BaseModel, ISensorStorageService
    {
        private readonly SensorDatabaseService database = new SensorDatabaseService();

        public async Task<string> AddOrUpdate(Guid guid, string alias)
        {
            Msg.DebugMessage("Begin Database Add");
            var m = await database.GetItemAsync(guid);
            if(m != null)
            {
                if(m.Alias != alias && alias != string.Empty)
                {
                    m.Alias = alias;
                    await database.SaveItemAsync(m);
                    Msg.DebugMessage($"Updating Alias for {m.Guid} to {alias}");
                }
            }
            else
            {
                //We are saving a new value to the database
                var tempalias = guid.GetTempAlias();
                await database.SaveItemAsync(new Sensor { Alias = tempalias, Guid = guid, ID = 0 } );
                m = database.GetItemAsync(guid).Result;
                Msg.DebugMessage($"Adding {guid} to storage");
            }
            return m.Alias;
        }
        
        public Task<string> GetAlias(Guid id)
        {
            Msg.DebugMessage("Database Get Alias");
            return database.GetItemAliasAsync(id);
        }

        public async Task LogSensorData(int sensorId, double voltage, double temperature)
        {
            Msg.DebugMessage("Adding sensor data to log");
            await database.InsertLogItemAsync(new LogItem { DateTime = DateTime.Now, Voltage=voltage, Temperature=temperature, SensorId=sensorId});
        }

        public async Task<List<LogItem>> ListSensorData()
        {
            Msg.DebugMessage("retrieving all sensor log dota.");
            return await database.GetLogItemsAsync();
        }

        public async Task ClearSensorData()
        {
            Msg.DebugMessage("clearning all sensor log data");
            await database.ClearLogItemTable();
        }
        
        
    }
}
