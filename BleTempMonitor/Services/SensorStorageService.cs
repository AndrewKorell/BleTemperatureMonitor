
using BleTempMonitor.Extensions;
using BleTempMonitor.Helpers;
using BleTempMonitor.Models;

namespace BleTempMonitor.Services
{
    public interface ISensorStorageService
    {
        Task<SensorModel> AddOrUpdate(Guid id, string alias);

        Task<string> GetAlias(Guid id);

        Task<string> GetAlias(int id);

        Task InsertLogData(int sensorId, double voltage, double temperature);

        Task<List<LogItem>> ListLogData();

        Task ClearLogData();
    }

    public sealed class SensorStorageService : BaseModel, ISensorStorageService
    {
        private readonly ISensorDatabaseService database = new SensorDatabaseService();

        public async Task<SensorModel> AddOrUpdate(Guid guid, string alias)
        {
            App.Logger.AddMessage("Begin Database Add");
            var m = await database.GetItemAsync(guid);
            if(m != null)
            {
                if(m.Alias != alias && alias != string.Empty)
                {
                    m.Alias = alias;
                    await database.SaveItemAsync(m);
                    App.Logger.AddMessage($"Updating Alias for {m.Guid} to {alias}");
                }
            }
            else
            {
                //We are saving a new value to the database
                var tempalias = guid.GetTempAlias();
                await database.SaveItemAsync(new SensorModel { Alias = tempalias, Guid = guid, ID = 0 } );
                m = await database.GetItemAsync(guid);
                App.Logger.AddMessage($"Adding {guid} to storage");
            }
            return m;
        }

        public async Task<string> GetAlias(int id)
        {
            return await database.GetItemAliasAsync(id);
        }

        public async Task<string> GetAlias(Guid id)
        {
            return await database.GetItemAliasAsync(id);
        }

        public async Task InsertLogData(int sensorId, double voltage, double temperature)
        {
            await database.InsertLogItemAsync(new LogItem { DateTime = DateTime.Now, Voltage=voltage, Temperature=temperature, SensorId=sensorId});
        }

        public async Task<List<LogItem>> ListLogData()
        {
            return await database.GetLogItemsAsync();
        }

        public async Task ClearLogData()
        {
            await database.ClearLogItemTable();
        }
    }
}
