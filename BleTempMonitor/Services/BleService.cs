
using Plugin.BLE;
using Plugin.BLE.Abstractions;
using Plugin.BLE.Abstractions.Contracts;
using Plugin.BLE.Abstractions.EventArgs;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BleTempMonitor.Services
{


    public class BleService
    {

        private readonly IBluetoothLE ble;
        private readonly IAdapter adapter;
        private readonly List<IDevice> advertiserList = [];
        private readonly List<IDevice> deviceList = [];

        public BleService() 
        {


            ble = CrossBluetoothLE.Current;
            ble.StateChanged += (s, e) =>
            {
                Debug.WriteLine($"The bluetooth state changed to {e.NewState}");
            };

            adapter = CrossBluetoothLE.Current.Adapter;

            adapter.DeviceAdvertised += DeviceAdvertisedEvent; 
        }

        private void DeviceAdvertisedEvent(object? sender, DeviceEventArgs a)
        {
            advertiserList.Add(a.Device);
        }

        public BluetoothState GetState()
        {
            //todo: add some Debug stuff here 
            return ble.State;
        }

        public async Task Scan()
        {
            var scanFilterOptions = new ScanFilterOptions();
            adapter.ScanMatchMode = ScanMatchMode.AGRESSIVE;
            adapter.ScanTimeout = 60000;

            adapter.DeviceDiscovered += (s, a) => deviceList.Add(a.Device);
            await adapter.StartScanningForDevicesAsync();    
        }

    }
}
