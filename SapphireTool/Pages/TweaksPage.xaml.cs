using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;
using System.Xml.Linq;
using SapphireTool.Classes;
using System.Diagnostics;
using static System.Net.WebRequestMethods;
using System.Windows.Input;

namespace SapphireTool.Pages
{
    public partial class TweaksPage : Page
    {
        private void CheckTweakState()
        {
            tsBluetooth.IsChecked = RegistryTools.CheckTweakState(SapphireTool, "DisableBluetooth", 1);
            tsFSOGamebar.IsChecked = RegistryTools.CheckTweakState(SapphireTool, "DisableFSO", 1);
            tsPrefetch.IsChecked = RegistryTools.CheckTweakState(SapphireTool, "DisablePrefetch", 1);
            tsHyperV.IsChecked = RegistryTools.CheckTweakState(SapphireTool, "DisableHyperV", 1);
            tsPrintSpooler.IsChecked = RegistryTools.CheckTweakState(SapphireTool, "DisablePrinter", 1);
            tsVPN.IsChecked = RegistryTools.CheckTweakState(SapphireTool, "DisableVPN", 1);
            tsActionCenter.IsChecked = RegistryTools.CheckTweakState(SapphireTool, "ActionCenter", 1);
            tsClipboard.IsChecked = RegistryTools.CheckTweakState(SapphireTool, "EnableClipboardSvc", 1);
            tsUACAdmin.IsChecked = RegistryTools.CheckTweakState(SapphireTool, "EnableAdminUAC", 1);
            tsMPO.IsChecked = RegistryTools.CheckTweakState(SapphireTool, "DisableMPO", 1);
            tsIntelTSX.IsChecked = RegistryTools.CheckTweakState(SapphireTool, "EnableTSX", 1);
            tsDEP.IsChecked = RegistryTools.CheckTweakState(SapphireTool, "DisableNX", 1);
            tsNTFSEncryption.IsChecked = RegistryTools.CheckTweakState(SapphireTool, "DisableNTFSEncryption", 1);
            tsNotifications.IsChecked = RegistryTools.CheckTweakState(SapphireTool, "DisableNotifications", 1);
            tsNoLazyMode.IsChecked = RegistryTools.CheckTweakState(SapphireTool, "NoLazyMode", 1);
            tsLargeSystemCache.IsChecked = RegistryTools.CheckTweakState(SapphireTool, "EnableLargeSystemCache", 1);
            tsStartMenu.IsChecked = RegistryTools.CheckTweakState(SapphireTool, "DisableStartmenu", 1);
            tsWallpaperQuality.IsChecked = RegistryTools.CheckTweakState(SapphireTool, "WallpaperQuality", 1);
            tsTransparency.IsChecked = RegistryTools.CheckTweakState(SapphireTool, "TransparencyEffects", 1);
            tsVBS.IsChecked = RegistryTools.CheckTweakState(SapphireTool, "VBS", 1);
            tsDCOM.IsChecked = RegistryTools.CheckTweakState(SapphireTool, "DisableDCOM", 1);
            tsAnimations.IsChecked = RegistryTools.CheckTweakState(SapphireTool, "DisableAnimations", 1);
            tsSystemProfile.IsChecked = RegistryTools.CheckTweakState(SapphireTool, "SystemProfile", 1);
            tsUAC.IsChecked = RegistryTools.CheckTweakState(SapphireTool, "EnableUAC", 1);
            tsNVMETweaks.IsChecked = RegistryTools.CheckTweakState(SapphireTool, "NVMETweaks", 1);
            tsBootMenu.IsChecked = RegistryTools.CheckTweakState(SapphireTool, "BootMenuPolicy", 1);
            tsLockScreen.IsChecked = RegistryTools.CheckTweakState(SapphireTool, "DisableLockScreen", 1);
            tsCDROM.IsChecked = RegistryTools.CheckTweakState(SapphireTool, "EnableCDROM", 1);
            tsVR.IsChecked = RegistryTools.CheckTweakState(SapphireTool, "EnableVR", 1);
            tsWiFi.IsChecked = RegistryTools.CheckTweakState(SapphireTool, "DisableWiFi", 1);
        }
        public TweaksPage()
        {
            InitializeComponent();
            CheckTweakState();
        }
        string OSName = Utils.GetOS();
        public static RegistryKey SapphireTool = Registry.CurrentUser.CreateSubKey(@"Software\SapphireTool", RegistryKeyPermissionCheck.ReadWriteSubTree);

        private void tsWiFi_Click(object sender, RoutedEventArgs e)
        {
            if (tsWiFi.IsChecked == true)
            {
                Registry.SetValue("HKEY_LOCAL_MACHINE\\SYSTEM\\CurrentControlSet\\Services\\WlanSvc", "Start", 4, RegistryValueKind.DWord);
                Registry.SetValue("HKEY_LOCAL_MACHINE\\SYSTEM\\CurrentControlSet\\Services\\vwififlt", "Start", 4, RegistryValueKind.DWord);
                Registry.SetValue("HKEY_LOCAL_MACHINE\\SYSTEM\\CurrentControlSet\\Services\\netprofm", "Start", 4, RegistryValueKind.DWord);
                Registry.SetValue("HKEY_LOCAL_MACHINE\\SYSTEM\\CurrentControlSet\\Services\\NlaSvc  ", "Start", 4, RegistryValueKind.DWord);
                if (OSName.Contains("Windows 11"))
                {
                    Registry.SetValue("HKEY_LOCAL_MACHINE\\SYSTEM\\CurrentControlSet\\Services\\netprofm", "Start", 3, RegistryValueKind.DWord);
                }
                SapphireTool.SetValue("DisableWiFi", 1);
            }
            else
            {
                Registry.SetValue("HKEY_LOCAL_MACHINE\\SYSTEM\\CurrentControlSet\\Services\\WlanSvc", "Start", 2, RegistryValueKind.DWord);
                Registry.SetValue("HKEY_LOCAL_MACHINE\\SYSTEM\\CurrentControlSet\\Services\\vwififlt", "Start", 1, RegistryValueKind.DWord);
                Registry.SetValue("HKEY_LOCAL_MACHINE\\SYSTEM\\CurrentControlSet\\Services\\netprofm", "Start", 3, RegistryValueKind.DWord);
                Registry.SetValue("HKEY_LOCAL_MACHINE\\SYSTEM\\CurrentControlSet\\Services\\NlaSvc  ", "Start", 2, RegistryValueKind.DWord);
                SapphireTool.DeleteValue("DisableWiFi");
            }
        }

        private void tsIntelTSX_Click(object sender, RoutedEventArgs e)
        {
            if (tsIntelTSX.IsChecked == true)
            {
                Registry.SetValue("HKEY_LOCAL_MACHINE\\SYSTEM\\ControlSet001\\Control\\Session Manager\\kernel", "DisableTsx", 0, RegistryValueKind.DWord);
                SapphireTool.SetValue("EnableTSX", 1);
            }
            else
            {
                Registry.SetValue("HKEY_LOCAL_MACHINE\\SYSTEM\\ControlSet001\\Control\\Session Manager\\kernel", "DisableTsx", 1, RegistryValueKind.DWord);
                SapphireTool.DeleteValue("EnableTSX");
            }
        }

