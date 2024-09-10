using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BleTempMonitor.Services
{
    public interface ILogService
    {
        void ClearMessages();

        void AddMessage(string message);

        ReadOnlyObservableCollection<string> Messages { get; init; }
    }
    
    public class LogService : ILogService
    {
        private readonly ObservableCollection<string> LogMessages = [];
        public ReadOnlyObservableCollection<string> Messages { get; init; }

        public LogService()
        {
            Messages = new(LogMessages);
        }

        public void ClearMessages()
        {
            if(MainThread.IsMainThread)
            {
                LogMessages.Clear();
            }
            else
            {
                MainThread.BeginInvokeOnMainThread(() => { LogMessages.Clear(); });
            }
        }

        public void AddMessage(string message)
        {
            if(MainThread.IsMainThread)
            {
                LogMessages.Add(message);
            }
            else
            {
                MainThread.BeginInvokeOnMainThread(() => { LogMessages.Add(message); });
            }
         
        }
    }
}
