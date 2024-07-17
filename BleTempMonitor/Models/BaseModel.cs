using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BleTempMonitor.Models
{
    public class BaseModel
    {
        protected void ShowMessage(string message)
        {
            DebugMessage(message);
            App.AlertSvc.ShowAlert("BLE Scanner", message);
        }

        protected void DebugMessage(string message)
        {
            Debug.WriteLine(message);
            App.Logger.AddMessage(message);
        }
    }
}
