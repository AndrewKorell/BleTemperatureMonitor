using BleTempMonitor.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BleTempMonitor.ViewModel
{
    public partial class LogItemViewModel : BaseViewModel
    {

        public LogItemViewModel(string alias, LogItem logItem) 
        {
            Alias =alias;
            voltage = logItem.Voltage;
            temperature = logItem.Temperature;
            dateTime = logItem.DateTime;
        }
        
        [ObservableProperty]
        string alias = string.Empty;

        [ObservableProperty]
        double voltage;

        [ObservableProperty]
        double temperature;

        [ObservableProperty]
        DateTime dateTime;
    }
}
