# BLE Temperature Monitor 

Read temperature from two ESP32-S configured to send temperature through advertisers 

# Screen Shots 

![Screenshot (Jul 23, 2024 10_25_32 PM)](https://github.com/user-attachments/assets/4033e499-4a56-45bf-86e5-3817b6024a95)

![Screenshot (Jul 23, 2024 10_26_31 PM)](https://github.com/user-attachments/assets/58111477-66a5-4931-bc44-f94603663c32)

![Screenshot (Jul 23, 2024 10_27_09 PM)](https://github.com/user-attachments/assets/9efd4205-3cdc-494d-9fff-191cd57c72e3)

![Screenshot (Jul 23, 2024 10_31_43 PM)](https://github.com/user-attachments/assets/78816a3b-a007-4374-a892-886d8500b299)

# Swimlane 
 
![BleTempMonitorActivity drawio](https://github.com/user-attachments/assets/1bb67647-aff7-4585-ac7c-845b152fb08c)



# The Data 

I get a nice print out in the debugger 

    [0:] OnDeviceAdvertised
    [0:] Adv rec [Type CompleteLocalName; Data 45-53-50-33-32-20-54-4C-4D-20-42-65-61-63-6F-6E]
    [0:] Completed Name = ESP32 TLM Beacon
    [0:] Adv rec [Type ServiceData; Data AA-FE-20-00-12-34-78-00-00-00-00-00-00-00-00-00]
    [0:] OnDeviceAdvertised None

I created a debug version of the EddyStone program to help things along. 

    EddystoneTLM.setVolt((uint16_t)0x1234);  // 3300mV = 3.3V
    EddystoneTLM.setTemp(0x5678);  // 3000 = 30.00 ˚C
    oScanResponseData.setServiceData(BLEUUID((uint16_t)0xFEAA), String(EddystoneTLM.getData().c_str(), EddystoneTLM.getData().length()));

This kind of fits what we have in the document except it appears the temperature is only 1 byte which is fine and normal but a descrepency all the same. 

Byte offset 	Field 	Description
0 	Frame Type 	Value = 0x20
1 	Version 	TLM version, value = 0x00
2 	VBATT[0] 	Battery voltage, 1 mV/bit
3 	VBATT[1] 	
4 	TEMP[0] 	Beacon temperature
5 	TEMP[1] 	
6 	ADV_CNT[0] 	Advertising PDU count
7 	ADV_CNT[1] 	
8 	ADV_CNT[2] 	
9 	ADV_CNT[3] 	
10 	SEC_CNT[0] 	Time since power-on or reboot
11 	SEC_CNT[1] 	
12 	SEC_CNT[2] 	
13 	SEC_CNT[3] 	



https://github.com/google/eddystone/blob/master/eddystone-tlm/tlm-plain.md


# with BLE.Client.Maui code we were scanning but not finding ESP32 device 

It turns out this type of device requires location permissions. Which means the addition of an in-app request for Course Location while in-App. 

Add the following where we check permissions before running the scan. 

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


# References 

https://novelbits.io/bluetooth-low-energy-advertisements-part-1/

https://developer.android.com/develop/sensors-and-location/location/permissions

http://www.2createawebsite.com/build/hex-colors.html

## Converting the Temparature 

https://www.hugi.scene.org/online/coding/hugi%2015%20-%20cmtadfix.htm

## Continuous Development

https://stackoverflow.com/questions/74977355/how-to-configure-gitlab-ci-cd-for-net-7-maui-android

https://github.com/IntarBV/dotnet-maui-android/blob/master/README.md

https://docs.github.com/en/actions/using-jobs/running-jobs-in-a-container

https://thewissen.io/making-maui-cd-pipeline/
