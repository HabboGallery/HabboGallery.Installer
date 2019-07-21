using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HabboGalleryInstaller
{
    public static class GuideMessages
    {
        public const string TEXT_CUSTOM_OR_START = "Click again to start the standard installation, or customize it first.";
        public const string BUTTON_START_INSTALL = "Install HabboGallery";

        public const string TEXT_DOWNLOADING = "Give it a second, HabboGallery is now being downloaded..";
        public const string TEXT_EXTRACTING = "Extracting files from ZIP archive..";

        public const string TEXT_INSTALL_CERT_INIT = "Next up, we need to install a certificate!";
        public const string BUTTON_INSTALL_CERT_INIT = "Let's do it";

        public const string TEXT_INSTALL_CERT_EXPLAIN = "After pressing \"Install\", click \"Yes\" when a security warning shows up.";
        public const string BUTTON_INSTALL_CERT_EXPLAIN = "Install";

        public const string TEXT_INSTALL_COMPLETE = "Yay! You're done :-)";
        public const string BUTTON_INSTALL_COMPLETE = "Complete Installation";

        public const string TEXT_FILE_MISSING_FAIL = "Installation failed. One or more files are missing.";
        public const string TEXT_CERT_INSTALL_FAIL = "Installation failed. Certification installation went wrong.";
    }
}
