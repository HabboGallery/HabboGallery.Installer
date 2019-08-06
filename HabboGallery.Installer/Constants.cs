using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HabboGalleryInstaller
{
    public static class Constants
    {
        public const string APP_NAME = "HabboGallery";
        public const string ICON_FILENAME = "icon.ico";
        public const string APP_FILENAME = "HabboGallery.Desktop.exe";
        public const string HANDLER_FILENAME = "HabboGallerySchemeHandler.exe";
        public const string SHORTCUT_DESCRIPTION = "A shortcut to HabboGallery Desktop";
        public const string CERT_ISSUER = "HabboGallery";
        public const string ROOT_CERT_NAME = "HabboGallery Root Certificate";
        public const string DOWNLOAD_PATH = "http://exp.test/downloads/HabboGallery.Desktop.zip";
        public const string PROTOCOL_VALUE = "URL: HabboGallery Protocol";
        public const string FILES_MISSING_MESSAGE = "One or more required files could not be found.";
        public const string SUBKEY_SHELL_OPEN_COMMAND = @"shell\open\command";
        public const string KEY_PROTOCOL_VALUE = "URL Protocol";
        public const string PROTOCOL_ARGS = " %1";
    }
}
