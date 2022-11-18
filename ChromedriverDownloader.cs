using OpenQA.Selenium.Chrome;
using System.IO.Compression;

namespace Chromedriver_Downloader
{
    public static class ChromedriverDownloader
    {
        private static readonly string DEFAULT_STORAGE = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "ChromedriverDownloader");
        
        /// <summary>
        /// Get ChromeDriver. If it was already downloaded locally, use the local one. Download it from Google's servers and store it locally otherwise.
        /// </summary>
        /// <param name="options"></param>
        /// <param name="forceDownload">true to always download a driver from Google's website</param>
        /// <returns></returns>
        public static async Task<ChromeDriver> GetDriver(ChromeOptions? options, bool forceDownload = false)
        {
            string chromePath = Utilities.GetChromePath();
            int version = Utilities.GetChromeVersion(chromePath);

            string localVersionDir = Path.Combine(DEFAULT_STORAGE, version.ToString());
            string localVersionPath = Path.Combine(localVersionDir, "chromedriver.exe");

            if (forceDownload)
            {
                Directory.Delete(localVersionDir, true);
            }
            else if (File.Exists(localVersionPath))
            {
                return new ChromeDriver(localVersionPath, options);
            }

            await DownloadMissingDriver(version, localVersionDir);
            return new ChromeDriver(localVersionPath, options);

        }

        /// <summary>
        /// Download chromedriver.exe for the provided chromeVersion and store it in the destinationDir.
        /// </summary>
        /// <param name="chromeVersion">version of Google Chrome to automate</param>
        /// <param name="destinationDir">to store chromedriver.exe</param>
        private static async Task DownloadMissingDriver(int chromeVersion, string destinationDir)
        {
            using HttpClient client = new HttpClient();

            // Get the chromedriver compatible version for our Google chrome version
            using HttpResponseMessage versionResponse = await client.GetAsync(Utilities.GetChromedriverLatestUrl(chromeVersion.ToString()));
            versionResponse.EnsureSuccessStatusCode();

            string driverVersion = await versionResponse.Content.ReadAsStringAsync();

            // Download chromedriver
            using HttpResponseMessage driverZipResponse = await client.GetAsync(Utilities.GetChromedriverDownloadUrl(driverVersion));
            driverZipResponse.EnsureSuccessStatusCode();

            string zipFilePath = Path.Combine(DEFAULT_STORAGE, "chromedriver.zip");
            Directory.CreateDirectory(destinationDir);
            using FileStream zipFileCreate = File.Create(zipFilePath);
            
            using Stream driverZipSteam = await driverZipResponse.Content.ReadAsStreamAsync();
            driverZipSteam.Seek(0, SeekOrigin.Begin);
            driverZipSteam.CopyTo(zipFileCreate);
            driverZipSteam.Close();

            zipFileCreate.Close();

            ZipFile.ExtractToDirectory(zipFilePath, destinationDir);
            File.Delete(zipFilePath);
        }
    }
}
