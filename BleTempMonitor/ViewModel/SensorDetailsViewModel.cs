using BleTempMonitor.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace BleTempMonitor.ViewModel
{
    [QueryProperty(nameof(Id), "sensorid")]
    public partial class SensorDetailsViewModel : BaseViewModel, IQueryAttributable
    {
        //private readonly SensorStorageService sensorService;

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            var q = (string) query["Id"];
            Guid temp;
            if (!Guid.TryParse(q, out temp))
            {
                throw new Exception("Failed to read Guid from details");
            }

            MainThread.BeginInvokeOnMainThread(async () =>
            {
                Id = temp;
                Alias = await App.SensorStorage.GetAlias(temp);
            });
        }

        public SensorDetailsViewModel()
        {
        }

        [ObservableProperty]
        Guid id = Guid.NewGuid();

        [ObservableProperty]
        string alias = "not set";

        [ObservableProperty]
        string newAlias = "";

        [ObservableProperty]
        bool isInViewMode = true;

        [ObservableProperty]
        bool isInEditMode = false;

        [RelayCommand]
        async Task ChangeName()
        {
            IsInViewMode = false;
            IsInEditMode = true;

        }

        [RelayCommand]
        async Task SaveName()
        {
            string newName;
            if (string.IsNullOrEmpty(NewAlias))
            {
                newName = await App.SensorStorage.GetAlias(Id);
            }
            else
            {
                newName = NewAlias;
                await App.SensorStorage.AddOrUpdate(Id, newName);
            }

            Alias = newName;

            IsInEditMode = false;
            IsInViewMode = true;
        }

    }
}
