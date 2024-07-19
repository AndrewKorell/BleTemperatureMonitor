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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BleTempMonitor.Services
{
    public interface ISensorDatabaseService
    {

    }

    public class SensorDatabaseService
    {
        SQLiteAsyncConnection Database;

        public SensorDatabaseService() { }

        async Task Init()
        {
            if (Database is not null)
                return;

            Database = new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);
            //await Database.EnableWriteAheadLoggingAsync();
            var result = await Database.CreateTableAsync<Sensor>();
        }

        public async Task<List<Sensor>> GetItemsAsync()
        {
            await Init();
            return await Database.Table<Sensor>().ToListAsync();
        }


        public async Task<Sensor> GetItemAsync(int id)
        {
            await Init();
            return await Database.Table<Sensor>().Where(i => i.ID == id).FirstOrDefaultAsync();
        }

        public async Task<Sensor> GetItemAsync(Guid guid)
        {
            await Init();
            return await Database.Table<Sensor>().Where(i => i.Guid == guid).FirstOrDefaultAsync();
        }

        public async Task<int> GetItemIDAsync(Guid guid)
        {
            await Init();
            var item = await Database.Table<Sensor>().Where(i => i.Guid == guid).FirstOrDefaultAsync();
            return item == null ? 0 : item.ID;
        }

        public async Task<string> GetItemAliasAsync(Guid guid)
        {
            await Init();
            var item = await Database.Table<Sensor>().Where(i => i.Guid == guid).FirstOrDefaultAsync();
            return item == null ? "error" : item.Alias;
        }
        public async Task<int> SaveItemAsync(Sensor item)
        {
            await Init();
            if (item.ID != 0)
                return await Database.UpdateAsync(item);
            else
                return await Database.InsertAsync(item);
        }

        public async Task<int> DeleteItemAsync(Sensor item)
        {
            await Init();
            return await Database.DeleteAsync(item);
        }

        public async Task<int> InsertLogItemAsync(LogItem item)
        {
            await Init();
            return await Database.InsertAsync(item);
        }
    }
}
