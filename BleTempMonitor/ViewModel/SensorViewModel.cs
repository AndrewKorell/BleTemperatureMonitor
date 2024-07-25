
using BleTempMonitor.Models;
using BleTempMonitor.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using Plugin.BLE.Abstractions;
using Plugin.BLE.Abstractions.Contracts;


namespace BleTempMonitor.ViewModel
{
    public partial class SensorViewModel : BaseViewModel
    {
        public SensorViewModel(IDevice device, AdverstisementModel ad, string alias)
        {
            //write once values
            Alias = alias;
            Id = device.Id;
            Name = device.Name;
            IsUpdated = true;
            //write many values
            UpdateDeviceRecord(device);

            //write once values;
            bleID = ((int)ad.ServiceData[0] << 8 | (int)ad.ServiceData[1]);

            FrameType = ad.ServiceData[2];
            Version = ad.ServiceData[3];

            //write many values;
            UpdateAdRecord(ad);
        }

        public void Update(IDevice device, AdverstisementModel ad, string alias)
        {
            Alias = alias;
            IsUpdated = true;
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

            VoltageRaw = (ushort)(ad.ServiceData[4] << 8 | (ushort)ad.ServiceData[5]);
            Voltage = (double)((ushort)ad.ServiceData[4] << 8 | (ushort)ad.ServiceData[5]) * App.Settings.VoltageScale;

            TempRaw = (short)((ushort) ad.ServiceData[6] << 8 | (ushort)ad.ServiceData[7]);
            Temperature = (double)(TempRaw) * App.Settings.TmpScale; // << 8 | data[5]; //We Swap the endienness of this as well. 

            Count = (long)ad.ServiceData[8] << 24 | (long)ad.ServiceData[9] << 16 | (long)ad.ServiceData[10] << 8 | (long)ad.ServiceData[11];
            Time = (long)ad.ServiceData[12] << 24 | (long)ad.ServiceData[13] << 16 | (long)ad.ServiceData[14] << 8 | (long)ad.ServiceData[15];
        }
        
        private void UpdateDeviceRecord(IDevice device)
        {
            Rssi = device.Rssi;
            State = device.State;
        }

        [ObservableProperty]
        bool isUpdated;

        [ObservableProperty]
        string alias = "not set";

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
        short tempRaw;

        [ObservableProperty]
        double voltage;

        [ObservableProperty]
        ushort voltageRaw;

        [ObservableProperty]
        long count;

        [ObservableProperty]
        long time;
    }
}
