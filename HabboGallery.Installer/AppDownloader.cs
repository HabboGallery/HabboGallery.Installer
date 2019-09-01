using System.IO;
using System.Net;
using System.Threading.Tasks;

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
