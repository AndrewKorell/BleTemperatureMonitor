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
            Id = temp;
            Alias = App.SensorStorage.Get(temp);
        }

        public SensorDetailsViewModel()
        {
        }


        [ObservableProperty]
        Guid id = Guid.NewGuid();

        [ObservableProperty]
        string alias = "not set";

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
            if (string.IsNullOrEmpty(Alias))
            {
                Alias = App.SensorStorage.Get(Id);
                return;
            }

            App.SensorStorage.AddOrUpdate(Id, Alias);

            IsInEditMode = false;
            IsInViewMode = true;
        }

    }
}
