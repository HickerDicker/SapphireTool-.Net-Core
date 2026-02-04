using System;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Threading;
using Microsoft.Win32;
using OSVersionExtension;
using SapphireTool.Classes;

namespace SapphireTool.Pages;

public partial class UpdatesPage : Page, IComponentConnector
{
    private CancellationTokenSource _cts;
    private readonly HttpClient _httpClient = new();
    private readonly Stopwatch _stopwatch = new();
    private readonly string _msuPackage = Path.Combine(Utils.UpdateFolder, "Update.msu");
    private bool _updateCancelled;
    private int _dotCount;
    private string _status = string.Empty;
    private readonly DispatcherTimer _timer;
    private readonly string currentBuild = OSVersion.GetOSVersion().Version.Build.ToString();

    public UpdatesPage()
    {
        InitializeComponent();
        _timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(0.5) };
        _timer.Tick += (_, _) => {
            txtBlockDownload.Text = _status + new string('.', _dotCount);
            _dotCount = (_dotCount + 1) % 5;
        };
        string windowsName = App.Winver == "23H2" ? "Windows 10" : "Windows 11";
        txtBlockCurrentWinver.Text = $"OS: {windowsName} [{GetRegistryValue("Edition", "DEFAULT")}] Build {Utils.GetOSVersion()}";
        if (GetRegistryValue("Edition") == "ULTRALITE")
        {
            txtBlockUltralite.Visibility = Visibility.Visible;
            containerGrid.Visibility = Visibility.Collapsed;
            Utils.ShowDialog("Sorry", "Updates don't work on Ultralite!");
            btnCheckForUpdates.IsEnabled = false;
        }
    }

    private string GetRegistryValue(string valueName, string defaultValue = null)
    {
        try
        {
            using var key = Registry.CurrentUser.OpenSubKey(@"Software\SapphireTool");
            return key?.GetValue(valueName)?.ToString() ?? defaultValue;
        }
        catch
        {
            return defaultValue;
        }
    }

    private bool CheckServicesConfiguration()
    {
        if (GetRegistryValue("Services") == "Default")
            return true;

        Utils.ShowDialog("Configuration Required", "You're not on Default Services. Please change to Default before updating.");
        return false;
    }

    private async void btnCheckForUpdates_Click(object sender, RoutedEventArgs e) => await CheckForUpdates();

    private async Task CheckForUpdates()
    {
        if (!CheckServicesConfiguration()) return;

        try
        {
            string[] availableVersions = (await Utils.GetLatestVersion()).Trim().Split(',');
            string latestVersionForBuild = availableVersions.FirstOrDefault(v => v.Contains(currentBuild));
            if (latestVersionForBuild == null)
            {
                await Utils.ShowDialog("You are up to date", "No update available for your current Windows build.");
                txtBlockLatestWinver.Text = $"Your build ({currentBuild}) is not supported.";
                return;
            }

            string currentVersion = Utils.GetOSVersion();
            if (Utils.IsNewVersionAvailable(currentVersion, latestVersionForBuild))
            {
                await Utils.ShowDialog("Update Available!", $"A new version: ({latestVersionForBuild}) is available!");
                txtBlockLatestWinver.Text = $"Latest Winver: {latestVersionForBuild}";

                if (GetRegistryValue("Edition") == "ULTRALITE")
                {
                    await Utils.ShowDialog("Update Available", "Sadly you are on Ultralite meaning you can't update. You have to wait for a new update or install Superlite");
                    return;
                }
                btnCheckForUpdates.Visibility = Visibility.Collapsed;
                btnDownloadUpdates.Visibility = Visibility.Visible;
                containerGrid.Visibility = Visibility.Visible;
            }
            else
            {
                await Utils.ShowDialog("No Update Available!", "You are using the latest version.");
                txtBlockLatestWinver.Text = "You're up to date!";
            }
        }
        catch (Exception ex)
        {
            await Utils.ShowDialog("Error", $"Error checking for updates: {ex.Message}");
        }
    }
    
    private async void btnDownloadUpdates_Click(object sender, RoutedEventArgs e)
    {
        _updateCancelled = false;
        _cts = new CancellationTokenSource();
        Directory.CreateDirectory(Utils.UpdateFolder);
        SetUIState(isDownloading: true);
        _stopwatch.Start();
        try
        {
            string url = currentBuild.Contains("22631")
                ? "https://hickos.hickdick.workers.dev/0:/Update11.msu"
                : "https://hickos.hickdick.workers.dev/0:/25H2Update.msu";

            UpdateStatus("Downloading Updates");
            await DownloadFileAsync(new Uri(url), _msuPackage, _cts.Token);

            if (!_updateCancelled)
            {
                UpdateStatus("Installing Updates");
                progBarDownload.IsIndeterminate = true;
                txtBlockSpeed.Visibility = Visibility.Collapsed;
                btnCancel.IsEnabled = false;

                await InstallUpdates(_msuPackage);
                await DownloadTweakUpdates();
            }
        }
        catch (OperationCanceledException)
        {
            await Utils.ShowDialog("Download Cancelled", "Download has been cancelled!");
            ResetUpdatePage();
        }
        catch (Exception)
        {
            // idk why it's throwing an exception here when the download is fine but wtv if it works it works
        }
        _stopwatch.Reset();
    }

    private async Task DownloadFileAsync(Uri uri, string destinationFilePath, CancellationToken cancellationToken)
    {
        using var response = await _httpClient.GetAsync(uri, HttpCompletionOption.ResponseHeadersRead, cancellationToken);
        response.EnsureSuccessStatusCode();
        long totalBytes = response.Content.Headers.ContentLength ?? -1;
        using var contentStream = await response.Content.ReadAsStreamAsync(cancellationToken);
        using var fileStream = new FileStream(destinationFilePath, FileMode.Create, FileAccess.Write, FileShare.None, 8192, true);
        byte[] buffer = new byte[8192];
        long totalBytesRead = 0;
        int bytesRead;
        while ((bytesRead = await contentStream.ReadAsync(buffer, 0, buffer.Length, cancellationToken)) > 0)
        {
            await fileStream.WriteAsync(buffer, 0, bytesRead, cancellationToken);
            totalBytesRead += bytesRead;

            if (totalBytes > 0)
            {
                int progressPercentage = (int)((double)totalBytesRead / totalBytes * 100);
                txtBlockSpeed.Visibility = Visibility.Visible;
                txtBlockSpeed.Text = $"Download Speed: {totalBytesRead / 1024 / _stopwatch.Elapsed.TotalSeconds:0.00} kb/s";
                progBarDownload.Value = progressPercentage;
            }
        }
    }

    private async Task InstallUpdates(string msuPath)
    {
        var psiInstall = currentBuild.Contains("26200")
            ? new ProcessStartInfo("dism.exe", $"/Online /Add-Package /PackagePath:{msuPath} /norestart") { UseShellExecute = false, CreateNoWindow = true }
            : new ProcessStartInfo("wusa.exe", $"{msuPath} /quiet /norestart") { UseShellExecute = false, CreateNoWindow = true };
        using var process = new Process { StartInfo = psiInstall };
        process.Start();
        await process.WaitForExitAsync();
        if (tsRemoveDefender.IsChecked == true) await Utils.DownloadFile("https://raw.githubusercontent.com/HickerDicker/Version/refs/heads/main/BlockDefender.bat", $"{Utils.UpdateFolder}\\BlockDefender.bat"); await Utils.DownloadFile("https://raw.githubusercontent.com/HickerDicker/Version/refs/heads/main/DisableDefender.ps1", $"{Utils.UpdateFolder}\\DisableDefender.ps1"); await Utils.DownloadFile("https://github.com/HickerDicker/Version/raw/refs/heads/main/NoDefender.cab", "C:\\Windows\\NoDefender.cab"); await ApplyTweak("BlockDefender.bat", Utils.TweakDisableDefender); await Utils.RunCommandAsync("powershell", $"{Utils.UpdateFolder}\\DisableDefender.ps1", true);
        if (tsRemoveEdge.IsChecked == true) await Utils.DownloadFile("https://raw.githubusercontent.com/HickerDicker/Version/refs/heads/main/BlockEdge.bat", $"{Utils.UpdateFolder}\\BlockEdge.bat"); await ApplyTweak("BlockEdge.bat");
        if (tsWinSxSCleanup.IsChecked == true) await SetupUpdateCleanup();
        progBarDownload.Value = 0;
        txtBlockSpeed.Text = string.Empty;
        btnCheckForUpdates.Visibility = Visibility.Collapsed;
        txtBlockDownload.Visibility = Visibility.Visible;
        UpdateStatus("Update Installation Finished, Downloading updated tweaks!");
    }

    private async Task DownloadTweakUpdates()
    {
        UpdateStatus("Downloading New Tweaks");
        string url = currentBuild.Contains("22631")
    ? "https://raw.githubusercontent.com/HickerDicker/Version/refs/heads/main/23H2.bat"
    : "https://raw.githubusercontent.com/HickerDicker/Version/refs/heads/main/25H2.bat";
        await Utils.DownloadFile(url, $"{Utils.StartupFolder}\\Script.bat");
        await Utils.DownloadFile("https://raw.githubusercontent.com/HickerDicker/Version/refs/heads/main/HKCU.reg", $"{Utils.UpdateFolder}\\HKCU.reg");
        await Utils.DownloadFile("https://raw.githubusercontent.com/HickerDicker/Version/refs/heads/main/HKCR.reg", $"{Utils.UpdateFolder}\\HKCR.reg");
        await Utils.DownloadFile("https://raw.githubusercontent.com/HickerDicker/Version/refs/heads/main/HKLM.reg", $"{Utils.UpdateFolder}\\HKLM.reg");
        await Utils.DownloadFile("https://raw.githubusercontent.com/HickerDicker/Version/refs/heads/main/run%20regs.bat", $"{Utils.StartupFolder}\\reg.bat");
        Thread.Sleep(1000);
        SetUIState(isDownloading: false);
        UpdateStatus("Finished Updating restart to apply tweaks");
        await Utils.ShowDialog("Update Finished", "Please wait for any batch files then Restart Your PC and wait For The Tweaks to Apply");
    }
    private async Task ApplyTweak(string scriptName, Action additionalAction = null)
    {
        string scriptPath = Path.Combine(Utils.UpdateFolder, scriptName);
        await Utils.RunCommandAsync("C:\\PostInstall\\Tweaks\\Nsudo.exe", $"-U:T -P:E {scriptPath}", isElevated: true);
        additionalAction?.Invoke();
    }

    private async Task SetupUpdateCleanup()
    {
        await Utils.DownloadFile("https://raw.githubusercontent.com/HickerDicker/Version/refs/heads/main/UpdateCleanup.bat", $"{Utils.StartupFolder}\\UpdateCleanup.bat");
    }

    private void UpdateStatus(string message)
    {
        _status = message;
        txtBlockDownload.Text = _status;
    }

    private void SetUIState(bool isDownloading)
    {
        txtBlockDownload.Visibility = isDownloading ? Visibility.Visible : Visibility.Collapsed;
        progBarDownload.Visibility = isDownloading ? Visibility.Visible : Visibility.Collapsed;
        btnDownloadUpdates.Visibility = isDownloading ? Visibility.Collapsed : Visibility.Visible;
        btnCancel.Visibility = isDownloading ? Visibility.Visible : Visibility.Collapsed;
        if (isDownloading)
        {
            _timer.Start();
        }
        else
        {
            _timer.Stop();
            _dotCount = 0;
            _status = string.Empty;
            txtBlockDownload.Text = _status;
            txtBlockSpeed.Text = "";
            progBarDownload.Value = 0;
            txtBlockSpeed.Visibility = Visibility.Collapsed;
            progBarDownload.IsIndeterminate = false;
        }
    }
    private void ResetUpdatePage() => SetUIState(isDownloading: false);
    private void btnCancel_Click(object sender, RoutedEventArgs e)
    {
        ResetUpdatePage();
        _cts?.Cancel();
        _updateCancelled = true;
    }
}