using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BleTempMonitor.Services
{
    public class LogService
    {
        private readonly ObservableCollection<string> LogMessages = [];
        public ReadOnlyObservableCollection<string> Messages { get; init; }

        public LogService()
        {
            Messages = new(LogMessages);
        }

        public void ClearMessages()
        {
            MainThread.BeginInvokeOnMainThread(LogMessages.Clear);
        }

        public void AddMessage(string message)
        {
            MainThread.BeginInvokeOnMainThread(() => { LogMessages.Add(message); });
        }
    }
}
