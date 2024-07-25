using BleTempMonitor.Helpers;
using BleTempMonitor.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
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
            if (IsUpdating) return;

            IsUpdating = true;
            try
            {
                Items.Clear();

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
            finally
            {
                IsUpdating = false;
            }

        }

        [RelayCommand]
        async Task ClearLog()
        {
            if (IsUpdating) return;

            try
            {
                IsUpdating = true;
                await App.SensorStorage.ClearLogData();
                IsUpdating = false;
                await UpdateItems();

            }
            catch (Exception ex)
            {
                Msg.DebugMessage($"Error clearing log {ex.Message}");
            }
            finally
            {
                IsUpdating = false;
            }
        }

        [ObservableProperty]
        bool isUpdating;
    }
}
