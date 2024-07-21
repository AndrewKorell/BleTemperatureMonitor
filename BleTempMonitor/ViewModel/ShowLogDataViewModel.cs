using BleTempMonitor.Helpers;
using BleTempMonitor.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BleTempMonitor.ViewModel
{
    public partial class ShowLogDataViewModel : BaseViewModel
    {
        public ObservableCollection<LogItemViewModel> Items { get; set; }

        public ShowLogDataViewModel() 
        {
            Items = [];
            Task.Run(() => UpdateItems());
        }

        private async Task UpdateItems()
        {
            try
            {
                var logItems = await App.SensorStorage.ListLogData();

                foreach (var item in logItems)
                {
                    var alias = await App.SensorStorage.GetAlias(item.SensorId);

                    MainThread.BeginInvokeOnMainThread(() =>
                    {
                        Items.Add(new LogItemViewModel(alias, item));
                    });
                }
            }
            catch (Exception ex)
            {
                Msg.DebugMessage($"Failed to load Log {ex.Message}");
            }
        }

    }
}
