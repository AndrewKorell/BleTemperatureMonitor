
using BleTempMonitor.Models;
using BleTempMonitor.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using Plugin.BLE.Abstractions;
using Plugin.BLE.Abstractions.Contracts;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BleTempMonitor.ViewModel
{
    public partial class SensorViewModel : BaseViewModel
    {
        public SensorViewModel(IDevice device, AdverstisementModel ad)
        {
            //write once values
            Id = device.Id;
            Name = device.Name;

            //write many values
            UpdateDeviceRecord(device);

            //write once values;
            bleID = ((int)ad.ServiceData[0] << 8 | (int)ad.ServiceData[1]);

            FrameType = ad.ServiceData[2];
            Version = ad.ServiceData[3];

            //write many values;
            UpdateAdRecord(ad);
        }

        public void Update(IDevice device, AdverstisementModel ad)
        {
            UpdateAdRecord(ad);
            UpdateDeviceRecord(device);
        }

        private void UpdateAdRecord(AdverstisementModel ad)
        {
            if (ad.ServiceData == null)
            {
                throw new ArgumentNullException(nameof(AdverstisementModel.ServiceData));
            }
            if (ad.ServiceData.Length < 16)
            {
                throw new ArgumentOutOfRangeException(nameof(AdverstisementModel.ServiceData.Length));
            }

            Voltage = (double)((int)ad.ServiceData[4] << 8 | (int)ad.ServiceData[5]) * App.Settings.VoltageScale;
            Temperature = (double)(ad.ServiceData[6] << 8 | ad.ServiceData[7]) * App.Settings.TmpScale; // << 8 | data[5];
            Count = (long)ad.ServiceData[8] << 24 | (long)ad.ServiceData[9] << 16 | (long)ad.ServiceData[10] << 8 | (long)ad.ServiceData[11];
            Time = (long)ad.ServiceData[12] << 24 | (long)ad.ServiceData[13] << 16 | (long)ad.ServiceData[14] << 8 | (long)ad.ServiceData[15];
        }
        
        private void UpdateDeviceRecord(IDevice device)
        {
            Rssi = device.Rssi;
            State = device.State;
        }

        [ObservableProperty]
        Guid id;

        [ObservableProperty]
        string name;

        [ObservableProperty]
        int rssi;

        [ObservableProperty]
        DeviceState state;

        [ObservableProperty]
        int bleID;

        [ObservableProperty]
        int frameType;

        [ObservableProperty]
        int version;

        [ObservableProperty]
        string sensorName = "not named";

        [ObservableProperty]
        double temperature;

        [ObservableProperty]
        double voltage;

        [ObservableProperty]
        long count;

        [ObservableProperty]
        long time;
    }
}
