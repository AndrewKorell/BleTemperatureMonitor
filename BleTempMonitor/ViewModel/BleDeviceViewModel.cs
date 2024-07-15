using CommunityToolkit.Mvvm.ComponentModel;
using Plugin.BLE.Abstractions;
using Plugin.BLE.Abstractions.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BleTempMonitor.ViewModel
{
    public partial class BleDeviceViewModel : BaseViewModel
    {
        [ObservableProperty]
        Guid deviceId;

        [ObservableProperty]
        string name = "not set";

        [ObservableProperty]
        int rssi;

        [ObservableProperty]
        bool isConnectable;

        [ObservableProperty]
        DeviceState state = DeviceState.Disconnected;


        public IReadOnlyList<AdvertisementRecord> AdvertisementRecords;


        public string Adverts
        {
            get => String.Join('\n', AdvertisementRecords.Select(advert => $"{advert.Type}: 0x{Convert.ToHexString(advert.Data)}"));
        }

        public BleDeviceViewModel(IDevice device)
        {
            Update(device);
        }

        public void Update(IDevice device)
        {
            DeviceId = device.Id;
            Name = device.Name;
            Rssi = device.Rssi;
            IsConnectable = device.IsConnectable;
            AdvertisementRecords = device.AdvertisementRecords;
            State = device.State;

        }

        public override string ToString()
        {
            var advertData = new StringBuilder();
            foreach (var advert in AdvertisementRecords)
            {
                advertData.Append($"|{advert.Type}: 0x{Convert.ToHexString(advert.Data)}|");
            }

            return $"{Name}:{DeviceId}: Adverts: '{advertData}'";
        }

    }
}
