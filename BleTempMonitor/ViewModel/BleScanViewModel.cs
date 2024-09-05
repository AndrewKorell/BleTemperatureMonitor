using BleTempMonitor.Models;
using BleTempMonitor.Services;
using BleTempMonitor.Extensions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls;
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
using BleTempMonitor.Views;
using BleTempMonitor.Helpers;
using BleTempMonitor.Resources;
using System.Collections.Specialized;

namespace BleTempMonitor.ViewModel
{
    public partial class BleScanViewModel : BaseViewModel
    {
        private readonly IBluetoothLE? _ble;
        protected IAdapter Adapter;

        private readonly IPermissionHelper prm = new PermissionHelper();
        private readonly IBleHelpers _bleHelper = new BleHelper();

        private CancellationTokenSource? _scanCancellationTokenSource = null;

//        public ObservableCollection<BleDeviceViewModel> BleDevices { get; private set; }
        public ObservableCollection<SensorViewModel> Sensors { get; private set; }

        public BleScanViewModel()
        {
            Title = AppResources.SensorPageTitle;
            Sensors = [];

            _ble = CrossBluetoothLE.Current;

            if (_ble == null || _ble?.Adapter == null)
            {
                App.AlertSvc.ShowAlert(AppResources.BLEScannerName, AppResources.BLEInitializationFailed);
            }
            else
            {
                Adapter = _ble.Adapter;
                ConfigureBLE();
            }
        }

        private void ConfigureBLE()
        {
            _ble.StateChanged += OnBluetoothStateChanged;
            Adapter.ScanMatchMode = ScanMatchMode.STICKY;
            Adapter.ScanTimeout = 10000;
            Adapter.DeviceAdvertised += OnDeviceAdvertised;
        }

        private void OnDeviceAdvertised(object? sender, DeviceEventArgs e)
        {
            App.Logger.AddMessage("OnDeviceAdvertised Start");
            AddOrUpdateSensor(e.Device);
            App.Logger.AddMessage("OnDeviceAdvertised Stop");
        }

        private void OnBluetoothStateChanged(object? sender, BluetoothStateChangedArgs e)
        {
            //TODO:This needs to be implemented
        }

        #region ObservableProperties

        [ObservableProperty]
        bool isScanning;

        [ObservableProperty]
        bool isRefreshing;

        [ObservableProperty]
        DateTime lastUpdate;

        #endregion

        #region RelayCommands

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
                await ShowAlert(AppResources.BLEUpdateFailed);
            }
            finally
            {
                IsLoading = false;
                IsRefreshing = false;
            }
        }

        [RelayCommand]  //The decorator adds Command on the generated code page 
        async Task GetSensorDetails(Guid id)
        {
            //if (id == null) return;
            try
            {
                await Shell.Current.GoToAsync($"{nameof(SensorDetailsPage)}?Id={id}", true);
            }
            catch (Exception ex)
            {
                App.Logger.AddMessage($"Failed to launch details page {ex.Message}");
            }
        }

        [RelayCommand]
        async Task ViewSensorLog()
        {
            try
            {
                await Shell.Current.GoToAsync($"{nameof(SensorLogViewPage)}", true);
            }
            catch (Exception ex)
            {
                App.Logger.AddMessage($"Failed to launch details page {ex.Message}");
            }
        }

        #endregion

        private async void ScanForDevicesAsync()
        {
            if (IsScanning || _ble == null) return;

            foreach(var svm in Sensors)
            {
                svm.IsUpdated = false;
            }

            IsScanning = true;

            if (!_ble.IsOn)
            {
                App.AlertSvc.ShowAlert(AppResources.BLEScannerName, AppResources.BLEOffText);
                IsScanning = false;
                return;
            }

            if (!await prm.HasCorrectPermissions())
            {
                App.Logger.AddMessage(AppResources.BLEIncorrectPermission);
                IsScanning = false;
                return;
            }
            //_msg.DebugMessage("StartScanForDevices called");
            //BleDevices.Clear();
            //await UpdateConnectedDevices();

            _scanCancellationTokenSource = new();

            App.Logger.AddMessage("call Adapter.StartScanningForDevicesAsync");
            await Adapter.StartScanningForDevicesAsync(_scanCancellationTokenSource.Token);
            App.Logger.AddMessage("back from Adapter.StartScanningForDevicesAsync");

            // Scanning stopped (for whichever reason) -> cleanup
            _scanCancellationTokenSource.Dispose();
            _scanCancellationTokenSource = null;
            IsScanning = false;
        }

        private void AddOrUpdateSensor(IDevice device)
        {
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                try
                {
                    //AddOrUpdateDevice(device);
                    var m = new AdverstisementModel();

                    foreach (var ad in device.AdvertisementRecords)
                    {
                        _bleHelper.ParseAdvertisementReord(ad, ref m);
                        App.Logger.AddMessage(ad.ToString());
                    }

                    if (m.ServiceData != null && m.ServiceData[0] == 0xAA)
                    {
                        LastUpdate = DateTime.Now; // Update Time Displayed 

                        // We use the device ID as a link between collections
                        var model = await App.SensorStorage.AddOrUpdate(device.Id, string.Empty);

                        var s = Sensors.FirstOrDefault(d => d.Id == device.Id);
                        if (s != null && s.IsUpdated == false)
                        {
                            s.Update(device, m, model.Alias);
                            await App.SensorStorage.InsertLogData(model.ID, s.Voltage, s.Temperature);
                        }
                        else if (s == null)
                        {
                            s = new SensorViewModel(device, m, model.Alias);
                            Sensors.Add(s);
                            await App.SensorStorage.InsertLogData(model.ID, s.Voltage, s.Temperature);
                        }
                    }
                }
                catch (Exception ex)
                {
                    App.Logger.AddMessage($"Error processing advertising data {ex.Message}");
                }
            });
        }

        //private void AddOrUpdateDevice(IDevice device)
        //{
        //    MainThread.BeginInvokeOnMainThread(() =>
        //    {
        //        var vm = BleDevices.FirstOrDefault(d => d.DeviceId == device.Id);
        //        if (vm != null)
        //        {
        //            vm.Update(device);
        //        }
        //        else
        //        {
        //            vm = new BleDeviceViewModel(device);
        //            BleDevices.Add(vm);
        //        }
        //    });
        //}

    }
}
