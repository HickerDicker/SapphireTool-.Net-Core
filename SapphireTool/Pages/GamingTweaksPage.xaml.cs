using System;
using Microsoft.Win32;
using System.Windows;
using System.Windows.Controls;
using Wpf.Ui.Controls;
using System.Diagnostics;
using SapphireTool.Classes;

namespace SapphireTool.Pages
{
    public partial class GamingTweaksPage : Page
    {
        public static RegistryKey SapphireTool = Registry.CurrentUser.CreateSubKey(@"Software\SapphireTool", RegistryKeyPermissionCheck.ReadWriteSubTree);
        private bool _isInitializing = true;

        public GamingTweaksPage()
        {
            InitializeComponent();
            LoadSavedSettings();
            _isInitializing = false;
        }

        private void LoadSavedSettings()
        {
            tsPreemption.IsChecked = RegistryTools.CheckTweakState(SapphireTool, "Preemption", 1);
            tsHDCP.IsChecked = RegistryTools.CheckTweakState(SapphireTool, "HDCP", 1);
            tsNetworkOptimization.IsChecked = RegistryTools.CheckTweakState(SapphireTool, "NetworkOptimization", 1);

            string savedService = SapphireTool.GetValue("Services") as string;
            if (!string.IsNullOrEmpty(savedService))
            {
                foreach (ComboBoxItem item in cbServices.Items)
                {
                    if (item.Content.ToString() == savedService)
                    {
                        cbServices.SelectedItem = item;
                        break;
                    }
                }
            }

            object splitThreshold = SapphireTool.GetValue("SvcHostSplitThreshold");
            if (splitThreshold != null && splitThreshold is int ramValue)
            {
                foreach (ComboBoxItem item in cbSvcHostSplit.Items)
                {
                    if (item.Content.ToString().StartsWith(ramValue.ToString()))
                    {
                        cbSvcHostSplit.SelectedItem = item;
                        break;
                    }
                }
            }

            object prioritySeparation = SapphireTool.GetValue("Win32PrioritySeparation");
            if (prioritySeparation != null && prioritySeparation is int hexValue)
            {
                string hexString = hexValue.ToString("X");
                foreach (ComboBoxItem item in cbWin32Priority.Items)
                {
                    if (item.Content.ToString().StartsWith(hexString))
                    {
                        cbWin32Priority.SelectedItem = item;
                        break;
                    }
                }
            }
        }

        private void tsPreemption_Click(object sender, RoutedEventArgs e)
        {
            if (tsPreemption.IsChecked == true)
            {
                Registry.SetValue("HKEY_LOCAL_MACHINE\\SYSTEM\\CurrentControlSet\\Control\\GraphicsDrivers\\Scheduler", "EnablePreemption", 0, RegistryValueKind.DWord);
                Registry.SetValue("HKEY_LOCAL_MACHINE\\System\\CurrentControlSet\\Services\\nvlddmkm", "DisablePreemption", 1, RegistryValueKind.DWord);
                Registry.SetValue("HKEY_LOCAL_MACHINE\\System\\CurrentControlSet\\Services\\nvlddmkm", "DisableCudaContextPreemption", 1, RegistryValueKind.DWord);
                Registry.SetValue("HKEY_LOCAL_MACHINE\\System\\CurrentControlSet\\Services\\nvlddmkm", "EnableCEPreemption", 0, RegistryValueKind.DWord);
                Registry.SetValue("HKEY_LOCAL_MACHINE\\System\\CurrentControlSet\\Services\\nvlddmkm", "DisablePreemptionOnS3S4", 1, RegistryValueKind.DWord);
                Registry.SetValue("HKEY_LOCAL_MACHINE\\System\\CurrentControlSet\\Services\\nvlddmkm", "ComputePreemption", 0, RegistryValueKind.DWord);
                SapphireTool.SetValue("Preemption", 1);
            }
            else
            {
                Registry.SetValue("HKEY_LOCAL_MACHINE\\SYSTEM\\CurrentControlSet\\Control\\GraphicsDrivers\\Scheduler", "EnablePreemption", 1, RegistryValueKind.DWord);
                Registry.SetValue("HKEY_LOCAL_MACHINE\\System\\CurrentControlSet\\Services\\nvlddmkm", "DisablePreemption", 0, RegistryValueKind.DWord);
                Registry.SetValue("HKEY_LOCAL_MACHINE\\System\\CurrentControlSet\\Services\\nvlddmkm", "DisableCudaContextPreemption", 0, RegistryValueKind.DWord);
                Registry.SetValue("HKEY_LOCAL_MACHINE\\System\\CurrentControlSet\\Services\\nvlddmkm", "EnableCEPreemption", 1, RegistryValueKind.DWord);
                Registry.SetValue("HKEY_LOCAL_MACHINE\\System\\CurrentControlSet\\Services\\nvlddmkm", "DisablePreemptionOnS3S4", 0, RegistryValueKind.DWord);
                Registry.SetValue("HKEY_LOCAL_MACHINE\\System\\CurrentControlSet\\Services\\nvlddmkm", "ComputePreemption", 1, RegistryValueKind.DWord);
                SapphireTool.DeleteValue("Preemption");
            }
        }

