using BleTempMonitor.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using static Android.Icu.Text.CaseMap;

namespace BleTempMonitor.Helpers
{
    public interface IPermissionHelper
    {
        Task<bool> HasCorrectPermissions();
    }

    public class PermissionHelper : IPermissionHelper
    {
        public async Task<bool> HasCorrectPermissions()
        {
            App.Logger.AddMessage("Verifying Bluetooth permissions..");
            var permissionResult = await Permissions.CheckStatusAsync<Permissions.Bluetooth>();
            if (permissionResult != PermissionStatus.Granted)
            {
                permissionResult = await Permissions.RequestAsync<Permissions.Bluetooth>();
            }

            App.Logger.AddMessage($"Result of requesting Bluetooth permissions: '{permissionResult}'");
  
            if (permissionResult != PermissionStatus.Granted)
            {
                App.Logger.AddMessage("Permissions not available, direct user to settings screen.");
                await App.AlertSvc.ShowAlertAsync(AppResources.PermissionHelperAlertTitle,
                    AppResources.PermissionHelperPermissionDenied,
                    AppResources.Button_OK);
                AppInfo.ShowSettingsUI();
                return false;
            }

            permissionResult = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();
            if (permissionResult != PermissionStatus.Granted)
            {
                permissionResult = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
                if (permissionResult != PermissionStatus.Granted)
                {
                    App.Logger.AddMessage("Location Persmission not granted");
                    await App.AlertSvc.ShowAlertAsync(AppResources.PermissionHelperAlertTitle,
                        AppResources.PermissionHelperLocationPermissionNotGranted, 
                        AppResources.Button_OK);
                    AppInfo.ShowSettingsUI();
                    return false;
                }
            }
            return true;
        }
    }
}
