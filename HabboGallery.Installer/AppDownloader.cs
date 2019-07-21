using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.IO.Compression;

namespace HabboGalleryInstaller
{
    public static class AppDownloader
    {
        public static async Task<string> DownloadZipAsync(string downloadLocation)
        {
            string pathToZip = Path.GetTempFileName();

            using (WebClient client = new WebClient())
            {
                await client.DownloadFileTaskAsync(downloadLocation, pathToZip);
            }

            return pathToZip;
        }
    }
}