        private void tsHDCP_Click(object sender, RoutedEventArgs e)
        {
            if (tsHDCP.IsChecked == true)
            {
                Registry.SetValue("HKEY_LOCAL_MACHINE\\SYSTEM\\CurrentControlSet\\Control\\Class\\{4d36e968-e325-11ce-bfc1-08002be10318}\\0001", "RMHdcpKeyglobZero", 1, RegistryValueKind.DWord);
                SapphireTool.SetValue("HDCP", 1);
            }
            else
            {
                Registry.SetValue("HKEY_LOCAL_MACHINE\\SYSTEM\\CurrentControlSet\\Control\\Class\\{4d36e968-e325-11ce-bfc1-08002be10318}\\0001", "RMHdcpKeyglobZero", 0, RegistryValueKind.DWord);
                SapphireTool.DeleteValue("HDCP");
            }
        }

        private void tsNetworkOptimization_Click(object sender, RoutedEventArgs e)
        {
            if (tsNetworkOptimization.IsChecked == true)
            {
                Process.Start("C:\\PostInstall\\Others\\Network\\Run this if you had to install a network driver.bat");
                SapphireTool.SetValue("NetworkOptimization", 1);
            }
            else
            {
                Process.Start("C:\\PostInstall\\Others\\Network\\Revert Network Tweaks.bat");
                SapphireTool.DeleteValue("NetworkOptimization");
            }
        }

        private void cbServices_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_isInitializing || cbServices.SelectedItem == null) return;

            string selectedService = (cbServices.SelectedItem as ComboBoxItem)?.Content.ToString();

