using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BleTempMonitor.ViewModel
{
    public partial class LogViewModel
    {
        public static ReadOnlyObservableCollection<string> Messages => App.Logger.Messages;

        [RelayCommand]
        public async Task ClearLogMessages()
        {
            App.Logger.ClearMessages();
        }
    }
}
