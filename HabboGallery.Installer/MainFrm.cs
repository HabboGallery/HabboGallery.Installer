﻿using System.Diagnostics;
using System.IO.Compression;
using System.Security.Principal;

using Microsoft.Win32;

using HabboGallery.Installer.Interop;

namespace HabboGallery.Installer;

//TODO: Add options..
public partial class MainFrm : Form
{
    private readonly HttpClient _client;

    private const string ApplicationName = "HabboGallery";
    private const string GithubOrganizationUrl = "https://github.com/HabboGallery/";

    private const string DesktopName = "HabboGallery.Desktop";
    private const string SchemeHandlerName = "HabboGallery.SchemeHandler";

    private readonly DirectoryInfo _applicationDirectory;

    public bool HasAdminPrivileges { get; }

    public MainFrm()
    {
        using var identity = WindowsIdentity.GetCurrent();
        HasAdminPrivileges = new WindowsPrincipal(identity).IsInRole(WindowsBuiltInRole.Administrator);

        if (!HasAdminPrivileges)
            ShowElevatePermissionsDialog();

        _client = new HttpClient
        {
            BaseAddress = new Uri(GithubOrganizationUrl)
        };

        string appPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), ApplicationName);
        _applicationDirectory = Directory.CreateDirectory(appPath);

        InitializeComponent();
    }

    private void ShowElevatePermissionsDialog()
    {
        TaskDialogCommandLinkButton buttonRestart = new()
        {
            Text = "&Restart the HabboGallery Installer with administrator rights",
            ShowShieldIcon = true
        };

        TaskDialogCommandLinkButton cancelButton = new()
        {
            Text = "&Cancel the installation"
        };

        TaskDialogPage elevatePermissionsPage = new()
        {
            Icon = TaskDialogIcon.Shield,
            Heading = "HabboGallery Installer requires elevated permissions.",
            Text = "Installer needs sufficient rights to create an shortcut and to register the \"habbogallery://\" URL-scheme.",

            Buttons =
            {
                TaskDialogButton.Cancel,
                buttonRestart,
                cancelButton
            },
            DefaultButton = cancelButton,
        };

        var resultButton = TaskDialog.ShowDialog(this, elevatePermissionsPage);
        if (resultButton == buttonRestart)
        {
            Process proc = new();
            proc.StartInfo.FileName = Environment.GetCommandLineArgs()[0];
            proc.StartInfo.UseShellExecute = true;
            proc.StartInfo.Verb = "runas";
            proc.Start();
        }
        Environment.Exit(0);
    }
    private void EnsureNoRunningInstances()
    {
        Process[] processes;
        while ((processes = Process.GetProcessesByName(DesktopName)).Length > 0)
        {
            string processDetails = string.Join("\r\n", processes.Select(p => $"{p.ProcessName} [{p.Id}]"));

            TaskDialogCommandLinkButton killInstanceButton = new()
            {
                Text = "&Close the running process",
                DescriptionText = "This will try to terminate the running HabboGallery application!"
            };
            TaskDialogCommandLinkButton retryButton = new()
            {
                Text = "&Retry",
                DescriptionText = "This will attempt to continue the installation process"
            };

            TaskDialogPage instanceIsRunningPage = new()
            {
                Icon = TaskDialogIcon.ShieldWarningYellowBar,
                Heading = "HabboGallery Desktop is running in the background.",
                Text = "In order to update the existing HabboGallery desktop installation, any existing instances of it must be closed!",

                Buttons =
                {
                    TaskDialogButton.Cancel,
                    killInstanceButton,
                    retryButton
                },
                DefaultButton = killInstanceButton,

                Expander = new TaskDialogExpander(processDetails)
            };

            var resultButton = TaskDialog.ShowDialog(this, instanceIsRunningPage);

            if (resultButton == retryButton) continue;
            else if (resultButton == killInstanceButton)
            {
                foreach (var process in processes)
                {
                    process.Kill();
                    process.WaitForExit(TimeSpan.FromSeconds(5));
                }
            }
            else Environment.Exit(0); //Cancel
        }
    }

    private void InstallBtn_Click(object sender, EventArgs e)
    {
        // Check for running instances of the desktop application.
        EnsureNoRunningInstances();

        //TODO: If it dies, allow close lol
        var installationProgressCloseButton = TaskDialogButton.Close;
        installationProgressCloseButton.Enabled = false;

        TaskDialogPage installationProgressPage = new()
        {
            Caption = ApplicationName,
            Heading = "Installing...",
            Text = "Please wait while the operation is in progress.",
            Icon = TaskDialogIcon.Information,

            ProgressBar = new TaskDialogProgressBar(TaskDialogProgressBarState.Marquee),
            Expander = new TaskDialogExpander("Initializing...")
            {
                Position = TaskDialogExpanderPosition.AfterFootnote
            },
            Buttons = { installationProgressCloseButton }
        };

        TaskDialogPage finishedPage = new()
        {
            Caption = ApplicationName,
            Heading = "Success!",
            Text = "The HabboGallery Desktop application has been installed succesfully.",
            Icon = TaskDialogIcon.ShieldSuccessGreenBar,
            Buttons = { TaskDialogButton.Close }
        };

        installationProgressPage.Created += async (s, e) =>
        {
            var progressBar = installationProgressPage.ProgressBar;

            await foreach (int progressValue in DownloadAndInstallAsync())
            {
                if (progressBar.State == TaskDialogProgressBarState.Marquee)
                    progressBar.State = TaskDialogProgressBarState.Normal;

                progressBar.Value = progressValue;
                installationProgressPage.Expander.Text = $"Progress: {progressValue} %";
            }

            installationProgressPage.Navigate(finishedPage);
        };

        finishedPage.Destroyed += (s, e) => Environment.Exit(0);

        var result = TaskDialog.ShowDialog(this, installationProgressPage);
    }

    private async IAsyncEnumerable<int> DownloadAndInstallAsync()
    {
        // Clean-up existing installation
        _applicationDirectory.Delete(true);
        _applicationDirectory.Create();
        yield return 25;

        // Download and extract the main desktop application
        using var desktopAssetStream = await DownloadAssetAsync(DesktopName, "zip").ConfigureAwait(false);
        await Task.Run(() =>
        {
            using var archive = new ZipArchive(desktopAssetStream, ZipArchiveMode.Read);
            archive.ExtractToDirectory(_applicationDirectory.FullName);
        });
        yield return 75;

        // Download and extract the SchemeHandler
        string schemeHandlerFileName = Path.Combine(_applicationDirectory.FullName, SchemeHandlerName + ".exe");

        using var schemeHandlerFs = File.Create(schemeHandlerFileName);
        using var schemeHandlerAssetStream = await DownloadAssetAsync(SchemeHandlerName, "exe").ConfigureAwait(false);

        await schemeHandlerAssetStream.CopyToAsync(schemeHandlerFs).ConfigureAwait(false);
        yield return 98;

        // Create desktop and start-menu shortcuts
        const string SHORTCUT_DESCRIPTION = "A shortcut to HabboGallery Desktop";

        string desktopAppFileName = Path.Combine(_applicationDirectory.FullName, DesktopName + ".exe");

        CreateShortcut(desktopAppFileName, ApplicationName, SHORTCUT_DESCRIPTION, Environment.SpecialFolder.DesktopDirectory);
        CreateShortcut(desktopAppFileName, ApplicationName, SHORTCUT_DESCRIPTION, Environment.SpecialFolder.StartMenu);
        yield return 99;

        // Register the URL Protocol
        RegisterURLProtocol(schemeHandlerFileName);
        yield return 100;
    }

    private Task<Stream> DownloadAssetAsync(string repositoryName, string assetFileExtension, CancellationToken cancellationToken = default)
        => _client.GetStreamAsync($"{repositoryName}/releases/latest/download/{repositoryName}.{assetFileExtension}", cancellationToken);

    private static void CreateShortcut(string targetPath, string name, string description, Environment.SpecialFolder folder)
    {
        string shortcutLocation = Path.Combine(Environment.GetFolderPath(folder), name + ".lnk");
        ShellLink.CreateAndSave(description, targetPath, shortcutLocation);
    }

    private static void RegisterURLProtocol(string myAppPath)
    {
        using RegistryKey key = Registry.ClassesRoot.CreateSubKey(ApplicationName);
        key.SetValue(string.Empty, "URL: HabboGallery Protocol");
        key.SetValue("URL Protocol", string.Empty);

        using RegistryKey commandKey = key.CreateSubKey("shell\\open\\command");
        commandKey.SetValue(string.Empty, $"\"{myAppPath}\" \"%1\"");
        commandKey.Close();

        key.Close();
    }
}
