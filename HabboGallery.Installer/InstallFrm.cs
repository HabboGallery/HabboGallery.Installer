using Eavesdrop;
using HabboGalleryInstaller.Helpers;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Windows.Forms;

namespace HabboGalleryInstaller
{
    public partial class InstallFrm : Form
    {
        public InstallState State { get; set; }
        public string InstallPath { get; set; }

        public const string APP_NAME = "HabboGallery";
        public const string ICON_FILENAME = "icon.ico";
        public const string APP_FILENAME = "HabboGallery.Desktop.exe";
        public const string HANDLER_FILENAME = "HabboGallerySchemeHandler.exe";
        public const string SHORTCUT_DESCRIPTION = "A shortcut to HabboGallery Desktop";
        public const string CERT_ISSUER = "HabboGallery";
        public const string ROOT_CERT_NAME = "HabboGallery Root Certificate";
        public const string DOWNLOAD_PATH = "http://exp.test/downloads/HabboGallery.Desktop.zip";

        private Dictionary<string, string> _paths;

        public InstallFrm()
        {
            InitializeComponent();
            TryInstallCertificate();
            State = InstallState.Initial;
            InstallPath = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles) + "\\" + APP_NAME;
            CustomPathTxt.Text = InstallPath;
        }

        private void CustomizeTgl_CheckedChanged(object sender, EventArgs e)
            => CustomInstallationPnl.Visible = CustomizeTgl.Checked;

        private async void MainContinueButton_Click(object sender, EventArgs e)
        {
            switch (State)
            {
                case InstallState.Initial:
                {
                    State = InstallState.Starting;
                    MainInfoLbl.Text = GuideMessages.TEXT_CUSTOM_OR_START;
                    MainContinueButton.Text = GuideMessages.BUTTON_START_INSTALL;
                    CustomizeTgl.Visible = true;
                    break;
                }
                case InstallState.Starting:
                {
                    InstallPath = CustomPathTxt.Text;
                    CustomizeTgl.Visible = false;
                    CustomInstallationPnl.Visible = false;
                    MainContinueButton.Visible = false;
                    MainInfoLbl.Text = GuideMessages.TEXT_DOWNLOADING;

                    string pathToTemp = await AppDownloader.DownloadZipAsync(DOWNLOAD_PATH);
                    MainInfoLbl.Text = GuideMessages.TEXT_EXTRACTING;
                    ZipFile.ExtractToDirectory(pathToTemp, InstallPath);
                    File.Delete(pathToTemp);

                    try
                    {
                        _paths = FindFilePathsInFolders(new string[] { ICON_FILENAME, APP_FILENAME, HANDLER_FILENAME });
                        if (!CustomizeTgl.Checked || DesktopShortTgl.Checked)
                            ShortcutBuilder.CreateShortcut(_paths[APP_FILENAME], _paths[ICON_FILENAME], APP_NAME, SHORTCUT_DESCRIPTION, Environment.SpecialFolder.Desktop);
                        if (!CustomizeTgl.Checked || StartMenuTgl.Checked)
                            ShortcutBuilder.CreateShortcut(_paths[APP_FILENAME], _paths[ICON_FILENAME], APP_NAME, SHORTCUT_DESCRIPTION, Environment.SpecialFolder.StartMenu);
                        MainInfoLbl.Text = GuideMessages.TEXT_INSTALL_CERT_INIT;
                        MainContinueButton.Text = GuideMessages.BUTTON_INSTALL_CERT_INIT;
                        MainContinueButton.Visible = true;
                        State = InstallState.Downloaded;
                    }
                    catch (FileNotFoundException)
                    {
                        MainInfoLbl.Text = GuideMessages.TEXT_FILE_MISSING_FAIL;
                    }
                    break;
                }
                case InstallState.Downloaded:
                {
                    MainInfoLbl.Text = GuideMessages.TEXT_INSTALL_CERT_EXPLAIN;
                    MainContinueButton.Text = GuideMessages.BUTTON_INSTALL_CERT_EXPLAIN;
                    State = InstallState.InstallingCert;
                    break;
                }
                case InstallState.InstallingCert:
                {
                    MainContinueButton.Visible = false;
                    if (TryInstallCertificate())
                    {
                        MainInfoLbl.Text = GuideMessages.TEXT_INSTALL_COMPLETE;
                        MainContinueButton.Text = GuideMessages.BUTTON_INSTALL_COMPLETE;
                        MainContinueButton.Visible = true;
                        State = InstallState.Done;

                        RegisterURLProtocol(_paths[HANDLER_FILENAME]);
                    }
                    else
                    {
                        MainInfoLbl.Text = GuideMessages.TEXT_CERT_INSTALL_FAIL;
                    }
                    break;
                }
                case InstallState.Done:
                {
                    if (LaunchAppTgl.Checked)
                        Process.Start(_paths[APP_FILENAME]);

                    Application.Exit();
                    break;
                }
            }
        }

        private bool TryInstallCertificate()
        {
            CertificateManager manager = new CertificateManager(CERT_ISSUER, ROOT_CERT_NAME);
            return manager.CreateTrustedRootCertificate();
        }

        static void RegisterURLProtocol(string myAppPath)
        {
            RegistryKey key = Registry.ClassesRoot.OpenSubKey("HabboGallery");

            if (key == null)
            {
                key = Registry.ClassesRoot.CreateSubKey("HabboGallery");
                key.SetValue(string.Empty, "URL: HabboGallery Protocol");
                key.SetValue("URL Protocol", string.Empty);

                key = key.CreateSubKey(@"shell\open\command");
                key.SetValue(string.Empty, myAppPath + " " + "%1");
            }

            key.Close();
        }

        private Dictionary<string, string> FindFilePathsInFolders(string[] fileNames)
        {
            var result = new Dictionary<string, string>(fileNames.Length);

            foreach (string name in fileNames)
            {
                var foundPaths = Directory.GetFiles(InstallPath, name, SearchOption.AllDirectories);
                if (foundPaths.Length > 0)
                {
                    result.Add(name, foundPaths.First());
                }
            }

            if (result.Count < fileNames.Length)
                throw new FileNotFoundException("One or more required files could not be found.");

            return result;
        }
    }
}
