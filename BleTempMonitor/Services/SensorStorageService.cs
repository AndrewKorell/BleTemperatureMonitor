
using BleTempMonitor.Extensions;
using BleTempMonitor.Helpers;
using BleTempMonitor.Models;

namespace BleTempMonitor.Services
{
    public interface ISensorStorageService
    {
        Task<SensorModel?> AddOrUpdate(Guid id, string alias);

        Task<string> GetAlias(Guid id);

        Task<string> GetAlias(int id);

        Task InsertLogData(int sensorId, double voltage, double temperature);

        Task<List<LogItem>> ListLogData();

        Task ClearLogData();
    }

    public sealed class SensorStorageService : BaseModel, ISensorStorageService
    {

        private readonly ISensorDatabaseService _database;
 
        public SensorStorageService() 
        {
            _database = new SensorDatabaseService();
        }

        public SensorStorageService(ISensorDatabaseService database)
        {
            _database = database;
        }

        //string alias may be empty when generating with new ID
        public async Task<SensorModel?> AddOrUpdate(Guid guid, string alias)
        {
            //App.Logger.AddMessage("Begin Database Add");
            var m = await _database.GetItemAsync(guid);
            if(m != null)
            {
                
                if(m.Alias != alias && alias != string.Empty)
                {
                    m.Alias = alias;
                    await _database.SaveItemAsync(m);
                    //App.Logger.AddMessage($"Updating Alias for {m.Guid} to {alias}");
                }
            }
            else
            {
                //We are saving a new value to the database
                var tempalias = guid.GetTempAlias();  //todo: this isn't unit tested
                await _database.SaveItemAsync(new SensorModel { Alias = tempalias, Guid = guid, ID = 0 } );
                m = await _database.GetItemAsync(guid);
                //App.Logger.AddMessage($"Adding {guid} to storage");
            }
            return m;
        }

        public Task<string> GetAlias(int id)
        {
            return _database.GetItemAliasAsync(id);
        }

        public Task<string> GetAlias(Guid id)
        {
            return _database.GetItemAliasAsync(id);
        }

        public Task InsertLogData(int sensorId, double voltage, double temperature)
        {
            return _database.InsertLogItemAsync(new LogItem(voltage, temperature, sensorId));
        }

        public Task<List<LogItem>> ListLogData()
        {
            return _database.GetLogItemsAsync();
        }

        public Task ClearLogData()
        {
            return _database.ClearLogItemTable();
        }
    }
}
