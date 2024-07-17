using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BleTempMonitor.Services
{
    public interface ISettingsService 
    {
        double VoltageScale { set; get; }
        double TmpScale { set; get; }
    }

    public sealed class SettingsService : ISettingsService
    {
        private const double voltageScale = 1.0 / 1000.0; //used as multiplier
        private const double tmpScale = 1.0 / 1000.0;  //used as multiplier
        
        public double VoltageScale
        {
            get => Preferences.Get(nameof(voltageScale), voltageScale);
            set => Preferences.Set(nameof(voltageScale), value);
        }

        public double TmpScale
        {
            get => Preferences.Get(nameof(TmpScale), tmpScale);
            set => Preferences.Set(nameof(TmpScale), value);
        }

    }
}
