using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BleTempMonitor.Helpers
{
    public class PermissionHelper
    {
        public async Task<bool> HasCorrectPermissions()
        {
            Msg.DebugMessage("Verifying Bluetooth permissions..");
            var permissionResult = await Permissions.CheckStatusAsync<Permissions.Bluetooth>();
            if (permissionResult != PermissionStatus.Granted)
            {
                permissionResult = await Permissions.RequestAsync<Permissions.Bluetooth>();
            }

            Msg.DebugMessage($"Result of requesting Bluetooth permissions: '{permissionResult}'");
            if (permissionResult != PermissionStatus.Granted)
            {
                Msg.DebugMessage("Permissions not available, direct user to settings screen.");
                Msg.ShowMessage("Permission denied. Not scanning.");
                AppInfo.ShowSettingsUI();
                return false;
            }

            permissionResult = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();
            if (permissionResult != PermissionStatus.Granted)
            {
                permissionResult = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
                if (permissionResult != PermissionStatus.Granted)
                {
                    Msg.DebugMessage("Location Persmission not granted");
                    Msg.ShowMessage("Without Location Permission we will not find ESP32 in scan");
                    AppInfo.ShowSettingsUI();
                    return false;
                }
            }
            return true;
        }
    }
}