        private void tsActionCenter_Click(object sender, RoutedEventArgs e)
        {
            if (tsActionCenter.IsChecked == true)
            {
                Registry.SetValue("HKEY_CURRENT_USER\\Software\\Policies\\Microsoft\\Windows\\Explorer", "DisableNotificationCenter", 1, RegistryValueKind.DWord);
                Utils.RestartExplorer();
                SapphireTool.SetValue("ActionCenter", 1);
            }
            else
            {
                Registry.SetValue("HKEY_CURRENT_USER\\Software\\Policies\\Microsoft\\Windows\\Explorer", "DisableNotificationCenter", 0, RegistryValueKind.DWord);
                Utils.RestartExplorer();
                SapphireTool.DeleteValue("ActionCenter");
            }
        }

        private void tsDEP_Click(object sender, RoutedEventArgs e)
        {
            if (tsDEP.IsChecked == true)
            {
                Utils.RunCommand("bcdedit", "/set NX AlwaysOff");
                SapphireTool.SetValue("DisableNX", 1);
            }
            else
            {
                Utils.RunCommand("bcdedit", "/set NX OptIn");
                SapphireTool.DeleteValue("DisableNX");
            }
        }

        private void tsClipboard_Click(object sender, RoutedEventArgs e)
        {
            if (tsClipboard.IsChecked == true)
            {
                Registry.SetValue("HKEY_LOCAL_MACHINE\\SYSTEM\\CurrentControlSet\\Services\\cbdhsvc", "Start", 2, RegistryValueKind.DWord);
                SapphireTool.SetValue("EnableClipboardSvc", 1);
                using (RegistryKey servicesKey = Registry.LocalMachine.OpenSubKey("SYSTEM\\CurrentControlSet\\Services", true))
                {
                    if (servicesKey != null)
                    {
                        foreach (string subkeyName in servicesKey.GetSubKeyNames())
                        {
                            if (subkeyName.Contains("cbdhsvc"))
                            {
                                using (RegistryKey subkey = servicesKey.OpenSubKey(subkeyName, true))
                                {
                                    subkey?.SetValue("Start", 2, RegistryValueKind.DWord);
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                Registry.SetValue("HKEY_LOCAL_MACHINE\\SYSTEM\\CurrentControlSet\\Services\\cbdhsvc", "Start", 4, RegistryValueKind.DWord);
                using (RegistryKey servicesKey = Registry.LocalMachine.OpenSubKey("SYSTEM\\CurrentControlSet\\Services", true))
                {
                    if (servicesKey != null)
                    {
                        foreach (string subkeyName in servicesKey.GetSubKeyNames())
                        {
                            if (subkeyName.Contains("cbdhsvc"))
                            {
                                using (RegistryKey subkey = servicesKey.OpenSubKey(subkeyName, true))
                                {
                                    subkey?.SetValue("Start", 4, RegistryValueKind.DWord);
                                }
                            }
                        }
                    }
                }
                SapphireTool.SetValue("EnableClipboardSvc", 1);
            }
        }

        private void tsBluetooth_Click(object sender, RoutedEventArgs e)
        {
            if (tsBluetooth.IsChecked == true)
            {
                Registry.SetValue("HKEY_LOCAL_MACHINE\\SYSTEM\\CurrentControlSet\\Services\\BthA4dp", "Start", 4, RegistryValueKind.DWord);
                Registry.SetValue("HKEY_LOCAL_MACHINE\\SYSTEM\\CurrentControlSet\\Services\\BthEnum", "Start", 4, RegistryValueKind.DWord);
                Registry.SetValue("HKEY_LOCAL_MACHINE\\SYSTEM\\CurrentControlSet\\Services\\BthHFEnum", "Start", 4, RegistryValueKind.DWord);
                Registry.SetValue("HKEY_LOCAL_MACHINE\\SYSTEM\\CurrentControlSet\\Services\\BthLEEnum", "Start", 4, RegistryValueKind.DWord);
                Registry.SetValue("HKEY_LOCAL_MACHINE\\SYSTEM\\CurrentControlSet\\Services\\BTHMODEM", "Start", 4, RegistryValueKind.DWord);
                Registry.SetValue("HKEY_LOCAL_MACHINE\\SYSTEM\\CurrentControlSet\\Services\\Microsoft_Bluetooth_AvrcpTransport", "Start", 4, RegistryValueKind.DWord);
                Registry.SetValue("HKEY_LOCAL_MACHINE\\SYSTEM\\CurrentControlSet\\Services\\BluetoothUserService", "Start", 4, RegistryValueKind.DWord);
                Registry.SetValue("HKEY_LOCAL_MACHINE\\SYSTEM\\CurrentControlSet\\Services\\BthAvctpSvc", "Start", 4, RegistryValueKind.DWord);
                Registry.SetValue("HKEY_LOCAL_MACHINE\\SYSTEM\\CurrentControlSet\\Services\\RFCOMM", "Start", 4, RegistryValueKind.DWord);
                Registry.SetValue("HKEY_LOCAL_MACHINE\\SYSTEM\\CurrentControlSet\\Services\\bthserv", "Start", 4, RegistryValueKind.DWord);
                Registry.SetValue("HKEY_LOCAL_MACHINE\\SYSTEM\\CurrentControlSet\\Services\\BTAGService", "Start", 4, RegistryValueKind.DWord);
                Registry.SetValue("HKEY_LOCAL_MACHINE\\SYSTEM\\CurrentControlSet\\Services\\BTHUSB", "Start", 4, RegistryValueKind.DWord);
                Registry.SetValue("HKEY_LOCAL_MACHINE\\SYSTEM\\CurrentControlSet\\Services\\BTHPORT", "Start", 4, RegistryValueKind.DWord);
                Registry.SetValue("HKEY_LOCAL_MACHINE\\SYSTEM\\CurrentControlSet\\Services\\BthMini", "Start", 4, RegistryValueKind.DWord);
                Registry.SetValue("HKEY_LOCAL_MACHINE\\SYSTEM\\CurrentControlSet\\Services\\HidBth", "Start", 4, RegistryValueKind.DWord);
                Utils.RunCommand("C:\\PostInstall\\Tweaks\\DevManView.exe", "/disable \"Microsoft Radio Device Enumeration Bus");
                SapphireTool.SetValue("DisableBluetooth", 1);
            }
            else
            {
                Registry.SetValue("HKEY_LOCAL_MACHINE\\SYSTEM\\CurrentControlSet\\Services\\BthA3dp", "Start", 3, RegistryValueKind.DWord);
                Registry.SetValue("HKEY_LOCAL_MACHINE\\SYSTEM\\CurrentControlSet\\Services\\BthEnum", "Start", 3, RegistryValueKind.DWord);
                Registry.SetValue("HKEY_LOCAL_MACHINE\\SYSTEM\\CurrentControlSet\\Services\\BthHFEnum", "Start", 3, RegistryValueKind.DWord);
                Registry.SetValue("HKEY_LOCAL_MACHINE\\SYSTEM\\CurrentControlSet\\Services\\BthLEEnum", "Start", 3, RegistryValueKind.DWord);
                Registry.SetValue("HKEY_LOCAL_MACHINE\\SYSTEM\\CurrentControlSet\\Services\\BTHMODEM", "Start", 3, RegistryValueKind.DWord);
                Registry.SetValue("HKEY_LOCAL_MACHINE\\SYSTEM\\CurrentControlSet\\Services\\Microsoft_Bluetooth_AvrcpTransport", "Start", 3, RegistryValueKind.DWord);
                Registry.SetValue("HKEY_LOCAL_MACHINE\\SYSTEM\\CurrentControlSet\\Services\\BluetoothUserService", "Start", 3, RegistryValueKind.DWord);
                Registry.SetValue("HKEY_LOCAL_MACHINE\\SYSTEM\\CurrentControlSet\\Services\\BthAvctpSvc", "Start", 3, RegistryValueKind.DWord);
                Registry.SetValue("HKEY_LOCAL_MACHINE\\SYSTEM\\CurrentControlSet\\Services\\RFCOMM", "Start", 3, RegistryValueKind.DWord);
                Registry.SetValue("HKEY_LOCAL_MACHINE\\SYSTEM\\CurrentControlSet\\Services\\bthserv", "Start", 3, RegistryValueKind.DWord);
                Registry.SetValue("HKEY_LOCAL_MACHINE\\SYSTEM\\CurrentControlSet\\Services\\BTAGService", "Start", 3, RegistryValueKind.DWord);
                Registry.SetValue("HKEY_LOCAL_MACHINE\\SYSTEM\\CurrentControlSet\\Services\\BTHUSB", "Start", 3, RegistryValueKind.DWord);
                Registry.SetValue("HKEY_LOCAL_MACHINE\\SYSTEM\\CurrentControlSet\\Services\\BTHPORT", "Start", 3, RegistryValueKind.DWord);
                Registry.SetValue("HKEY_LOCAL_MACHINE\\SYSTEM\\CurrentControlSet\\Services\\BthMini", "Start", 3, RegistryValueKind.DWord);
                Registry.SetValue("HKEY_LOCAL_MACHINE\\SYSTEM\\CurrentControlSet\\Services\\HidBth", "Start", 3, RegistryValueKind.DWord);
                Utils.RunCommand("C:\\PostInstall\\Tweaks\\DevManView.exe", "/enable \"Microsoft Radio Device Enumeration Bus");
                SapphireTool.DeleteValue("DisableBluetooth");
            }
        }

        private void tsBootMenu_Click(object sender, RoutedEventArgs e)
        {
            if (tsBootMenu.IsChecked == true)
            {
                Utils.RunCommand("bcdedit.exe", "/set bootmenupolicy Standard");
                SapphireTool.SetValue("BootMenuPolicy", 1);
            }
            else
            {
                Utils.RunCommand("bcdedit.exe", "/set bootmenupolicy legacy");
                SapphireTool.DeleteValue("BootMenuPolicy");
            }
        }

        private void tsVPN_Click(object sender, RoutedEventArgs e)
        {
            if (tsVPN.IsChecked == true)
            {
                Registry.SetValue("HKEY_LOCAL_MACHINE\\SYSTEM\\CurrentControlSet\\Services\\IKEEXT", "Start", 4, RegistryValueKind.DWord);
                Registry.SetValue("HKEY_LOCAL_MACHINE\\SYSTEM\\CurrentControlSet\\Services\\WinHttpAutoProxySvc", "Start", 4, RegistryValueKind.DWord);
                Registry.SetValue("HKEY_LOCAL_MACHINE\\SYSTEM\\CurrentControlSet\\Services\\RasMan", "Start", 4, RegistryValueKind.DWord);
                Registry.SetValue("HKEY_LOCAL_MACHINE\\SYSTEM\\CurrentControlSet\\Services\\SstpSvc", "Start", 4, RegistryValueKind.DWord);
                Registry.SetValue("HKEY_LOCAL_MACHINE\\SYSTEM\\CurrentControlSet\\Services\\iphlpsvc", "Start", 4, RegistryValueKind.DWord);
                Registry.SetValue("HKEY_LOCAL_MACHINE\\SYSTEM\\CurrentControlSet\\Services\\NdisVirtualBus", "Start", 4, RegistryValueKind.DWord);
                Registry.SetValue("HKEY_LOCAL_MACHINE\\SYSTEM\\CurrentControlSet\\Services\\Eaphost", "Start", 4, RegistryValueKind.DWord);
                SapphireTool.SetValue("DisableVPN", 1);
            }
            else
            {
                Registry.SetValue("HKEY_LOCAL_MACHINE\\SYSTEM\\CurrentControlSet\\Services\\IKEEXT", "Start", 3, RegistryValueKind.DWord);
                Registry.SetValue("HKEY_LOCAL_MACHINE\\SYSTEM\\CurrentControlSet\\Services\\BFE", "Start", 2, RegistryValueKind.DWord);
                Registry.SetValue("HKEY_LOCAL_MACHINE\\SYSTEM\\CurrentControlSet\\Services\\WinHttpAutoProxySvc", "Start", 3, RegistryValueKind.DWord);
                Registry.SetValue("HKEY_LOCAL_MACHINE\\SYSTEM\\CurrentControlSet\\Services\\RasMan", "Start", 3, RegistryValueKind.DWord);
                Registry.SetValue("HKEY_LOCAL_MACHINE\\SYSTEM\\CurrentControlSet\\Services\\SstpSvc", "Start", 3, RegistryValueKind.DWord);
                Registry.SetValue("HKEY_LOCAL_MACHINE\\SYSTEM\\CurrentControlSet\\Services\\iphlpsvc", "Start", 3, RegistryValueKind.DWord);
                Registry.SetValue("HKEY_LOCAL_MACHINE\\SYSTEM\\CurrentControlSet\\Services\\NdisVirtualBus", "Start", 3, RegistryValueKind.DWord);
                Registry.SetValue("HKEY_LOCAL_MACHINE\\SYSTEM\\CurrentControlSet\\Services\\Eaphost", "Start", 3, RegistryValueKind.DWord);
                SapphireTool.DeleteValue("DisableVPN");
            }
        }

        private void tsNTFSEncryption_Click(object sender, RoutedEventArgs e)
        {
            if (tsNTFSEncryption.IsChecked == true)
            {
                Utils.RunCommand("fsutil", "behavior set disableencryption 1");
                Registry.SetValue("HKEY_LOCAL_MACHINE\\SYSTEM\\CurrentControlSet\\Policies", "NtfsDisableEncryption", 1, RegistryValueKind.DWord);
                SapphireTool.SetValue("DisableNTFSEncryption", 1);
            }
            else
            {
                Utils.RunCommand("fsutil", "behavior set disableencryption 0");
                Registry.LocalMachine.OpenSubKey(@"SYSTEM\CurrentControlSet\Policies", true)?.DeleteValue("NtfsDisableEncryption", false);
                SapphireTool.DeleteValue("DisableNTFSEncryption");
            }
        }

        private void tsFSOGamebar_Click(object sender, RoutedEventArgs e)
        {
            if (tsFSOGamebar.IsChecked == true)
            {
                Registry.SetValue(@"HKEY_CURRENT_USER\Software\Microsoft\GameBar", "ShowStartupPanel", 0, RegistryValueKind.DWord);
                Registry.SetValue(@"HKEY_CURRENT_USER\Software\Microsoft\GameBar", "GamePanelStartupTipIndex", 3, RegistryValueKind.DWord);
                Registry.SetValue(@"HKEY_CURRENT_USER\Software\Microsoft\GameBar", "AllowAutoGameMode", 0, RegistryValueKind.DWord);
                Registry.SetValue(@"HKEY_CURRENT_USER\Software\Microsoft\GameBar", "AutoGameModeEnabled", 0, RegistryValueKind.DWord);
                Registry.SetValue(@"HKEY_CURRENT_USER\Software\Microsoft\GameBar", "UseNexusForGameBarEnabled", 0, RegistryValueKind.DWord);
                Registry.SetValue(@"HKEY_CURRENT_USER\System\GameConfigStore", "GameDVR_Enabled", 0, RegistryValueKind.DWord);
                Registry.SetValue(@"HKEY_CURRENT_USER\System\GameConfigStore", "GameDVR_FSEBehaviorMode", 2, RegistryValueKind.DWord);
                Registry.SetValue(@"HKEY_CURRENT_USER\System\GameConfigStore", "GameDVR_FSEBehavior", 2, RegistryValueKind.DWord);
                Registry.SetValue(@"HKEY_CURRENT_USER\System\GameConfigStore", "GameDVR_HonorUserFSEBehaviorMode", 1, RegistryValueKind.DWord);
                Registry.SetValue(@"HKEY_CURRENT_USER\System\GameConfigStore", "GameDVR_DXGIHonorFSEWindowsCompatible", 1, RegistryValueKind.DWord);
                Registry.SetValue(@"HKEY_CURRENT_USER\System\GameConfigStore", "GameDVR_EFSEFeatureFlags", 0, RegistryValueKind.DWord);
                Registry.SetValue(@"HKEY_CURRENT_USER\System\GameConfigStore", "GameDVR_DSEBehavior", 2, RegistryValueKind.DWord);
                Registry.SetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\GameDVR", "AllowGameDVR", 0, RegistryValueKind.DWord);
                Registry.SetValue(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\GameDVR", "AppCaptureEnabled", 0, RegistryValueKind.DWord);
                Registry.SetValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\BcastDVRUserService", "Start", 4, RegistryValueKind.DWord);
                Registry.SetValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Environment", "__COMPAT_LAYER", "~ DISABLEDXMAXIMIZEDWINDOWEDMODE", RegistryValueKind.String);
                SapphireTool.SetValue("DisableFSO", 1);
            }
            else
            {
                Registry.SetValue(@"HKEY_CURRENT_USER\System\GameConfigStore", "GameDVR_Enabled", 0, RegistryValueKind.DWord);
                Registry.SetValue(@"HKEY_CURRENT_USER\System\GameConfigStore", "GameDVR_FSEBehaviorMode", 0, RegistryValueKind.DWord);
                Registry.SetValue(@"HKEY_CURRENT_USER\System\GameConfigStore", "GameDVR_FSEBehavior", 0, RegistryValueKind.DWord);
                Registry.SetValue(@"HKEY_CURRENT_USER\System\GameConfigStore", "GameDVR_HonorUserFSEBehaviorMode", 1, RegistryValueKind.DWord);
                Registry.SetValue(@"HKEY_CURRENT_USER\System\GameConfigStore", "GameDVR_DXGIHonorFSEWindowsCompatible", 0, RegistryValueKind.DWord);
                Registry.SetValue(@"HKEY_CURRENT_USER\System\GameConfigStore", "GameDVR_EFSEFeatureFlags", 0, RegistryValueKind.DWord);
                Registry.SetValue(@"HKEY_CURRENT_USER\System\GameConfigStore", "GameDVR_DSEBehavior", 0, RegistryValueKind.DWord);
                SapphireTool.DeleteValue("DisableFSO");
            }
        }

        private void tsNotifications_Click(object sender, RoutedEventArgs e)
        {
            if (tsNotifications.IsChecked == true)
            {
                Registry.SetValue("HKEY_LOCAL_MACHINE\\SYSTEM\\CurrentControlSet\\Services\\WpnService", "Start", 4, RegistryValueKind.DWord);
                Registry.SetValue("HKEY_CURRENT_USER\\SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Notifications\\Settings", "NOC_GLOBAL_SETTING_ALLOW_NOTIFICATION_SOUND", 0, RegistryValueKind.DWord);
                Registry.SetValue("HKEY_CURRENT_USER\\Microsoft\\Windows\\CurrentVersion\\CapabilityAccessManager\\ConsentStore\\userNotificationListener", "Value", "Deny", RegistryValueKind.String);
                Registry.SetValue("HKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\PushNotifications", "ToastEnabled", 0, RegistryValueKind.DWord);
                Registry.SetValue("HKEY_LOCAL_MACHINE\\SOFTWARE\\Policies\\Microsoft\\Windows\\CurrentVersion\\PushNotifications", "NoCloudApplicationNotification", 1, RegistryValueKind.DWord);
                SapphireTool.SetValue("DisableNotifications", 1);
            }
            else
            {
                Registry.SetValue("HKEY_LOCAL_MACHINE\\SYSTEM\\CurrentControlSet\\Services\\WpnService", "Start", 2, RegistryValueKind.DWord);
                Registry.SetValue("HKEY_CURRENT_USER\\SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Notifications\\Settings", "NOC_GLOBAL_SETTING_ALLOW_NOTIFICATION_SOUND", 1, RegistryValueKind.DWord);
                Registry.SetValue("HKEY_CURRENT_USER\\Microsoft\\Windows\\CurrentVersion\\CapabilityAccessManager\\ConsentStore\\userNotificationListener", "Value", "Allow", RegistryValueKind.String);
                Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\PushNotifications", true)?.DeleteValue("ToastEnabled", false);
                Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Policies\Microsoft\Windows\CurrentVersion\PushNotifications", true)?.DeleteValue("NoCloudApplicationNotification", false);
                SapphireTool.DeleteValue("DisableNotifications");
            }
        }

        private void tsPrefetch_Click(object sender, RoutedEventArgs e)
        {
            if (tsPrefetch.IsChecked == true)
            {
                Registry.SetValue("HKEY_LOCAL_MACHINE\\SYSTEM\\CurrentControlSet\\Services\\SysMain", "Start", 4, RegistryValueKind.DWord);
                Registry.SetValue("HKEY_LOCAL_MACHINE\\SYSTEM\\CurrentControlSet\\Services\\FontCache", "Start", 4, RegistryValueKind.DWord);
                SapphireTool.SetValue("DisablePrefetch", 1);
            }
            else
            {
                Registry.SetValue("HKEY_LOCAL_MACHINE\\SYSTEM\\CurrentControlSet\\Services\\SysMain", "Start", 2, RegistryValueKind.DWord);
                Registry.SetValue("HKEY_LOCAL_MACHINE\\SYSTEM\\CurrentControlSet\\Services\\FontCache", "Start", 2, RegistryValueKind.DWord);
                SapphireTool.DeleteValue("DisablePrefetch");
            }
        }

        private void tsCDROM_Click(object sender, RoutedEventArgs e)
        {
            if (tsCDROM.IsChecked == true)
            {
                Registry.SetValue("HKEY_LOCAL_MACHINE\\SYSTEM\\CurrentControlSet\\Services\\cdrom", "Start", 3, RegistryValueKind.DWord);
                SapphireTool.SetValue("EnableCDROM", 1);
            }
            else
            {
                Registry.SetValue("HKEY_LOCAL_MACHINE\\SYSTEM\\CurrentControlSet\\Services\\cdrom", "Start", 4, RegistryValueKind.DWord);
                SapphireTool.DeleteValue("EnableCDROM");
            }
        }

        private void tsPrintSpooler_Click(object sender, RoutedEventArgs e)
        {
            if (tsPrintSpooler.IsChecked == true)
            {
                Registry.SetValue("HKEY_LOCAL_MACHINE\\SYSTEM\\CurrentControlSet\\Services\\Spooler", "Start", 4, RegistryValueKind.DWord);
                SapphireTool.SetValue("DisablePrinter", 1);
            }
            else
            {
                Registry.SetValue("HKEY_LOCAL_MACHINE\\SYSTEM\\CurrentControlSet\\Services\\Spooler", "Start", 2, RegistryValueKind.DWord);
                SapphireTool.DeleteValue("DisablePrinter");
            }
        }

        private void tsNoLazyMode_Click(object sender, RoutedEventArgs e)
        {
            if (tsNoLazyMode.IsChecked == true)
            {
                Registry.SetValue("HKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion\\Multimedia\\SystemProfile", "NoLazyMode", 1, RegistryValueKind.DWord);
                SapphireTool.SetValue("NoLazyMode", 1);
            }
            else
            {
                Registry.SetValue("HKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion\\Multimedia\\SystemProfile", "NoLazyMode", 0, RegistryValueKind.DWord);
                SapphireTool.DeleteValue("NoLazyMode");
            }
        }

        private void tsUACAdmin_Click(object sender, RoutedEventArgs e)
        {
            if (tsUACAdmin.IsChecked == true)
            {
                Registry.SetValue("HKEY_LOCAL_MACHINE\\SYSTEM\\CurrentControlSet\\Services\\AppInfo", "Start", 2, RegistryValueKind.DWord);
                Registry.SetValue("HKEY_LOCAL_MACHINE\\SYSTEM\\CurrentControlSet\\Services\\AppInfo", "Luafv", 2, RegistryValueKind.DWord);
                Registry.SetValue("HKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Policies\\System", "ValidateAdminCodeSignatures", 0, RegistryValueKind.DWord);
                Registry.SetValue("HKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Policies\\System", "EnableLUA", 1, RegistryValueKind.DWord);
                Registry.SetValue("HKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Policies\\System", "FilterAdministratorToken", 1, RegistryValueKind.DWord);
                SapphireTool.SetValue("EnableAdminUAC", 1);
            }
            else
            {
                Registry.SetValue("HKEY_LOCAL_MACHINE\\SYSTEM\\CurrentControlSet\\Services\\AppInfo", "Start", 4, RegistryValueKind.DWord);
                Registry.SetValue("HKEY_LOCAL_MACHINE\\SYSTEM\\CurrentControlSet\\Services\\Luafv", "Start", 4, RegistryValueKind.DWord);
                Registry.SetValue("HKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Policies\\System", "ValidateAdminCodeSignatures", 0, RegistryValueKind.DWord);
                Registry.SetValue("HKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Policies\\System", "EnableLUA", 0, RegistryValueKind.DWord);
                Registry.SetValue("HKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Policies\\System", "FilterAdministratorToken", 0, RegistryValueKind.DWord);
                SapphireTool.DeleteValue("EnableAdminUAC");
            }
        }

        private void tsVR_Click(object sender, RoutedEventArgs e)
        {
            if (tsVR.IsChecked == true)
            {
                Registry.SetValue("HKEY_LOCAL_MACHINE\\SYSTEM\\ControlSet001\\Services\\KSecPkg", "Start", 0, RegistryValueKind.DWord);
                Registry.SetValue("HKEY_LOCAL_MACHINE\\SYSTEM\\ControlSet001\\Services\\LanmanWorkstation", "Start", 2, RegistryValueKind.DWord);
                Registry.SetValue("HKEY_LOCAL_MACHINE\\SYSTEM\\ControlSet001\\Services\\mrxsmb", "Start", 3, RegistryValueKind.DWord);
                Registry.SetValue("HKEY_LOCAL_MACHINE\\SYSTEM\\ControlSet001\\Services\\mrxsmb20", "Start", 3, RegistryValueKind.DWord);
                Registry.SetValue("HKEY_LOCAL_MACHINE\\SYSTEM\\ControlSet001\\Services\\rdbss", "Start", 1, RegistryValueKind.DWord);
                Registry.SetValue("HKEY_LOCAL_MACHINE\\SYSTEM\\ControlSet001\\Services\\srv2", "Start", 2, RegistryValueKind.DWord);
                Registry.SetValue("HKEY_LOCAL_MACHINE\\SYSTEM\\ControlSet001\\Services\\QwaveDrv", "Start", 3, RegistryValueKind.DWord);
                Registry.SetValue("HKEY_LOCAL_MACHINE\\SYSTEM\\ControlSet001\\Services\\Qwave", "Start", 3, RegistryValueKind.DWord);
                Registry.SetValue("HKEY_LOCAL_MACHINE\\SYSTEM\\ControlSet001\\Services\\FontCache", "Start", 2, RegistryValueKind.DWord);
                Utils.RunCommand("cmd", "/c DISM /Online /Enable-Feature /FeatureName:SmbDirect /NoRestart");
                SapphireTool.SetValue("EnableVR", 1);
            }
            else
            {
                Registry.SetValue("HKEY_LOCAL_MACHINE\\SYSTEM\\ControlSet001\\Services\\KSecPkg", "Start", 4, RegistryValueKind.DWord);
                Registry.SetValue("HKEY_LOCAL_MACHINE\\SYSTEM\\ControlSet001\\Services\\LanmanWorkstation", "Start", 4, RegistryValueKind.DWord);
                Registry.SetValue("HKEY_LOCAL_MACHINE\\SYSTEM\\ControlSet001\\Services\\mrxsmb", "Start", 4, RegistryValueKind.DWord);
                Registry.SetValue("HKEY_LOCAL_MACHINE\\SYSTEM\\ControlSet001\\Services\\mrxsmb20", "Start", 4, RegistryValueKind.DWord);
                Registry.SetValue("HKEY_LOCAL_MACHINE\\SYSTEM\\ControlSet001\\Services\\rdbss", "Start", 4, RegistryValueKind.DWord);
                Registry.SetValue("HKEY_LOCAL_MACHINE\\SYSTEM\\ControlSet001\\Services\\srv2", "Start", 4, RegistryValueKind.DWord);
                Registry.SetValue("HKEY_LOCAL_MACHINE\\SYSTEM\\ControlSet001\\Services\\QwaveDrv", "Start", 4, RegistryValueKind.DWord);
                Registry.SetValue("HKEY_LOCAL_MACHINE\\SYSTEM\\ControlSet001\\Services\\Qwave", "Start", 4, RegistryValueKind.DWord);
                Registry.SetValue("HKEY_LOCAL_MACHINE\\SYSTEM\\ControlSet001\\Services\\FontCache", "Start", 4, RegistryValueKind.DWord);
                Utils.RunCommand("cmd", "/c DISM /Online /Disable-Feature /FeatureName:SmbDirect /NoRestart");
                if (OSName.Contains("Windows 11"))
                {
                    Registry.SetValue("HKEY_LOCAL_MACHINE\\SYSTEM\\ControlSet001\\Services\\KSecPkg", "Start", 0, RegistryValueKind.DWord);
                    Registry.SetValue("HKEY_LOCAL_MACHINE\\SYSTEM\\ControlSet001\\Services\\srv2", "Start", 2, RegistryValueKind.DWord);
                    Registry.SetValue("HKEY_LOCAL_MACHINE\\SYSTEM\\ControlSet001\\Services\\mrxsmb20", "Start", 3, RegistryValueKind.DWord);
                    Registry.SetValue("HKEY_LOCAL_MACHINE\\SYSTEM\\ControlSet001\\Services\\LanmanWorkstation", "Start", 2, RegistryValueKind.DWord);
                }
                SapphireTool.DeleteValue("EnableVR");
            }
        }

        private void tsUAC_Click(object sender, RoutedEventArgs e)
        {
            if (tsUAC.IsChecked == true)
            {
                Registry.SetValue("HKEY_LOCAL_MACHINE\\SYSTEM\\CurrentControlSet\\Services\\AppInfo", "Start", 2, RegistryValueKind.DWord);
                Registry.SetValue("HKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Policies\\System", "EnableLUA", 1, RegistryValueKind.DWord);
                SapphireTool.SetValue("EnableUAC", 1);
            }
            else
            {
                Registry.SetValue("HKEY_LOCAL_MACHINE\\SYSTEM\\CurrentControlSet\\Services\\AppInfo", "Start", 4, RegistryValueKind.DWord);
                Registry.SetValue("HKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Policies\\System", "EnableLUA", 0, RegistryValueKind.DWord);
                SapphireTool.DeleteValue("EnableUAC");
            }
        }

        private void tsStartMenu_Click(object sender, RoutedEventArgs e)
        {
            if (tsStartMenu.IsChecked == true)
            {
                if (OSName.Contains("Windows 11"))
                {
                    Process.Start(@"C:\PostInstall\Others\Startmenu\disable startmenu 11.bat");
                }
                else
                {
                    Process.Start(@"C:\PostInstall\Others\Startmenu\disable startmenu 10 and server.bat");
                }
                SapphireTool.SetValue("DisableStartmenu", 1);
            }
            else
            {
                if (OSName.Contains("Windows 11"))
                {
                    Process.Start(@"C:\PostInstall\Others\Startmenu\enable startmenu 11.bat");
                }
                else
                {
                    Process.Start(@"C:\PostInstall\Others\Startmenu\enable startmenu.bat");
                }
                SapphireTool.DeleteValue("DisableStartmenu");
            }
        }

        private void tsHyperV_Click(object sender, RoutedEventArgs e)
        {
            if (tsHyperV.IsChecked == true)
            {
                Utils.RunCommand("bcdedit", "/set hypervisorlaunchtype off");
                Utils.RunCommand("bcdedit", "/set vm no");
                Utils.RunCommand("bcdedit", "/set vsmlaunchtype Off");
                Utils.RunCommand("DISM", "/Online /Disable-Feature:Microsoft-Hyper-V-All /Quiet /NoRestart");
                Registry.SetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DeviceGuard", "EnableVirtualizationBasedSecurity", 0, RegistryValueKind.DWord);
                Registry.SetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DeviceGuard", "RequirePlatformSecurityFeatures", 1, RegistryValueKind.DWord);
                Registry.SetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DeviceGuard", "HypervisorEnforcedCodeIntegrity", 0, RegistryValueKind.DWord);
                Registry.SetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DeviceGuard", "HVCIMATRequired", 0, RegistryValueKind.DWord);
                Registry.SetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DeviceGuard", "LsaCfgFlags", 0, RegistryValueKind.DWord);
                Registry.SetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DeviceGuard", "ConfigureSystemGuardLaunch", 0, RegistryValueKind.DWord);
                Registry.SetValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\DeviceGuard", "RequireMicrosoftSignedBootChain", 0, RegistryValueKind.DWord);
                Registry.SetValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\DeviceGuard\Scenarios\HypervisorEnforcedCodeIntegrity", "WasEnabledBy", 0, RegistryValueKind.DWord);
                Registry.SetValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\DeviceGuard\Scenarios\HypervisorEnforcedCodeIntegrity", "Enabled", 0, RegistryValueKind.DWord);
                SapphireTool.SetValue("DisableHyperV", 1);
            }
            else
            {
                Utils.RunCommand("bcdedit", "/set hypervisorlaunchtype auto");
                Utils.RunCommand("bcdedit", "/deletevalue vm");
                Utils.RunCommand("bcdedit", "/deletevalue vsmlaunchtype");
                Utils.RunCommand("DISM", "/Online /Enable-Feature:Microsoft-Hyper-V-All /Quiet /NoRestart");
                Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Policies\Microsoft\Windows\DeviceGuard", true)?.DeleteValue("EnableVirtualizationBasedSecurity", false);
                Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Policies\Microsoft\Windows\DeviceGuard", true)?.DeleteValue("RequirePlatformSecurityFeatures", false);
                Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Policies\Microsoft\Windows\DeviceGuard", true)?.DeleteValue("HypervisorEnforcedCodeIntegrity", false);
                Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Policies\Microsoft\Windows\DeviceGuard", true)?.DeleteValue("HVCIMATRequired", false);
                Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Policies\Microsoft\Windows\DeviceGuard", true)?.DeleteValue("LsaCfgFlags", false);
                Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Policies\Microsoft\Windows\DeviceGuard", true)?.DeleteValue("ConfigureSystemGuardLaunch", false);
                Registry.SetValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\DeviceGuard", "RequireMicrosoftSignedBootChain", 1, RegistryValueKind.DWord);
                Registry.SetValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\DeviceGuard", "EnableVirtualizationBasedSecurity", 1, RegistryValueKind.DWord);
                Registry.SetValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\DeviceGuard", "RequirePlatformSecurityFeatures", 1, RegistryValueKind.DWord);
                Registry.SetValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\DeviceGuard", "Locked", 0, RegistryValueKind.DWord);
                Registry.SetValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\DeviceGuard\Scenarios\HypervisorEnforcedCodeIntegrity", "Enabled", 1, RegistryValueKind.DWord);
                Registry.SetValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\DeviceGuard\Scenarios\HypervisorEnforcedCodeIntegrity", "Locked", 0, RegistryValueKind.DWord);
                Registry.SetValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\DeviceGuard\Scenarios\HypervisorEnforcedCodeIntegrity", "WasEnabledBy", 1, RegistryValueKind.DWord);
                SapphireTool.DeleteValue("DisableHyperV");
            }
        }

        private void tsVBS_Click(object sender, RoutedEventArgs e)
        {
            if (tsVBS.IsChecked == true)
            {
                Registry.SetValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\DeviceGuard\Scenarios\HypervisorEnforcedCodeIntegrity", "Enabled", 1, RegistryValueKind.DWord);
                SapphireTool.SetValue("VBS", 1);
            }
            else
            {
                Registry.SetValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\DeviceGuard\Scenarios\HypervisorEnforcedCodeIntegrity", "Enabled", 0, RegistryValueKind.DWord);
                SapphireTool.DeleteValue("VBS");
            }
        }

        private void tsWallpaperQuality_Click(object sender, RoutedEventArgs e)
        {
            if (tsWallpaperQuality.IsChecked == true)
            {
                Registry.SetValue(@"HKEY_CURRENT_USER\Control Panel\Desktop", "JPEGImportQuality", 100, RegistryValueKind.String);
                SapphireTool.SetValue("WallpaperQuality", 1);
            }
            else
            {
                RegistryKey currentUserKey = Registry.CurrentUser;
                currentUserKey.OpenSubKey(@"Control Panel\Desktop", true)?.DeleteValue("JPEGImportQuality", false);
                SapphireTool.DeleteValue("WallpaperQuality");
            }
        }

        private void tsMPO_Click(object sender, RoutedEventArgs e)
        {
            if (tsMPO.IsChecked == true)
            {
                Process.Start("C:\\PostInstall\\GPU\\Nvidia\\mpo disable.bat");
                SapphireTool.SetValue("DisableMPO", 1);
            }
            else
            {
                Process.Start("C:\\PostInstall\\GPU\\Nvidia\\mpo enable.bat");
                SapphireTool.DeleteValue("DisableMPO");
            }
        }

        private void tsTransparency_Click(object sender, RoutedEventArgs e)
        {
            if (tsTransparency.IsChecked == true)
            {
                Registry.SetValue(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Themes\Personalize", "EnableTransparency", 1, RegistryValueKind.DWord);
                Utils.RestartExplorer();
                SapphireTool.SetValue("TransparencyEffects", 1);
            }
            else
            {
                Registry.SetValue(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Themes\Personalize", "EnableTransparency", 0, RegistryValueKind.DWord);
                Utils.RestartExplorer();
                SapphireTool.DeleteValue("TransparencyEffects");
            }
        }

        private void tsLockScreen_Click(object sender, RoutedEventArgs e)
        {
            if (tsLockScreen.IsChecked == true)
            {
                Registry.SetValue("HKEY_LOCAL_MACHINE\\SOFTWARE\\Policies\\Microsoft\\Windows\\Personalization", "NoLockScreen", 1, RegistryValueKind.DWord);
                SapphireTool.SetValue("DisableLockScreen", 1);
            }
            else
            {
                Registry.SetValue("HKEY_LOCAL_MACHINE\\SOFTWARE\\Policies\\Microsoft\\Windows\\Personalization", "NoLockScreen", 0, RegistryValueKind.DWord);
                SapphireTool.DeleteValue("DisableLockScreen");
            }
        }

        private void tsAnimations_Click(object sender, RoutedEventArgs e)
        {
            if (tsAnimations.IsChecked == true)
            {
                Registry.SetValue("HKEY_LOCAL_MACHINE\\SOFTWARE\\Policies\\Microsoft\\Windows\\DWM", "DisallowAnimations", 1, RegistryValueKind.DWord);
                Registry.SetValue("HKEY_CURRENT_USER\\SOFTWARE\\Microsoft\\Windows\\DWM", "EnableAeroPeek", 0, RegistryValueKind.DWord);
                Registry.SetValue("HKEY_CURRENT_USER\\SOFTWARE\\Microsoft\\Windows\\DWM", "AlwaysHibernateThumbnails", 0, RegistryValueKind.DWord);
                Registry.SetValue("HKEY_CURRENT_USER\\Control Panel\\Desktop\\WindowMetrics", "MinAnimate", 0, RegistryValueKind.DWord);
                Registry.SetValue("HKEY_CURRENT_USER\\SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Explorer\\Advanced", "TaskbarAnimations", 0, RegistryValueKind.DWord);
                Registry.SetValue("HKEY_CURRENT_USER\\SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Explorer\\Advanced", "IconsOnly", 1, RegistryValueKind.DWord);
                Registry.SetValue("HKEY_CURRENT_USER\\SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Explorer\\Advanced", "ListviewAlphaSelect", 0, RegistryValueKind.DWord);
                Registry.SetValue("HKEY_CURRENT_USER\\SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Explorer\\Advanced", "ListviewShadow", 0, RegistryValueKind.DWord);
                Registry.SetValue("HKEY_CURRENT_USER\\SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Explorer\\VisualEffects", "VisualFXSetting", 3, RegistryValueKind.DWord);
                byte[] userPreferencesMaskData = new byte[8] { 144, 18, 3, 128, 16, 0, 0, 0 };
                Registry.SetValue("HKEY_CURRENT_USER\\Control Panel\\Desktop", "UserPreferencesMask", userPreferencesMaskData, RegistryValueKind.Binary);
                SapphireTool.SetValue("DisableAnimations", 1);
            }
            else
            {
                Registry.SetValue("HKEY_CURRENT_USER\\SOFTWARE\\Policies\\Microsoft\\Windows\\DWM", "EnableAeroPeek", 1, RegistryValueKind.DWord);
                Registry.SetValue("HKEY_CURRENT_USER\\SOFTWARE\\Policies\\Microsoft\\Windows\\DWM", "AlwaysHibernateThumbnails", 1, RegistryValueKind.DWord);
                RegistryKey localMachineKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Policies\\Microsoft\\Windows\\DWM", writable: true);
                if (localMachineKey != null)
                {
                    localMachineKey.DeleteValue("DisallowAnimations", throwOnMissingValue: false);
                    localMachineKey.Close();
                }
                RegistryKey currentUserKey = Registry.CurrentUser.OpenSubKey("Control Panel\\Desktop\\WindowMetrics", writable: true);
                if (currentUserKey != null)
                {
                    currentUserKey.DeleteValue("MinAnimate", throwOnMissingValue: false);
                    currentUserKey.Close();
                }
                Registry.SetValue("HKEY_CURRENT_USER\\SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Explorer\\Advanced", "TaskbarAnimations", 1, RegistryValueKind.DWord);
                Registry.SetValue("HKEY_CURRENT_USER\\SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Explorer\\Advanced", "IconsOnly", 0, RegistryValueKind.DWord);
                Registry.SetValue("HKEY_CURRENT_USER\\SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Explorer\\Advanced", "ListviewAlphaSelect", 1, RegistryValueKind.DWord);
                Registry.SetValue("HKEY_CURRENT_USER\\SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Explorer\\Advanced", "ListviewShadow", 1, RegistryValueKind.DWord);
                Registry.SetValue("HKEY_CURRENT_USER\\SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Explorer\\VisualEffects", "VisualFXSetting", 1, RegistryValueKind.DWord);
                byte[] userPreferencesMaskData = new byte[8] { 158, 62, 7, 128, 18, 0, 0, 0 };
                Registry.SetValue("HKEY_CURRENT_USER\\Control Panel\\Desktop", "UserPreferencesMask", userPreferencesMaskData, RegistryValueKind.Binary);
                SapphireTool.DeleteValue("DisableAnimations");
            }
        }

        private void tsDCOM_Click(object sender, RoutedEventArgs e)
        {
            if (tsDCOM.IsChecked == true)
            {
                Registry.SetValue("HKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\\Ole", "EnableDCOM", "N", RegistryValueKind.String);
                SapphireTool.SetValue("DisableDCOM", 1);
                 }
            else
            {
                Registry.SetValue("HKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\\Ole", "EnableDCOM", "Y", RegistryValueKind.String);
                SapphireTool.DeleteValue("DisableDCOM");
            }
        }

        private void tsNVMETweaks_Click(object sender, RoutedEventArgs e)
        {
            if (tsNVMETweaks.IsChecked == true)
            {
                Registry.SetValue("HKEY_LOCAL_MACHINE\\SYSTEM\\ControlSet001\\Services\\stornvme\\Parameters\\Device", "ContiguousMemoryFromAnyNode", 1, RegistryValueKind.DWord);
                Registry.SetValue("HKEY_LOCAL_MACHINE\\SYSTEM\\ControlSet001\\Services\\stornvme\\Parameters\\Device", "LogSize", 0, RegistryValueKind.DWord);
                Registry.SetValue("HKEY_LOCAL_MACHINE\\SYSTEM\\ControlSet001\\Services\\stornvme\\Parameters\\Device", "IdlePowerMode", 0, RegistryValueKind.DWord);
                Registry.SetValue("HKEY_LOCAL_MACHINE\\SYSTEM\\ControlSet001\\Services\\stornvme\\Parameters\\Device", "DiagnosticFlags", 0, RegistryValueKind.DWord);
                SapphireTool.SetValue("NVMETweaks", 1);
            }
            else
            {
                RegistryKey LocalMachineKey = Registry.LocalMachine;
                LocalMachineKey.OpenSubKey(@"SYSTEM\ControlSet001\Services\stornvme\Parameters\Device", true)?.DeleteValue("ContiguousMemoryFromAnyNode", false);
                LocalMachineKey.OpenSubKey(@"SYSTEM\ControlSet001\Services\stornvme\Parameters\Device", true)?.DeleteValue("LogSize", false);
                LocalMachineKey.OpenSubKey(@"SYSTEM\ControlSet001\Services\stornvme\Parameters\Device", true)?.DeleteValue("IdlePowerMode", false);
                LocalMachineKey.OpenSubKey(@"SYSTEM\ControlSet001\Services\stornvme\Parameters\Device", true)?.DeleteValue("DiagnosticFlags", false);
                SapphireTool.DeleteValue("NVMETweaks");
            }
        }

        private void tsLargeSystemCache_Click(object sender, RoutedEventArgs e)
        {
            if (tsLargeSystemCache.IsChecked == true)
            {
                Registry.SetValue("HKEY_LOCAL_MACHINE\\SYSTEM\\CurrentControlSet\\Control\\Session Manager\\Memory Management", "LargeSystemCache", 1, RegistryValueKind.DWord);
                SapphireTool.SetValue("EnableLargeSystemCache", 1);
            }
            else
            {
                Registry.SetValue("HKEY_LOCAL_MACHINE\\SYSTEM\\CurrentControlSet\\Control\\Session Manager\\Memory Management", "LargeSystemCache", 0, RegistryValueKind.DWord);
                SapphireTool.DeleteValue("EnableLargeSystemCache");
            }
        }

        private void tsSystemProfile_Click(object sender, RoutedEventArgs e)
        {
            if (tsSystemProfile.IsChecked == true)
            {
                Registry.SetValue("HKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion\\Multimedia\\SystemProfile\\Tasks\\Games", "Affinity", 0, RegistryValueKind.DWord);
                Registry.SetValue("HKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion\\Multimedia\\SystemProfile\\Tasks\\Games", "Background Only", "False", RegistryValueKind.String);
                Registry.SetValue("HKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion\\Multimedia\\SystemProfile\\Tasks\\Games", "Clock Rate", 2710, RegistryValueKind.DWord);
                Registry.SetValue("HKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion\\Multimedia\\SystemProfile\\Tasks\\Games", "GPU Priority", 8, RegistryValueKind.DWord);
                Registry.SetValue("HKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion\\Multimedia\\SystemProfile\\Tasks\\Games", "Priority", 8, RegistryValueKind.DWord);
                Registry.SetValue("HKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion\\Multimedia\\SystemProfile\\Tasks\\Games", "SFIO Priority", "High", RegistryValueKind.String);
                Registry.SetValue("HKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion\\Multimedia\\SystemProfile\\Tasks\\Pro Audio", "Priority", 8, RegistryValueKind.DWord);
                Registry.SetValue("HKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion\\Multimedia\\SystemProfile\\Tasks\\Pro Audio", "Scheduling Category", "Medium", RegistryValueKind.String);
                Registry.SetValue("HKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion\\Multimedia\\SystemProfile\\Tasks\\Audio", "Priority", 8, RegistryValueKind.DWord);
                SapphireTool.SetValue("SystemProfile", 1);
            }
            else
            {
                RegistryKey LocalMachineKey = Registry.LocalMachine;
                LocalMachineKey.OpenSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile\Tasks\Games", true)?.DeleteValue("Affinity", false);
                LocalMachineKey.OpenSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile\Tasks\Games", true)?.DeleteValue("Background Only", false);
                LocalMachineKey.OpenSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile\Tasks\Games", true)?.DeleteValue("Clock Rate", false);
                LocalMachineKey.OpenSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile\Tasks\Games", true)?.DeleteValue("GPU Priority", false);
                LocalMachineKey.OpenSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile\Tasks\Games", true)?.DeleteValue("Priority", false);
                LocalMachineKey.OpenSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile\Tasks\Games", true)?.DeleteValue("SFIO Priority", false);
                LocalMachineKey.OpenSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile\Tasks\Pro Audio", true)?.DeleteValue("Priority", false);
                LocalMachineKey.OpenSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile\Tasks\Pro Audio", true)?.DeleteValue("Scheduling Category", false);
                LocalMachineKey.OpenSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile\Tasks\Audio", true)?.DeleteValue("Priority", false);
                SapphireTool.DeleteValue("SystemProfile");
            }
        }
    }
}
