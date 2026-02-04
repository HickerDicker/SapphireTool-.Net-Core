using System;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Win32;
using OSVersionExtension;
using Wpf.Ui.Controls;

namespace SapphireTool.Classes;

public class Utils
{
    public static readonly string DownloadsFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads");

    public static readonly string VersionUrl = "https://raw.githubusercontent.com/HickerDicker/Version/refs/heads/main/version.txt";

    public static readonly string StartupFolder = Environment.GetFolderPath(Environment.SpecialFolder.Startup);

    public static readonly string UpdateFolder = Environment.GetFolderPath(Environment.SpecialFolder.Windows) + "\\Modules\\Updates";

    public static RegistryKey Def = Registry.LocalMachine.CreateSubKey("SOFTWARE\\Policies\\Microsoft\\Windows Defender");

    public static RegistryKey Def1 = Registry.LocalMachine.CreateSubKey("SOFTWARE\\Policies\\Microsoft\\Windows Defender\\Real-Time Protection");

    public static RegistryKey Def2 = Registry.LocalMachine.CreateSubKey("SOFTWARE\\Policies\\Microsoft\\Windows Defender Security Center\\Systray");
    public static void RestartExplorer()
    {
        foreach (var process in Process.GetProcessesByName("explorer"))
        {
            process.Kill();
        }
        Process.Start(new ProcessStartInfo
        {
            FileName = "explorer.exe",
            Arguments = "/NORESTART",
            UseShellExecute = true
        });
    }
    public static void RunCommand(string command, string arguments)
    {
        ProcessStartInfo startInfo = new ProcessStartInfo
        {
            FileName = command,
            Arguments = arguments,
            UseShellExecute = false,
            CreateNoWindow = false
        };
        Process process = new Process();
        process.StartInfo = startInfo;
        process.Start();
        process.WaitForExit();
    }

    public static async Task<bool> DownloadFile(string url, string filename)
    {
        try
        {
            using HttpClient client = new HttpClient();
            using HttpResponseMessage response = await client.GetAsync(url, HttpCompletionOption.ResponseHeadersRead);
            if (response.IsSuccessStatusCode)
            {
                using (Stream stream = await response.Content.ReadAsStreamAsync())
                {
                    using FileStream fileStream = new FileStream(filename, FileMode.Create, FileAccess.Write, FileShare.None);
                    await stream.CopyToAsync(fileStream);
                    return true;
                }
            }
            return false;
        }
        catch
        {
            return false;
        }
    }

    public static string GetOS()
    {
        string productName = (string)Registry.GetValue("HKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion", "ProductName", "");
        if (productName.Contains("Windows 10") && Convert.ToInt32((string)Registry.GetValue("HKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion", "CurrentBuild", "")) >= 22000)
        {
            productName = productName.Replace("Windows 10", "Windows 11");
        }
        return productName;
    }

    public static string GetOSVersion()
    {
        return $"{OSVersion.GetOSVersion().Version.Build}.{OSVersion.MajorVersion10Properties().UBR ?? "0"}";
    }

    public static string GetEdition()
    {
        return Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion")?.GetValue("EditionSubVersion")?.ToString();
    }

    public static async Task<string> GetLatestVersion()
    {
        using HttpClient client = new HttpClient();
        HttpResponseMessage obj = await client.GetAsync(VersionUrl);
        obj.EnsureSuccessStatusCode();
        return await obj.Content.ReadAsStringAsync();
    }

    public static bool IsNewVersionAvailable(string currentVersion, string latestVersion)
    {
        latestVersion = latestVersion.Trim();
        return !string.Equals(latestVersion, currentVersion);
    }

    public static void TweakDisableDefender()
    {
        // windows updates technically bring back defender so this is needed!
        Def.SetValue("DisableAntiSpyware", 1, RegistryValueKind.DWord);
        Def1.SetValue("DisableBehaviorMonitoring", 1, RegistryValueKind.DWord);
        Def1.SetValue("DisableOnAccessProtection", 1, RegistryValueKind.DWord);
        Def1.SetValue("DisableScanOnRealtimeEnable", 1, RegistryValueKind.DWord);
        Def1.SetValue("DisableRealtimeMonitoring", 1, RegistryValueKind.DWord);
        Registry.SetValue("HKEY_CURRENT_USER\\Software\\Microsoft\\Windows\\CurrentVersion\\AppHost", "EnableWebContentEvaluation", 0, RegistryValueKind.DWord);
        Registry.SetValue("HKEY_CURRENT_USER\\Software\\Classes\\Local Settings\\Software\\Microsoft\\Windows\\CurrentVersion\\AppContainer\\Storage\\microsoft.microsoftedge_8wekyb3d8bbwe\\MicrosoftEdge\\PhishingFilter", "EnabledV9", 0, RegistryValueKind.DWord);
        Registry.SetValue("HKEY_LOCAL_MACHINE\\SOFTWARE\\Policies\\Microsoft\\Windows Defender", "DisableAntiSpyware", 1, RegistryValueKind.DWord);
        Registry.SetValue("HKEY_LOCAL_MACHINE\\SOFTWARE\\Policies\\Microsoft\\Windows Defender\\Spynet", "SpyNetReporting", 0, RegistryValueKind.DWord);
        Registry.SetValue("HKEY_LOCAL_MACHINE\\SOFTWARE\\Policies\\Microsoft\\Windows Defender\\Spynet", "SubmitSamplesConsent", 2, RegistryValueKind.DWord);
        Registry.SetValue("HKEY_LOCAL_MACHINE\\SOFTWARE\\Policies\\Microsoft\\Windows Defender\\Spynet", "DontReportInfectionInformation", 1, RegistryValueKind.DWord);
        Def2.SetValue("HideSystray", 1, RegistryValueKind.DWord);
    }

    public static async Task RunCommandAsync(string filename, string arguments, bool isElevated)
    {
        ProcessStartInfo startInfo = new ProcessStartInfo(filename, arguments)
        {
            UseShellExecute = false,
            CreateNoWindow = true,
            RedirectStandardOutput = true,
            RedirectStandardError = true
        };
        using Process process = new Process
        {
            StartInfo = startInfo
        };
        process.Start();
        await process.WaitForExitAsync();
    }

    public static async Task ShowDialog(string title, string content)
    {
        await new MessageBox
        {
            Title = title,
            Content = content
        }.ShowDialogAsync();
    }
}
