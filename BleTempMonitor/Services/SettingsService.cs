using BleTempMonitor.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BleTempMonitor.Services
{
    public interface ISettingsService 
    {
        double VoltScale { set; get; }
        double TmpScale { set; get; }
    }


        
        


    public sealed class SettingsService : ISettingsService
    {
        private IPreferences _pref;
        private const double voltScale = 1.0 / 1000.0; // mV to V, used as multiplier
        private const double tmpScale = 1.0 / 256.0;  //8.8 format 2^8 = 256, used as multiplier

        public SettingsService()
        {
            _pref = Preferences.Default;
        }

        public SettingsService(IPreferences pref)
        {
            _pref = pref;
        }

        public double VoltScale
        {
            get => _pref.Get(nameof(VoltScale), voltScale);
            set => _pref.Set(nameof(VoltScale), value);
        }

        public double TmpScale
        {
            get => _pref.Get(nameof(TmpScale), tmpScale);
            set => _pref.Set(nameof(TmpScale), value);
        }

    }
}