            switch (selectedService)
            {
                case "Default":
                    Process.Start(@"C:\PostInstall\Services\exes enable.bat");
                    Utils.RunCommand("C:\\PostInstall\\Tweaks\\Nsudo.exe", "-U:S -P:E cmd /c C:\\PostInstall\\Services\\Windows-Default-services.reg");
                    SapphireTool.SetValue("Services", "Default");
                    break;

                case "SapphireOS":
                    Process.Start(@"C:\PostInstall\Services\exes enable.bat");
                    Utils.RunCommand("C:\\PostInstall\\Tweaks\\Nsudo.exe", "-U:S -P:E cmd /c C:\\PostInstall\\Services\\SapphireOS-Default-services.reg");
                    SapphireTool.SetValue("Services", "SapphireOS");
                    break;

                case "Minimal":
                    Registry.SetValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services",
                                    "SapphireServiceMode", "Minimal", RegistryValueKind.String);
                    Process.Start(@"C:\PostInstall\Services\exes.bat");
                    Utils.RunCommand("C:\\PostInstall\\Tweaks\\Nsudo.exe", "-U:S -P:E cmd /c C:\\PostInstall\\Services\\minimal-services.reg");
                    SapphireTool.SetValue("Services", "Minimal");
                    // this was missing in the last version of the os btw in case anyone is wondering about why they had to manually do it oopsie daisy
                    break;
            }
        }

        private void cbSvcHostSplit_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_isInitializing || cbSvcHostSplit.SelectedItem == null) return;

            string selectedThreshold = (cbSvcHostSplit.SelectedItem as ComboBoxItem)?.Content.ToString();
            int ramValue = ParseRamValue(selectedThreshold);

            if (ramValue > 0)
            {
                Registry.SetValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control",
                                "SvcHostSplitThresholdInKB", ramValue * 1024 * 1024, RegistryValueKind.DWord);
                SapphireTool.SetValue("SvcHostSplitThreshold", ramValue);
            }
        }

        private void cbWin32Priority_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_isInitializing || cbWin32Priority.SelectedItem == null) return;

            string selectedPriority = (cbWin32Priority.SelectedItem as ComboBoxItem)?.Content.ToString();
            int hexValue = ParseHexValue(selectedPriority);

            if (hexValue > 0)
            {
                Registry.SetValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\PriorityControl",
                                "Win32PrioritySeparation", hexValue, RegistryValueKind.DWord);
                SapphireTool.SetValue("Win32PrioritySeparation", hexValue);
            }
        }

        #region NVIDIA Tweaks

        private void btnNIP_Click(object sender, RoutedEventArgs e)
        {
            Utils.RunCommand("C:\\PostInstall\\GPU\\Nvidia\\NIP\\nvidiaProfileInspector.exe", "/s C:\\PostInstall\\GPU\\Nvidia\\NIP\\Settings.nip");
        }

        private void btnPState0_Click(object sender, RoutedEventArgs e)
        {
            Registry.SetValue("HKEY_LOCAL_MACHINE\\SYSTEM\\CurrentControlSet\\Control\\Class\\{4d36e968-e325-11ce-bfc1-08002be10318}\\0000", "DisableDynamicPstate", 1, RegistryValueKind.DWord);
        }

        private void btnDisableECC_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("C:\\PostInstall\\GPU\\Nvidia\\!No ECC.bat");
        }

        private void btnDisableTelemetry_Click(object sender, RoutedEventArgs e)
        {
            Registry.SetValue("HKEY_LOCAL_MACHINE\\System\\CurrentControlSet\\Services\\nvlddmkm\\Global\\Startup", "SendTelemetryData", 0, RegistryValueKind.DWord);
            Registry.SetValue("HKEY_LOCAL_MACHINE\\SOFTWARE\\NVIDIA Corporation\\NvControlPanel2\\Client", "OptInOrOutPreference", 0, RegistryValueKind.DWord);
        }

        private void btnUnrestrictClockPolicy_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("C:\\PostInstall\\GPU\\Nvidia\\!Unrestricted Clock Policy by Cancerogeno.bat");
        }

        private void btnNVCleanstall_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("C:\\PostInstall\\GPU\\Nvidia\\NVCleanstall_1.18.0.exe");
        }
        private void btnMiscellaneous_Click(object sender, RoutedEventArgs e)
        {
            Registry.SetValue("HKEY_LOCAL_MACHINE\\SYSTEM\\CurrentControlSet\\Control\\Class\\{4d36e968-e325-11ce-bfc1-08002be10318}\\0000", "RmDisableHwFaultBuffer", 1, RegistryValueKind.DWord);
            Registry.SetValue("HKEY_LOCAL_MACHINE\\SYSTEM\\CurrentControlSet\\Control\\Class\\{4d36e968-e325-11ce-bfc1-08002be10318}\\0000", "RMD3Feature", 1, RegistryValueKind.DWord);
            Registry.SetValue("HKEY_LOCAL_MACHINE\\SYSTEM\\CurrentControlSet\\Control\\Class\\{4d36e968-e325-11ce-bfc1-08002be10318}\\0000", "RMDisableGpuASPMFlags", 3, RegistryValueKind.DWord);
            Registry.SetValue("HKEY_LOCAL_MACHINE\\SYSTEM\\CurrentControlSet\\Control\\Class\\{4d36e968-e325-11ce-bfc1-08002be10318}\\0000", "RMBlcg", 286331153, RegistryValueKind.DWord);
            Registry.SetValue("HKEY_LOCAL_MACHINE\\SYSTEM\\CurrentControlSet\\Control\\Class\\{4d36e968-e325-11ce-bfc1-08002be10318}\\0000", "RMElcg", 1431655765, RegistryValueKind.DWord);
            Registry.SetValue("HKEY_LOCAL_MACHINE\\SYSTEM\\CurrentControlSet\\Control\\Class\\{4d36e968-e325-11ce-bfc1-08002be10318}\\0000", "RMElpg", 4095, RegistryValueKind.DWord);
            Registry.SetValue("HKEY_LOCAL_MACHINE\\SYSTEM\\CurrentControlSet\\Control\\Class\\{4d36e968-e325-11ce-bfc1-08002be10318}\\0000", "RMFspg", 15, RegistryValueKind.DWord);
            Registry.SetValue("HKEY_LOCAL_MACHINE\\SYSTEM\\CurrentControlSet\\Control\\Class\\{4d36e968-e325-11ce-bfc1-08002be10318}\\0000", "RMSlcg", 262143, RegistryValueKind.DWord);
            Registry.SetValue("HKEY_LOCAL_MACHINE\\SYSTEM\\CurrentControlSet\\Control\\Class\\{4d36e968-e325-11ce-bfc1-08002be10318}\\0000", "EnableRuntimePowerManagement", 0, RegistryValueKind.DWord);
            Registry.SetValue("HKEY_LOCAL_MACHINE\\SYSTEM\\CurrentControlSet\\Control\\Class\\{4d36e968-e325-11ce-bfc1-08002be10318}\\0000", "DisableOverlay", 1, RegistryValueKind.DWord);
            Registry.SetValue("HKEY_LOCAL_MACHINE\\SYSTEM\\CurrentControlSet\\Control\\Class\\{4d36e968-e325-11ce-bfc1-08002be10318}\\0000", "D3PCLatency", 1, RegistryValueKind.DWord);
            Registry.SetValue("HKEY_LOCAL_MACHINE\\SYSTEM\\CurrentControlSet\\Control\\Class\\{4d36e968-e325-11ce-bfc1-08002be10318}\\0000", "F1TransitionLatency", 1, RegistryValueKind.DWord);
            Registry.SetValue("HKEY_LOCAL_MACHINE\\SYSTEM\\CurrentControlSet\\Control\\Class\\{4d36e968-e325-11ce-bfc1-08002be10318}\\0000", "Node3DLowLatency", 1, RegistryValueKind.DWord);
            Registry.SetValue("HKEY_LOCAL_MACHINE\\SYSTEM\\CurrentControlSet\\Control\\Class\\{4d36e968-e325-11ce-bfc1-08002be10318}\\0000", "PciLatencyTimerControl", 1, RegistryValueKind.DWord);
            Registry.SetValue("HKEY_LOCAL_MACHINE\\SYSTEM\\CurrentControlSet\\Control\\Class\\{4d36e968-e325-11ce-bfc1-08002be10318}\\0000", "RMDeepL1EntryLatencyUsec", 1, RegistryValueKind.DWord);
            Registry.SetValue("HKEY_LOCAL_MACHINE\\SYSTEM\\CurrentControlSet\\Control\\Class\\{4d36e968-e325-11ce-bfc1-08002be10318}\\0000", "RmGspcMaxFtuS", 1, RegistryValueKind.DWord);
            Registry.SetValue("HKEY_LOCAL_MACHINE\\SYSTEM\\CurrentControlSet\\Control\\Class\\{4d36e968-e325-11ce-bfc1-08002be10318}\\0000", "RmGspcMinFtuS", 1, RegistryValueKind.DWord);
            Registry.SetValue("HKEY_LOCAL_MACHINE\\SYSTEM\\CurrentControlSet\\Control\\Class\\{4d36e968-e325-11ce-bfc1-08002be10318}\\0000", "RmGspcPerioduS", 1, RegistryValueKind.DWord);
            Registry.SetValue("HKEY_LOCAL_MACHINE\\SYSTEM\\CurrentControlSet\\Control\\Class\\{4d36e968-e325-11ce-bfc1-08002be10318}\\0000", "RMLpwrEiIdleThresholdUs", 1, RegistryValueKind.DWord);
            Registry.SetValue("HKEY_LOCAL_MACHINE\\SYSTEM\\CurrentControlSet\\Control\\Class\\{4d36e968-e325-11ce-bfc1-08002be10318}\\0000", "RMLpwrGrIdleThresholdUs", 1, RegistryValueKind.DWord);
            Registry.SetValue("HKEY_LOCAL_MACHINE\\SYSTEM\\CurrentControlSet\\Control\\Class\\{4d36e968-e325-11ce-bfc1-08002be10318}\\0000", "RMLpwrGrRgIdleThresholdUs", 1, RegistryValueKind.DWord);
            Registry.SetValue("HKEY_LOCAL_MACHINE\\SYSTEM\\CurrentControlSet\\Control\\Class\\{4d36e968-e325-11ce-bfc1-08002be10318}\\0000", "RMLpwrMsIdleThresholdUs", 1, RegistryValueKind.DWord);
            Registry.SetValue("HKEY_LOCAL_MACHINE\\SYSTEM\\CurrentControlSet\\Control\\Class\\{4d36e968-e325-11ce-bfc1-08002be10318}\\0000", "PreferSystemMemoryContiguous", 1, RegistryValueKind.DWord);
            Registry.SetValue("HKEY_LOCAL_MACHINE\\SYSTEM\\CurrentControlSet\\Control\\Class\\{4d36e968-e325-11ce-bfc1-08002be10318}\\0000", "TCCSupported", 0, RegistryValueKind.DWord);
            Registry.SetValue("HKEY_LOCAL_MACHINE\\SYSTEM\\CurrentControlSet\\Control\\Class\\{4d36e968-e325-11ce-bfc1-08002be10318}\\0000", "RmCacheLoc", 0, RegistryValueKind.DWord);
            Registry.SetValue("HKEY_LOCAL_MACHINE\\SYSTEM\\CurrentControlSet\\Control\\Class\\{4d36e968-e325-11ce-bfc1-08002be10318}\\0000", "TrackResetEngine", 0, RegistryValueKind.DWord);
            Registry.SetValue("HKEY_CURRENT_USER\\Software\\NVIDIA Corporation\\Global\\NVTweak\\Devices\\509901423-0\\Color", "NvCplUseColorCorrection", 0, RegistryValueKind.DWord);
            Registry.SetValue("HKEY_LOCAL_MACHINE\\System\\CurrentControlSet\\Services\\nvlddmkm\\FTS", "EnableRID61684", 1, RegistryValueKind.DWord);
            Registry.SetValue("HKEY_LOCAL_MACHINE\\System\\CurrentControlSet\\Services\\nvlddmkm", "DisplayPowerSaving", 0, RegistryValueKind.DWord);
            Registry.SetValue("HKEY_LOCAL_MACHINE\\System\\CurrentControlSet\\Services\\nvlddmkm", "RmGpsPsEnablePerCpuCoreDpc", 1, RegistryValueKind.DWord);
            Registry.SetValue("HKEY_LOCAL_MACHINE\\System\\CurrentControlSet\\Services\\nvlddmkm", "DisableWriteCombining", 1, RegistryValueKind.DWord);
            Registry.SetValue("HKEY_LOCAL_MACHINE\\System\\CurrentControlSet\\Services\\nvlddmkm", "LogWarningEntries", 0, RegistryValueKind.DWord);
            Registry.SetValue("HKEY_LOCAL_MACHINE\\System\\CurrentControlSet\\Services\\nvlddmkm", "LogPagingEntries", 0, RegistryValueKind.DWord);
            Registry.SetValue("HKEY_LOCAL_MACHINE\\System\\CurrentControlSet\\Services\\nvlddmkm", "LogEventEntries", 0, RegistryValueKind.DWord);
            Registry.SetValue("HKEY_LOCAL_MACHINE\\System\\CurrentControlSet\\Services\\nvlddmkm", "LogErrorEntries", 0, RegistryValueKind.DWord);
            Registry.SetValue("HKEY_LOCAL_MACHINE\\System\\CurrentControlSet\\Control\\GraphicsDrivers", "PlatformSupportMiracast", 0, RegistryValueKind.DWord);
        }

        #endregion

        #region AMD Tweaks

        private void btnDwords_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("C:\\PostInstall\\GPU\\AMD\\AMD Dwords by imribiy.bat");
        }

        private void btnRSS_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("C:\\PostInstall\\GPU\\AMD\\radeon software slimmer\\RadeonSoftwareSlimmer.exe");
        }

        private void btnDriverDownload_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("https://www.amd.com/en/support/download/drivers.html");
        }

        private void btnDisableDXNAVI_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("C:\\PostInstall\\GPU\\AMD\\disable_dx11navi.exe");
        }

        private void btnShaderCacheOn_Click(object sender, RoutedEventArgs e)
        {
            Registry.SetValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Class\{4d36e968-e325-11ce-bfc1-08002be10318}\0000\UMD", "ShaderCache", new byte[] { 0x32, 0x00 }, RegistryValueKind.Binary);
        }

        private void btnShaderCacheDefault_Click(object sender, RoutedEventArgs e)
        {
            Registry.SetValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Class\{4d36e968-e325-11ce-bfc1-08002be10318}\0000\UMD", "ShaderCache", new byte[] { 0x31, 0x00 }, RegistryValueKind.Binary);
        }

        #endregion

        #region Useful Tools

        private void btnAutoruns_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("C:\\PostInstall\\Tweaks\\Autoruns.exe");
        }

        private void btnDevmanview_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("C:\\PostInstall\\Tweaks\\DevManView.exe");
        }

        private void btnServiwin_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("C:\\PostInstall\\Tweaks\\serviwin.exe");
        }

        private void btnInSpectre_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("C:\\PostInstall\\Mitigations\\InSpectre.exe");
        }

        private void btnMouseTester_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("C:\\PostInstall\\Tweaks\\Mouse Polling Test\\MouseTester.exe");
        }

        private void btnNSudo_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("C:\\PostInstall\\Tweaks\\NSudo.exe");
        }

        private void btnCRU_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("C:\\PostInstall\\Tweaks\\CRU\\CRU.exe");
        }

        private void btnAutoDscp_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("C:\\PostInstall\\Tweaks\\Auto DSCP & FSE.bat");
        }

        private void btnMsiUtil_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("C:\\PostInstall\\Tweaks\\MSI Mode Utility.exe");
        }

        private void btnDismpp_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("C:\\Users\\Administrator\\Desktop\\Dism++10.1.1002.1B\\Dism++x64.exe");
        }

        private void btnDevCleanup_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("C:\\PostInstall\\Tweaks\\DeviceCleanup.exe");
        }

        private void btnInterruptAfpt_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("C:\\PostInstall\\Tweaks\\Interrupt Affinity Policy Tool.exe");
        }

        private void btnHidUsb_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("C:\\PostInstall\\Tweaks\\hidusbf\\DRIVER\\Setup.exe");
        }

        private void btnMeasureSleep_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("C:\\PostInstall\\Tweaks\\MeasureSleep.exe");
        }

        private void btnProcessExplorer_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("C:\\PostInstall\\Tweaks\\Process explorer\\Process Explorer.exe");
        }

        private void btnReservedCPUSets_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("C:\\PostInstall\\Tweaks\\ReservedCpuSets.exe");
        }

        #endregion

        private int ParseHexValue(string input)
        {
            try
            {
                if (input.Contains("(Hex)"))
                {
                    string hexValue = input.Split(' ')[0];
                    return Convert.ToInt32(hexValue, 16);
                }
                return 0;
            }
            catch
            {
                return 0;
            }
        }

        private int ParseRamValue(string input)
        {
            try
            {
                if (input.Contains("GB RAM"))
                {
                    string ramValue = input.Split('G')[0];
                    if (int.TryParse(ramValue, out int result))
                    {
                        return result;
                    }
                }
                return 0;
            }
            catch
            {
                return 0;
            }
        }
    }
}