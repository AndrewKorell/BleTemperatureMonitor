﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BleTempMonitor.Resources {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class AppResources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal AppResources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("BleTempMonitor.Resources.AppResources", typeof(AppResources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Aborting scan due to lack permissions.
        /// </summary>
        internal static string BLEIncorrectPermission {
            get {
                return ResourceManager.GetString("BLEIncorrectPermission", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Failed to initialize BLE Adaptor.
        /// </summary>
        internal static string BLEInitializationFailed {
            get {
                return ResourceManager.GetString("BLEInitializationFailed", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Bluetooth transmitter OFF.\nPlease turn ON and try again..
        /// </summary>
        internal static string BLEOffText {
            get {
                return ResourceManager.GetString("BLEOffText", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to BLE Scanner.
        /// </summary>
        internal static string BLEScannerName {
            get {
                return ResourceManager.GetString("BLEScannerName", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to BLE Failed To Update Sensors.
        /// </summary>
        internal static string BLEUpdateFailed {
            get {
                return ResourceManager.GetString("BLEUpdateFailed", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Cancel.
        /// </summary>
        internal static string Button_Cancel {
            get {
                return ResourceManager.GetString("Button_Cancel", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to OK.
        /// </summary>
        internal static string Button_OK {
            get {
                return ResourceManager.GetString("Button_OK", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Permission Alert.
        /// </summary>
        internal static string PermissionHelperAlertTitle {
            get {
                return ResourceManager.GetString("PermissionHelperAlertTitle", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Location Permission Not Granded. Required to find devices. .
        /// </summary>
        internal static string PermissionHelperLocationPermissionNotGranted {
            get {
                return ResourceManager.GetString("PermissionHelperLocationPermissionNotGranted", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Permission Denied. Scan Cancelled.
        /// </summary>
        internal static string PermissionHelperPermissionDenied {
            get {
                return ResourceManager.GetString("PermissionHelperPermissionDenied", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to TmpScale.
        /// </summary>
        internal static string PrefTmpScaleName {
            get {
                return ResourceManager.GetString("PrefTmpScaleName", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to VoltScale.
        /// </summary>
        internal static string PrefVoltScaleName {
            get {
                return ResourceManager.GetString("PrefVoltScaleName", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SensorPageTitle.
        /// </summary>
        internal static string SensorPageTitle {
            get {
                return ResourceManager.GetString("SensorPageTitle", resourceCulture);
            }
        }
    }
}
