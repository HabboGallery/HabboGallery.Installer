using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HabboGalleryInstaller
{
    public enum InstallState
    {
        Initial,
        Starting,
        Downloaded,
        InstallingCert,
        Done,
    }
}
