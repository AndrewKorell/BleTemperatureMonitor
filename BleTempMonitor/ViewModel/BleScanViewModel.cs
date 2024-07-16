using BleTempMonitor.Models;
using BleTempMonitor.Services;
using BleTempMonitor.Extensions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.ApplicationModel;
using Plugin.BLE;
using Plugin.BLE.Abstractions;
using Plugin.BLE.Abstractions.Contracts;
using Plugin.BLE.Abstractions.EventArgs;
using Plugin.BLE.Abstractions.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Runtime.InteropServices.Marshalling;

namespace BleTempMonitor.ViewModel
{
    public partial class BleScanViewModel : BaseViewModel
    {
        
        CancellationTokenSource? _scanCancellationTokenSource = null;
        private readonly IBluetoothLE _ble;
        protected IAdapter Adapter;

        public ObservableCollection<BleDeviceViewModel> BleDevices { get; private set; }
        public ObservableCollection<SensorViewModel> Sensors { get; private set; }

        public BleScanViewModel()
        {
            Title = "SCan for Sensors";

            BleDevices = [];
            Sensors = [];

            _ble = CrossBluetoothLE.Current;

            Adapter = _ble?.Adapter;
            if (_ble is null)
            {
                //ShowMessage("BluetoothManager is null");
            }
            else if (Adapter is null)
            {
                //ShowMessage("Adapter is null");
            }
            else
            {
                ConfigureBLE();
            }

        }

        private void ConfigureBLE()
        {
            _ble.StateChanged += OnBluetoothStateChanged;
            Adapter.ScanMatchMode = ScanMatchMode.AGRESSIVE;
            Adapter.ScanTimeout = 30000;
            Adapter.ScanTimeoutElapsed += Adapter_ScanTimeoutElapsed;
            Adapter.DeviceAdvertised += OnDeviceAdvertised;
            Adapter.DeviceDiscovered += Adapter_DeviceDiscovered;
        }

        private void Adapter_DeviceDiscovered(object? sender, DeviceEventArgs e)
        {
            AddOrUpdateDevice(e.Device);
        }

        private void OnDeviceAdvertised(object? sender, DeviceEventArgs e)
        {
            DebugMessage("OnDeviceAdvertised Start");
            AddOrUpdateSensor(e.Device);
            DebugMessage("OnDeviceAdvertised Stop");
        }

        private void Adapter_ScanTimeoutElapsed(object? sender, EventArgs e)
        {
            //throw new NotImplementedException();
        }

        private void OnBluetoothStateChanged(object? sender, BluetoothStateChangedArgs e)
        {

        }

        [ObservableProperty]
        bool isScanning;

        [ObservableProperty]
        bool isRefreshing;


        [RelayCommand]
        async Task UpdateSensors()
        {
            if (IsLoading) return;
            try
            {
                IsLoading = true;
                ScanForDevicesAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Unable to get sensor list {ex.Message}");
                await ShowAlert("Failed to Update Sensors");
            }
            finally
            {
                IsLoading = false;
                IsRefreshing = false;
            }
        }


        private async void ScanForDevicesAsync()
        {
            if (IsScanning) return;

            IsScanning = true;

            if (!_ble.IsOn)
            {
                ShowMessage("Bluetooth is not ON.\nPlease turn on Bluetooth and try again.");
                IsScanning = false;
                return;
            }
            if (!await HasCorrectPermissions())
            {
                DebugMessage("Aborting scan attempt");
                IsScanning = false;
                return;
            }
            DebugMessage("StartScanForDevices called");
            BleDevices.Clear();
            //await UpdateConnectedDevices();

            _scanCancellationTokenSource = new();

            var filter = new ScanFilterOptions() {
                DeviceAddresses = new[] {"4B:A9:D0:33:BD:08", "90:00:00:00:05:1D"}
            };
            DebugMessage("call Adapter.StartScanningForDevicesAsync");
            await Adapter.StartScanningForDevicesAsync(_scanCancellationTokenSource.Token);
            DebugMessage("back from Adapter.StartScanningForDevicesAsync");

            // Scanning stopped (for whichever reason) -> cleanup
            _scanCancellationTokenSource.Dispose();
            _scanCancellationTokenSource = null;
            IsScanning = false;
        }


        private async Task<bool> HasCorrectPermissions()
        {
            DebugMessage("Verifying Bluetooth permissions..");
            var permissionResult = await Permissions.CheckStatusAsync<Permissions.Bluetooth>();
            if (permissionResult != PermissionStatus.Granted)
            {
                permissionResult = await Permissions.RequestAsync<Permissions.Bluetooth>();
            }

            DebugMessage($"Result of requesting Bluetooth permissions: '{permissionResult}'");
            if (permissionResult != PermissionStatus.Granted)
            {
                DebugMessage("Permissions not available, direct user to settings screen.");
                ShowMessage("Permission denied. Not scanning.");
                AppInfo.ShowSettingsUI();
                return false;
            }
                
                
            permissionResult = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();
            if (permissionResult != PermissionStatus.Granted)
            {
                permissionResult = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
                if (permissionResult != PermissionStatus.Granted)
                {
                    DebugMessage("Location Persmission not granted");
                    ShowMessage("Without Location Permission we will not find ESP32 in scan");
                    AppInfo.ShowSettingsUI();
                    return false;
                }
            }
            return true;
        }

        private void AddOrUpdateSensor(IDevice device)
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                try
                {
                    AddOrUpdateDevice(device);
                    var m = new AdverstisementModel();
                    foreach (var ad in device.AdvertisementRecords)
                    {
                        ad.Parse(ref m);
                        DebugMessage(ad.ToString());
                    }

                    if (m.ServiceData != null && m.ServiceData[0] == 0xAA)
                    {
                        var s = Sensors.FirstOrDefault(d => d.Id == device.Id);
                        if(s != null)
                        {
                            s.Update(device, m);
                        }
                        else
                        {
                            var sensor = new SensorViewModel(device, m);
                            Sensors.Add(sensor);
                        }

                    }
                }
                catch (Exception ex)
                {
                    DebugMessage($"Error processing advertising data {ex.Message}");
                }
            });
        }
        private void AddOrUpdateDevice(IDevice device)
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                var vm = BleDevices.FirstOrDefault(d => d.DeviceId == device.Id);
                if (vm != null)
                {
                    vm.Update(device);
                }
                else
                {
                    vm = new BleDeviceViewModel(device);
                    BleDevices.Add(vm);
                }
            });
        }

    }
}
