using Eavesdrop;
using HabboGalleryInstaller.Helpers;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace HabboGalleryInstaller
{
    public partial class InstallFrm : Form, IMessageFilter
    {
        public InstallState State { get; set; }
        public string InstallPath { get; set; }

        private Dictionary<string, string> _paths;

        #region Draggable Controls
        private const int WmNclbuttondown = 0xA1;
        private const int HtCaption = 0x2;
        private const int WmLbuttondown = 0x0201;
        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();

        public bool DragControl(ref Message m)
        {
            if (m.Msg == WmLbuttondown && Control.FromHandle(m.HWnd) == LogoBx)
            {
                ReleaseCapture();
                SendMessage(Handle, WmNclbuttondown, HtCaption, 0);
                return true;
            }
            return false;
        }

        public bool PreFilterMessage(ref Message m)
        {
            return DragControl(ref m);
        }
        #endregion

        public InstallFrm()
        {
            InitializeComponent();
            TryInstallCertificate();
            State = InstallState.Initial;
            InstallPath = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles) + "\\" + Constants.APP_NAME;
            CustomPathTxt.Text = InstallPath;

            Application.AddMessageFilter(this);
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

                    string pathToTemp = await AppDownloader.DownloadZipAsync(Constants.DOWNLOAD_PATH);
                    MainInfoLbl.Text = GuideMessages.TEXT_EXTRACTING;
                    ZipFile.ExtractToDirectory(pathToTemp, InstallPath);
                    File.Delete(pathToTemp);

                    try
                    {
                        _paths = FindFilePathsInFolders(new string[] { Constants.ICON_FILENAME, Constants.APP_FILENAME, Constants.HANDLER_FILENAME });
                        if (!CustomizeTgl.Checked || DesktopShortTgl.Checked)
                            ShortcutBuilder.CreateShortcut(_paths[Constants.APP_FILENAME], _paths[Constants.ICON_FILENAME], Constants.APP_NAME, Constants.SHORTCUT_DESCRIPTION, Environment.SpecialFolder.Desktop);
                        if (!CustomizeTgl.Checked || StartMenuTgl.Checked)
                            ShortcutBuilder.CreateShortcut(_paths[Constants.APP_FILENAME], _paths[Constants.ICON_FILENAME], Constants.APP_NAME, Constants.SHORTCUT_DESCRIPTION, Environment.SpecialFolder.StartMenu);
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

                        RegisterURLProtocol(_paths[Constants.HANDLER_FILENAME]);
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
                        Process.Start(_paths[Constants.APP_FILENAME]);

                    Application.Exit();
                    break;
                }
            }
        }

        private bool TryInstallCertificate()
        {
            CertificateManager manager = new CertificateManager(Constants.CERT_ISSUER, Constants.ROOT_CERT_NAME);
            return manager.CreateTrustedRootCertificate();
        }

        static void RegisterURLProtocol(string myAppPath)
        {
            RegistryKey key = Registry.ClassesRoot.OpenSubKey(Constants.APP_NAME);

            if (key == null)
            {
                key = Registry.ClassesRoot.CreateSubKey(Constants.APP_NAME);
                key.SetValue(string.Empty, Constants.PROTOCOL_VALUE);
                key.SetValue(Constants.KEY_PROTOCOL_VALUE, string.Empty);

                key = key.CreateSubKey(Constants.SUBKEY_SHELL_OPEN_COMMAND);
                key.SetValue(string.Empty, myAppPath + Constants.PROTOCOL_ARGS);
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
                throw new FileNotFoundException(Constants.FILES_MISSING_MESSAGE);

            return result;
        }

        private void ExitBtn_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void SelectCustomPathBtn_Click(object sender, EventArgs e)
        {
            var result = InstallationPathDlg.ShowDialog();

            if (result != DialogResult.OK)
                return;

            InstallPath = InstallationPathDlg.SelectedPath;
            CustomPathTxt.Text = InstallPath;
        }
    }
}
