/*
 * This whole file is mainly taken from microsoft 
 * It will be changed where changes are required
 * beyond that I am interested in their implementation. 
 * https://learn.microsoft.com/en-us/dotnet/maui/data-cloud/database-sqlite?view=net-maui-8.0
 * 
 * 
 */


using BleTempMonitor.Models;
using SQLite;

namespace BleTempMonitor.Services
{
    public interface ISensorDatabaseService
    {
        Task<List<SensorModel>> GetItemsAsync();
        Task<SensorModel?> GetItemAsync(Guid guid);
        Task<int> GetItemIDAsync(Guid guid);
        Task<string> GetItemAliasAsync(Guid guid);
        Task<string> GetItemAliasAsync(int id);
        Task<int> SaveItemAsync(SensorModel item);
        Task<int> InsertLogItemAsync(LogItem item);
        Task<int> DeleteItemAsync(SensorModel item);
        Task<List<LogItem>> GetLogItemsAsync();
        Task ClearLogItemTable();
    }

    public class SensorDatabaseService : ISensorDatabaseService
    {
        protected ISQLiteAsyncConnection Database;

        public SensorDatabaseService(ISQLiteAsyncConnection database) { Database = database; }

        public SensorDatabaseService() 
        {
            Database = new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);

            //await Database.EnableWriteAheadLoggingAsync();
            Database.CreateTableAsync<SensorModel>().GetAwaiter().GetResult();
            Database.CreateTableAsync<LogItem>().GetAwaiter().GetResult();
        }

        public Task<List<SensorModel>> GetItemsAsync()
        {
            return Database.Table<SensorModel>().ToListAsync();
        }


        public Task<SensorModel?> GetItemAsync(int id)
        {
            return Database.Table<SensorModel?>().Where(predExpr: i => i.ID == id).FirstOrDefaultAsync();
        }

        public Task<SensorModel?> GetItemAsync(Guid guid)
        {
            return Database.Table<SensorModel?>().Where(predExpr: i => i.Guid == guid).FirstOrDefaultAsync();
        }

        public async Task<int> GetItemIDAsync(Guid guid)
        {
            var item = await Database.Table<SensorModel>().Where(i => i.Guid == guid).FirstOrDefaultAsync();
            return item == null ? 0 : item.ID;
        }

        public async Task<string> GetItemAliasAsync(Guid guid)
        {
            var item = await Database.Table<SensorModel>().Where(i => i.Guid == guid).FirstOrDefaultAsync();
            return item == null ? "Error" : item.Alias;
        }

        public async Task<string> GetItemAliasAsync(int id)
        {
            var item = await Database.Table<SensorModel>().Where(i => i.ID == id).FirstOrDefaultAsync();
            return item == null ? "Error" : item.Alias;
        }
        public async Task<int> SaveItemAsync(SensorModel item)
        {
            if (item.ID != 0)
                return await Database.UpdateAsync(item);
            else
                return await Database.InsertAsync(item);
        }

        public Task<int> DeleteItemAsync(SensorModel item)
        {
            return Database.DeleteAsync(item);
        }

        public Task<int> InsertLogItemAsync(LogItem item)
        {
            return Database.InsertAsync(item);
        }

        public Task<List<LogItem>> GetLogItemsAsync()
        {
            return Database.Table<LogItem>().ToListAsync();
        }

        public Task ClearLogItemTable()
        {
            return Database.Table<LogItem>().DeleteAsync(d => d.Id > 0);
        }
    }
}
