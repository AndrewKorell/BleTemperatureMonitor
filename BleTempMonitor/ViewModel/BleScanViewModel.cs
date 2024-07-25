﻿using BleTempMonitor.Models;
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

namespace BleTempMonitor.ViewModel
{
    public partial class BleScanViewModel : BaseViewModel
    {
        private readonly PermissionHelper prm = new PermissionHelper();

        CancellationTokenSource? _scanCancellationTokenSource = null;
        private readonly IBluetoothLE _ble;
        protected IAdapter Adapter;

//        public ObservableCollection<BleDeviceViewModel> BleDevices { get; private set; }
        public ObservableCollection<SensorViewModel> Sensors { get; private set; }

        public BleScanViewModel()
        {
            Title = "Local Temperature Sensors";

//            BleDevices = [];
            Sensors = [];

            _ble = CrossBluetoothLE.Current;

            Adapter = _ble.Adapter;
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
            Adapter.ScanMatchMode = ScanMatchMode.STICKY;
            Adapter.ScanTimeout = 10000;
            Adapter.ScanTimeoutElapsed += Adapter_ScanTimeoutElapsed;
            Adapter.DeviceAdvertised += OnDeviceAdvertised;
 //           Adapter.DeviceDiscovered += Adapter_DeviceDiscovered;
        }

        //private void Adapter_DeviceDiscovered(object? sender, DeviceEventArgs e)
        //{
        //    AddOrUpdateDevice(e.Device);
        //}

        private void OnDeviceAdvertised(object? sender, DeviceEventArgs e)
        {
            Msg.DebugMessage("OnDeviceAdvertised Start");
            AddOrUpdateSensor(e.Device);
            Msg.DebugMessage("OnDeviceAdvertised Stop");
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

        [ObservableProperty]
        DateTime lastUpdate;

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

        [RelayCommand]  //The decorator adds Command on the generated code page 
        async Task GetSensorDetails(Guid id)
        {
            //if (id == null) return;
            try
            {
                await Shell.Current.GoToAsync($"{nameof(SensorDetailsPage)}?Id={id}", true);
            }
            catch(Exception ex)
            {
                Msg.DebugMessage($"Failed to launch details page {ex.Message}");
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
                Msg.DebugMessage($"Failed to launch details page {ex.Message}");
            }
        }
        private async void ScanForDevicesAsync()
        {
            if (IsScanning) return;

            foreach(var svm in Sensors)
            {
                svm.IsUpdated = false;
            }

            IsScanning = true;

            if (!_ble.IsOn)
            {
                Msg.ShowMessage("Bluetooth is not ON.\nPlease turn on Bluetooth and try again.");
                IsScanning = false;
                return;
            }

            if (!await prm.HasCorrectPermissions())
            {
                Msg.DebugMessage("Aborting scan attempt");
                IsScanning = false;
                return;
            }
            Msg.DebugMessage("StartScanForDevices called");
            //BleDevices.Clear();
            //await UpdateConnectedDevices();

            _scanCancellationTokenSource = new();

            Msg.DebugMessage("call Adapter.StartScanningForDevicesAsync");
            await Adapter.StartScanningForDevicesAsync(_scanCancellationTokenSource.Token);
            Msg.DebugMessage("back from Adapter.StartScanningForDevicesAsync");

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
                        ad.Parse(ref m);
                        Msg.DebugMessage(ad.ToString());
                    }

                    if (m.ServiceData != null && m.ServiceData[0] == 0xAA)
                    {
                        LastUpdate = DateTime.Now;
                        var model = await App.SensorStorage.AddOrUpdate(device.Id, string.Empty);

                        var s = Sensors.FirstOrDefault(d => d.Id == device.Id);
                        if(s != null && s.IsUpdated == false)
                        {
                            s.Update(device, m, model.Alias);
                            await App.SensorStorage.InsertLogData(model.ID, s.Voltage, s.Temperature);
                        }
                        else if(s == null)
                        {
                            s = new SensorViewModel(device, m, model.Alias);
                            Sensors.Add(s);
                            await App.SensorStorage.InsertLogData(model.ID, s.Voltage, s.Temperature);
                        }
                        
                    }
                }
                catch (Exception ex)
                {
                    Msg.DebugMessage($"Error processing advertising data {ex.Message}");
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
